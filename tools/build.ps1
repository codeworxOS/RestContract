Import-Module -Name "./Build-Versioning.psm1"


$projects = "..\src\Codeworx.Rest.Primitives\Codeworx.Rest.Primitives.csproj",
            "..\src\Codeworx.Rest.Client\Codeworx.Rest.Client.csproj", 
            "..\src\Codeworx.Rest.AspNetCore\Codeworx.Rest.AspNetCore.csproj", 
            "..\src\Codeworx.Rest.Tool\Codeworx.Rest.Tool.csproj"

New-NugetPackages `
    -Projects $projects `
    -NugetServerUrl "http://www.nuget.org/api/v2" `
    -VersionPackage "Codeworx.Rest.Primitives" `
    -VersionFilePath "..\version.json" `
    -OutputPath "..\dist\nuget\" `
    -MsBuildParams "SourceLinkCreate=true;SignAssembly=true;AssemblyOriginatorKeyFile=..\..\private\signkey.snk"