using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codeworx.Rest.Tool.Extensions;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace Codeworx.Rest.Tool
{
    public class ProxyCreator
    {
        private Options _options;
        private Project _project;
        private MSBuildWorkspace _workspace;

        public ProxyCreator(Options options)
        {
            _options = options;
        }

        public async Task ProcessAsync()
        {
            var options = _options;

            var target = _options.Target ?? FindFallbackProjectFile();

            target = new FileInfo(target).FullName;

            WriteVerboseInfo($"Using project {target} as generation target.");
            WriteVerboseInfo($"MSBuildLocator executing...");

            MSBuildLocator.RegisterDefaults();

            using (_workspace = MSBuildWorkspace.Create())
            {
                var projectsBuilder = ImmutableList.CreateBuilder<Project>();
                _project = await _workspace.OpenProjectAsync(target);

                if (_options.SourceProjects.Any())
                {
                    foreach (var item in _options.SourceProjects)
                    {
                        WriteVerboseInfo($"Looking for project {item}.");

                        var found = _project.Solution.Projects.FirstOrDefault(p => p.Name == item);
                        if (found == null)
                        {
                            WriteErrorOutput($"Project {item} was not found. It needs to be referenced from the target project.");
                            Environment.Exit(404);
                        }

                        projectsBuilder.Add(found);
                        WriteVerboseInfo($"Found project {item}.");
                    }
                }
                else
                {
                    projectsBuilder.Add(_project);
                }

                var projects = projectsBuilder.ToImmutable();
                var interfacesPerProjectBuilder =
                    ImmutableDictionary.CreateBuilder<Project, ImmutableList<InterfaceDeclarationSyntax>>();

                foreach (var project in projects)
                {
                    var interfacesBuilder = ImmutableList.CreateBuilder<InterfaceDeclarationSyntax>();
                    foreach (var document in project.Documents)
                    {
                        var root = await document.GetSyntaxRootAsync();

                        var interfaces = root.DescendantNodes()
                            .OfType<InterfaceDeclarationSyntax>()
                            .Where(p => IsRestContract(p))
                            .ToImmutableList();

                        interfacesBuilder.AddRange(interfaces);
                    }

                    var allInterfacesPerProject = interfacesBuilder.ToImmutable();
                    interfacesPerProjectBuilder.Add(project, allInterfacesPerProject);
                }

                var interfacesPerProject = interfacesPerProjectBuilder.ToImmutable();
                var foundInterfaces = interfacesPerProject.SelectMany(pair => pair.Value).ToImmutableList();

                Console.WriteLine($"Found {foundInterfaces.Count} matching interfaces.");

                if (_options.Verbose)
                {
                    foreach (var item in foundInterfaces)
                    {
                        WriteVerboseInfo($"Found interface {item.Identifier.Text}");
                    }
                }

                foreach (var projectInterfacesPair in interfacesPerProject)
                {
                    var project = projectInterfacesPair.Key;
                    var interfaces = projectInterfacesPair.Value;

                    var compilation = await project.GetCompilationAsync();

                    foreach (var item in interfaces)
                    {
                        var syntaxTree = item.SyntaxTree;
                        var semanticModel = compilation.GetSemanticModel(syntaxTree);
                        var interfaceSymbol = semanticModel.GetDeclaredSymbol(item);

                        await WriteTargetProxyClassAsync(interfaceSymbol);
                    }
                }
            }

            _workspace = null;
            _project = null;
        }

        private static string FindFallbackProjectFile()
        {
            var projectFile = Directory.GetFiles(".", "*.csproj", SearchOption.TopDirectoryOnly);
            if (projectFile.Length != 1)
            {
                Console.WriteLine("Could not find a project file to use");
                Environment.Exit(404);
            }

            return projectFile[0];
        }

        private static void WriteErrorOutput(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"[Error] => {message}");
            Console.ResetColor();
        }

        private SyntaxNode CreateProxyClassSyntax(INamedTypeSymbol currentInterface, string className, IEnumerable<string> folders)
        {
            var targetNamespace = _project.DefaultNamespace;
            if (folders.Any())
            {
                targetNamespace = $"{targetNamespace}.{string.Join(".", folders.Select(x => $"{x.Substring(0, 1).ToUpper()}{x.Substring(1)}"))}";
            }

            var constructors = GenerateConstructors(currentInterface, className);
            var methods = GenerateMethods(currentInterface);

            var classDeclaration = SyntaxFactory.ClassDeclaration(className)
                                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                        .AddBaseListTypes(
                                            SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"Codeworx.Rest.Client.RestClient<{currentInterface.AssemblyQualifiedName()}>")),
                                            SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(currentInterface.AssemblyQualifiedName())))
                                        .AddMembers(constructors)
                                        .AddMembers(methods);

            var namespaceSyntax = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(targetNamespace))
                                        .AddMembers(classDeclaration)
                                        .NormalizeWhitespace();

            var result = SyntaxFactory.CompilationUnit()
                .AddAttributeLists(
                    SyntaxFactory.AttributeList()
                        .WithTarget(SyntaxFactory.AttributeTargetSpecifier(SyntaxFactory.Identifier("assembly")))
                        .AddAttributes(
                            SyntaxFactory.Attribute(SyntaxFactory.ParseName("Codeworx.Rest.RestProxy"))
                                .AddArgumentListArguments(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(currentInterface.AssemblyQualifiedName()))),
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName($"{targetNamespace}.{classDeclaration.Identifier.Text}"))))))
                .WithLeadingTrivia(
                    SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, "// <auto-generated />"))
                .AddMembers(namespaceSyntax);

            return result.NormalizeWhitespace();
        }

        private MemberDeclarationSyntax[] GenerateConstructors(INamedTypeSymbol currentInterface, string className)
        {
            var memberDeclarations = new List<MemberDeclarationSyntax>();

            var typedConstructor = SyntaxFactory.ConstructorDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("options")).WithType(SyntaxFactory.ParseTypeName($"Codeworx.Rest.Client.RestOptions<{currentInterface.AssemblyQualifiedName()}>")))
                .WithInitializer(
                    SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                        .AddArgumentListArguments(SyntaxFactory.Argument(SyntaxFactory.IdentifierName("options"))))
                .WithBody(SyntaxFactory.Block())
                .NormalizeWhitespace();
            memberDeclarations.Add(typedConstructor);

            var test = 1;

            // TODO add config switch.
            if (test > 2)
            {
                var untypedConstructor = SyntaxFactory.ConstructorDeclaration(className)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("options")).WithType(SyntaxFactory.ParseTypeName($"RestOptions")))
                    .WithInitializer(
                        SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                            .AddArgumentListArguments(SyntaxFactory.Argument(SyntaxFactory.IdentifierName("options"))))
                    .WithBody(SyntaxFactory.Block())
                    .NormalizeWhitespace();
                memberDeclarations.Add(untypedConstructor);
            }

            return memberDeclarations.ToArray();
        }

        private MemberDeclarationSyntax[] GenerateMethods(INamedTypeSymbol currentInterface)
        {
            var memberDeclarations = new List<MemberDeclarationSyntax>();

            var allInterfaces = currentInterface.AllInterfaces;
            allInterfaces = allInterfaces.Add(currentInterface);

            var methods = allInterfaces.SelectMany(interfaceSymbol => interfaceSymbol.GetMembers().OfType<IMethodSymbol>()).ToImmutableList();
            foreach (var method in methods)
            {
                var arguments = method.Parameters.Select(
                    parameter => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parameter.Name)));
                var callMethodLambda = SyntaxFactory.Argument(
                    SyntaxFactory.SimpleLambdaExpression(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("c")),
                        SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("c"),
                                    SyntaxFactory.IdentifierName(method.Name)))
                            .AddArgumentListArguments(arguments.ToArray())));

                var callAsync = SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("CallAsync"))
                    .AddArgumentListArguments(callMethodLambda);
                var returnStatement = SyntaxFactory.ReturnStatement(callAsync);

                var parameters = method.Parameters.Select(
                    parameter => SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameter.Name)).WithType(SyntaxFactory.ParseTypeName(parameter.Type.AssemblyQualifiedName())));
                var newMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(method.ReturnType.AssemblyQualifiedName()), method.Name)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(parameters.ToArray())
                    .AddBodyStatements(returnStatement)
                    .NormalizeWhitespace();

                memberDeclarations.Add(newMethod);
            }

            return memberDeclarations.ToArray();
        }

        private bool IsAttributeName(NameSyntax nameSyntax, string attributeName)
        {
            switch (nameSyntax)
            {
                case SimpleNameSyntax simple:
                    return simple.Identifier.Text == attributeName || simple.Identifier.Text == $"{attributeName}Attribute";

                case QualifiedNameSyntax qualified:
                    return IsAttributeName(qualified.Right, attributeName);
            }

            throw new NotSupportedException($"{nameSyntax.GetType()} not supported as AttributeName.");
        }

        private bool IsRestContract(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return interfaceDeclaration.AttributeLists.Any(p => p.Attributes.Any(x => IsAttributeName(x.Name, "RestRoute")));
        }

        private async Task WriteTargetProxyClassAsync(INamedTypeSymbol currentInterface)
        {
            await Task.Yield();

            var className = $"{currentInterface.Name.Substring(1)}Client";
            var fileName = $"{className}.cs";
            var folders = _options.OutputDir.Split('\\', '/');
            var filePath = Path.Combine(
                                new FileInfo(_project.FilePath).DirectoryName,
                                string.Join(@"\", folders),
                                fileName);

            WriteVerboseInfo($"Creating {className} in target file {filePath}.");

            var clientSyntax = CreateProxyClassSyntax(currentInterface, className, folders);

            Directory.CreateDirectory(new FileInfo(filePath).DirectoryName);
            using (var stream = File.Create(filePath))
            using (var writer = new StreamWriter(stream))
            {
                clientSyntax.WriteTo(writer);
            }

            // TODO add switch in options to enable adding the project item as well.
            var test = 1 > 2;
            if (test)
            {
                var newDocument = _project.AddDocument(fileName, clientSyntax, folders, filePath);
                var applied = _workspace.TryApplyChanges(newDocument.Project.Solution);
                WriteVerboseInfo($"Applying generated class: Success({applied})");
                _project = _workspace.CurrentSolution.GetProject(_project.Id);
            }
        }

        private void WriteVerboseInfo(string message)
        {
            if (_options.Verbose)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}