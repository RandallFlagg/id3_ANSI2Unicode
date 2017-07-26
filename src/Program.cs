using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ID3Utils
{
    class Program
    {
        public static void Main(string[] args)
        {
            int exitCode = -1;
            Console.WriteLine("Start Argumnts Proccessing!");
            var parser =
           //  new Parser(with => {
           //     with.EnableDashDash = false;
           // });
           CommandLine.Parser.Default;
            var result = parser.ParseArguments<CommandLineOptions>(args);
            result
                .WithParsed<CommandLineOptions>(options =>
                {
                    var values = ((IList<object>)options.StringSeq).Count;
                    if (values > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("All values are ignored. Please use only the options (-, --).");
                    }
                    Console.WriteLine("Start Converting!");
                    IID3Converter converter = new ID3SharpConverter();
                    //IID3Converter converterBAD = new ID3DotNetConverter();

                    var configuration = (path: options.Path.Trim(), ext: options.Extension.Trim(), searchOption: options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    if (converter.Init(configuration))
                    {
                        Console.WriteLine("Before:");
                        //converter.PrintTag();
                        converter.Execute();
                        Console.WriteLine("After:");
                        //converter.PrintTag();
                        Console.WriteLine("Done Converting!");
                        exitCode = 0;
                    }
                })
                .WithNotParsed<CommandLineOptions>((errs) =>
                {
                    foreach (Error e in errs)
                    {
                        //Console.WriteLine("Repeated Option Error: {0}", e.NameInfo);
                        exitCode = (int)e.Tag;
                    }
                });

            Console.WriteLine("Done Argumnts Proccessing!");
            Environment.Exit(exitCode);
        }
    }
}