using System.Collections.Generic;
using CommandLine;

namespace Codeworx.Rest.Tool
{
    public class Options
    {
        [Option('o', "output-dir", Default = "generated", HelpText = "The output directory for the generated proxies.")]
        public string OutputDir { get; set; }

        [Option('i', "source-projects", HelpText = "A list of projects to check for rest contracts.")]
        public IEnumerable<string> SourceProjects { get; set; }

        [Option('t', "target", HelpText = "Target project for the generated proxies.")]
        public string Target { get; set; }

        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
}