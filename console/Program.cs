using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ID3Utils;
using ID3Utils.Common;

namespace Program
{
    class Program
    {
        public static void Main(string[] args)
        {
            var out1 = Output.Factory(OutputOptions.Console);//TODO: Check why the name out gives an error during compile
            out1.WriteLine("Start Argumnts Proccessing!");
            int exitCode = -1;
            var parser =
           //  new Parser(with => {
           //     with.EnableDashDash = false;
           // });
           CommandLine.Parser.Default;
            var result = parser.ParseArguments<CommandLineOptions>(args);
            result
                .WithParsed<CommandLineOptions>(options =>
                {
                    if (options.StringSeq is IList<object>)
                    {
                        var values = (options.StringSeq as IList<object>).Count;
                        if (values > 0)
                        {
                            out1.WriteLine("All values are ignored. Please use only the options (-, --).", Severity.Warning);
                        }
                    }

                    out1.WriteLine("Start Converting!");
                    IID3Converter converter = new ID3Converter();
                    //IID3Converter converterBAD = new ID3DotNetConverter();

                    var configuration = (path: options.Path.Trim(), ext: options.Extension.Trim(), searchOption: options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    if (converter.Init(configuration))
                    {
                        out1.WriteLine("Before:");
                        //Path.Combine(path, "WriteLines.txt");
                        //converter.PrintTag();
                        converter.Execute<TagSharp>();
                        // converter.Execute<TagDotNet>();
                        // converter.Execute<TagID3>();
                        out1.WriteLine("After:");
                        //converter.PrintTag();
                        out1.WriteLine("Done Converting!");
                        exitCode = 0;
                    }
                })
                .WithNotParsed<CommandLineOptions>((errs) =>
                {
                    foreach (Error e in errs)
                    {
                        //out1.WriteLine("Repeated Option Error: {0}", e.NameInfo);
                        exitCode = (int)e.Tag;
                    }
                });

            out1.WriteLine("Done Argumnts Proccessing!");
            Environment.Exit(exitCode);
        }
    }
}