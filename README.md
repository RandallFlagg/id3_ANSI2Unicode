# ID3 ANSI2Unicode Convert

## ID3 ANSI2Unicode Convert for .NET Core
<!-- ![id3-ansi2unicode Social Banner](https://s3.amazonaws.com/id3-ansi2unicode/wide-social-banner.png) -->

[![Throughput Graph](https://graphs.waffle.io/RandallFlagg/id3_ANSI2Unicode/throughput.svg)](https://waffle.io/RandallFlagg/id3_ANSI2Unicode/metrics/throughput)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)<!-- [![Known Vulnerabilities](https://snyk.io/test/github/RandallFlagg/id3_ANSI2Unicode/badge.svg)](https://snyk.io/test/github/RandallFlagg/id3_ANSI2Unicode) -->
[![Pull Requests Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat)](http://makeapullrequest.com)
![subject-music-data id3](https://img.shields.io/badge/Subject-Music%2Fid3-8A2BE2.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/gs5f2a4xavxvgtxc/branch/master?svg=true)](https://ci.appveyor.com/project/RandallFlagg/id3-ansi2unicode/branch/master)

<!--
[![Build Status](https://travis-ci.org/RandallFlagg/id3-ansi2unicode.svg?branch=staging)](https://travis-ci.org/RandallFlagg/id3-ansi2unicode)

[![NuGet Badge](https://buildstats.info/nuget/CommandLineArgumentsParser)](https://www.nuget.org/packages/CommandLineArgumentsParser) -->


ID3 ANSI2Unicode Convert for .NET Core is an effort to port an entire mp3 library from ANSI encoding to Unicode in order to have better support using modern players that don't support ANSI.

This utility uses the [IDSharp for.NET Core](https://github.com/RandallFlagg/IdSharpCore) library but it can also be used with the [id3](https://archive.codeplex.com/?p=id3) library, if wanted for any reason.

**NOTE** - The code in this repo has not been thoroughly tested.

This version of the utility should support .NET Core 1.1 and 2.0, as well as .NET Standard 1.3, 1.5 and 2.0 but was tested only against 2.x.

## Future Plans

* A javascript version; probably based on Node.js

## Getting Started or Wanting to Help

1.  Fork and/or Clone this repo to a desired location(**e.g.** C:\Projects\ID3)
2.  Fork or Clone the following repo to the same location as above(**e.g.** C:\Projects\ID3): https://github.com/RandallFlagg/IdSharpCore
3.  Try the example below to get started

## Example

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start Converting!");
            IID3Converter converter = new ID3SharpConverter();
            var recursive = false;
            var configuration = (
                path: @"C:\My Audio Files",
                ext: "*.mp3",
                searchOption: recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
            );

            if (converter.Init(configuration))
            {
                Console.WriteLine("Before:");
                converter.PrintTag();

                converter.Execute();

                Console.WriteLine("After:");
                converter.PrintTag();

                Console.WriteLine("Done Converting!");
            }
        }
    }
}
```
