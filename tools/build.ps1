Import-Module -Name "./Build-Versioning.psm1"


$projects = "..\src\Codeworx.Rest.Primitives\Codeworx.Rest.Primitives.csproj",
            "..\src\Codeworx.Rest.Client\Codeworx.Rest.Client.csproj", 
            "..\src\Codeworx.Rest.Formatters.Protobuf\Codeworx.Rest.Formatters.Protobuf.csproj", 
            "..\src\Codeworx.Rest.AspNetCore\Codeworx.Rest.AspNetCore.csproj", 
            "..\src\Codeworx.Rest.Tool.Cli\Codeworx.Rest.Tool.Cli.csproj"

New-NugetPackages `
    -Projects $projects `
    -NugetServerUrl "https://api.nuget.org/v3/index.json" `
    -VersionPackage "Codeworx.Rest.Primitives" `
    -VersionFilePath "..\version.json" `
    -OutputPath "..\dist\nuget\" `
    -MsBuildParams "SourceLinkCreate=true;SignAssembly=true;AssemblyOriginatorKeyFile=..\..\private\restcontract_signkey.snk"