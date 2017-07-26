using IdSharp.Tagging.ID3v2;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using IdSharp.Tagging.ID3v2.Frames;
using IdSharp.Tagging.ID3v2.Extensions;
using System.Linq;

namespace ID3Utils
{
    interface IID3Converter
    {
        // bool Init(string path, string ext, SearchOption searchOption)
        bool Init((string path, string ext, SearchOption searchOption) configuration);

        void Execute();

        void PrintTag(object tag);

        void PrintTag(string file);

        void PrintTag();

        void WriteToFile(object tag, string path);
    }
}