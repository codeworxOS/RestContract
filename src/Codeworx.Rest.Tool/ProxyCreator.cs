using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace Codeworx.Rest.Tool
{
    public class ProxyCreator
    {
        private Options _options;

        public ProxyCreator(Options options)
        {
            _options = options;
        }

        public async Task ProcessAsync()
        {
            var options = _options;

            var target = _options.Target ?? FindFallbackProjectFile();

            MSBuildLocator.RegisterDefaults();

            using (var workspace = MSBuildWorkspace.Create())
            {
                var project = await workspace.OpenProjectAsync(target);

                var builder = ImmutableList.CreateBuilder<InterfaceDeclarationSyntax>();

                foreach (var document in project.Documents)
                {
                    var root = await document.GetSyntaxRootAsync();

                    var interfaces = root.DescendantNodes()
                        .OfType<InterfaceDeclarationSyntax>()
                        .ToImmutableList();

                    builder.AddRange(interfaces);
                }

                var foundInterfaces = builder.ToImmutable();

                Console.WriteLine($"Found {foundInterfaces.Count} matching interfaces.");
            }
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
    }
}