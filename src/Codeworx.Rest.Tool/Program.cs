using System;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;

namespace Codeworx.Rest.Tool
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var versionString = Assembly.GetEntryAssembly()
                                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                    .InformationalVersion
                                    .ToString();
            Console.WriteLine($"restcontract v{versionString}");

            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            if (result.Tag == ParserResultType.Parsed)
            {
                var parsed = (Parsed<Options>)result;
                var worker = new ProxyCreator(parsed.Value);
                await worker.ProcessAsync();
            }
            else
            {
                var notParsed = (NotParsed<Options>)result;
            }
        }
    }
}