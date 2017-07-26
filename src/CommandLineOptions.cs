using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

class CommandLineOptions
{
    [Value(0)]
    public IEnumerable<string> StringSeq { get; set; }

    [Option('p', "path", Required = false, Default = ".", HelpText = "The location of the files you want to work on.")]
    public string Path { get; set; }
    // public IEnumerable<string> InputFiles { get; set; }

    // Omitting long name, defaults to name of property, ie "--verbose"
    [Option('r', "recursive", Required = false, Default = false, HelpText = "Scan only the current directory or the whole tree.")]
    public bool Recursive { get; set; }


    [Option('e', "ext", Required = false, Default = "*.mp3", HelpText = "The pattern of the files to scan.")]
    public string Extension { get; set; }

    //[Value(0, MetaName = "offset", HelpText = "File offset." )]
    //public long? Offset { get; set; }

    // [Usage(ApplicationAlias = "yourapp")]
    // public static IEnumerable<Example> Examples
    // {
    //     get
    //     {
    //         yield return new Example("Normal scenario", new CommandLineOptions { InputFile = "file.bin", OutputFile = "out.bin" });
    //         yield return new Example("Logging warnings", UnParserSettings.WithGroupSwitchesOnly(), new CommandLineOptions { InputFile = "file.bin", LogWarning = true });
    //         yield return new Example("Logging errors", new[] { UnParserSettings.WithGroupSwitchesOnly(), UnParserSettings.WithUseEqualTokenOnly() }, new CommandLineOptions { InputFile = "file.bin", LogError = true });
    //     }
    // }
}