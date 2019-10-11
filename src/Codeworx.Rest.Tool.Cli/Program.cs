using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Codeworx.Rest.Tool.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var location = new FileInfo(typeof(Program).Assembly.Location);
            var path = Path.Combine(location.DirectoryName, "Codeworx.Rest.Tool.exe");
            var process = Process.Start(path, string.Join(" ", args.Select(p => $"\"{p}\"")));
            process.WaitForExit();
        }
    }
}