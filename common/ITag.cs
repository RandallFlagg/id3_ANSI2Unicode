// using Id3;
// using Id3.Frames;
// using System;
// using System.IO;
// using System.Collections.Generic;
// // using System.Text;
// using System.Reflection;
// using ID3Utils.Common;
// using System.Dynamic;
//using System.Linq;
//using Utils;

using System.Collections.Generic;
using System.Text;

namespace ID3Utils.Common
{
    public interface ITag
    {
        byte Encoding { get; }
        void Init(string file, IOutput output);

        void UpdateAllFrames(Encoding srcEncoding);

        void ConvertTo(int major, int minor);

        void Print();

        void PrintToFile(string path);

        void Save();
    }
    public interface ITag<TF, E> : ITag
    {
        // 00 – ISO-8859-1 (ASCII).
        // 01 – UCS-2 (UTF-16 encoded Unicode with BOM), in ID3v2.2 and ID3v2.3.
        // 02 – UTF-16BE encoded Unicode without BOM, in ID3v2.4.
        // 03 – UTF-8 encoded Unicode, in ID3v2.4.
        new E Encoding { get; }

        void UpdateFrame(TF fixme, Encoding srcEencoding);

        IList<TF> TextFrames { get; }
    }
}