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
    class ID3SharpConverter : IID3Converter
    {
        private IEnumerable<string> _files;
        private IOutputPrint _output;

        public ID3SharpConverter(IOutputPrint output = null)
        {
            _output = output == null ? new OutputPrintConsole() : output;
        }

        public bool Init((string path, string ext, SearchOption searchOption) configuration)
        {
            bool result = false;
            if (Directory.Exists(configuration.path))
            {
                _files = Directory.GetFiles(configuration.path, configuration.ext, configuration.searchOption);
                result = true;
            }

            return result;
        }

        public void Execute()
        {
            foreach (var file in _files)
            {
                Console.WriteLine(file);
                var tag = new ID3v2Tag(file);
                if (tag != null)
                {
                    // 00 – ISO-8859-1 (ASCII).
                    // 01 – UCS-2 (UTF-16 encoded Unicode with BOM), in ID3v2.2 and ID3v2.3.
                    // 02 – UTF-16BE encoded Unicode without BOM, in ID3v2.4.
                    // 03 – UTF-8 encoded Unicode, in ID3v2.4.
                    var encoding = tag.Header.TagVersion == ID3v2TagVersion.ID3v24 ? EncodingType.UTF8 : EncodingType.Unicode;
                    var textFramesProperties = ID3SharpConverter.GetTextFrames(tag);

                    foreach (var frame in textFramesProperties)
                    {
                        {
                            Console.WriteLine("Before: Value: {0}, Encoding: {1}", frame.Value, frame.TextEncoding);
                            UpdateFrame(frame, encoding);
                            //fixme.RequiresFix()
                            Console.WriteLine("After: Value: {0}, Encoding: {1}", frame.Value, frame.TextEncoding);
                        }
                    }

                    //TODO: Are this frames being takeמ care or not? CHECK!
                    // UpdateFrameList(tag.Composers);//TODO: Open this
                    // UpdateFrameList(tag.Artists);//TODO: Open this
                    // UpdateFrameList(tag.Comments);
                    // UpdateFrameList(tag.PrivateData);

                    //tag.ConvertTo(2, 4);
                    tag.Languages.TextEncoding = encoding;
                    tag.Save(file);
                }
                else
                {
                    Console.WriteLine("TAG IS NULL");
                }
            }
        }

        private void UpdateFrame(ITextFrame fixme, EncodingType encoding)
        {
            var oldValue = fixme.Value;
            var newValue = LangUtil.FixEncoding(oldValue, LangEncoding.HebrewEncoding);
            fixme.Value = newValue;
            fixme.TextEncoding = encoding;
        }

        private static IEnumerable<ITextFrame> GetTextFrames(ID3v2Tag tag)
        {
            //var b = tag.GetFrames<ITextFrame>();
            return tag.GetFrames().OfType<ITextFrame>();
        }

        public void PrintTag(object tag)
        {
            var textFramesProperties = ID3SharpConverter.GetTextFrames(tag as ID3v2Tag);
            foreach (var property in textFramesProperties)
            {
                var value = property.Value;
                if (_output != null)
                {
                    _output.WriteLine("{0}: {1}", property, value);
                }
            }
        }

        public void PrintTag(string file)
        {
            PrintTag(new ID3v2Tag(file));
        }

        public void PrintTag()
        {
            foreach (var file in _files)
            {
                PrintTag(file);
            }
        }

        public void WriteToFile(object tag, string path)
        {
            var id3Tag = tag as ID3v2Tag;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "WriteLines.txt"), true))
            {
                var line = id3Tag.Title;

                outputFile.WriteLine("Title: {0}", line);
                outputFile.WriteLine("TextEncoding: {0}", id3Tag.Languages.TextEncoding);
                outputFile.WriteLine("TagVersion: {0}", id3Tag.Header.TagVersion);
            }
            Console.WriteLine("tag: {0}", id3Tag);
        }
    }
}