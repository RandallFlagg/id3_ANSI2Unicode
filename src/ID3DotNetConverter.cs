using Id3;
using Id3.Frames;
using System;
using System.IO;
using System.Collections.Generic;
// using System.Text;
using System.Reflection;
// using System.Dynamic;
//using System.Linq;
//using Utils;

namespace ID3Utils
{
    class ID3DotNetConverter : IID3Converter
    {
        private IEnumerable<string> _files;
        private IOutputPrint _output;

        public ID3DotNetConverter(IOutputPrint output = null)
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
            foreach (string musicFile in _files)
            {
                using (var mp3 = new Mp3File(musicFile, Mp3Permissions.ReadWrite))
                {
                    var tag = mp3.GetTag(Id3TagFamily.Version2x);

                    UpdateFrameList(tag.Composers);
                    UpdateFrameList(tag.Artists);
                    // UpdateFrameList(tag.Comments);
                    // UpdateFrameList(tag.PrivateData);

                    if (tag != null)
                    {
                        var textFramesProperties = ID3DotNetConverter.GetTextFrames(tag);
                        foreach (var property in textFramesProperties)
                        {
                            UpdateFrame(tag, property);
                        }

                        //tag.ConvertTo(2, 4);

                        // 00 – ISO-8859-1 (ASCII).
                        // 01 – UCS-2 (UTF-16 encoded Unicode with BOM), in ID3v2.2 and ID3v2.3.
                        // 02 – UTF-16BE encoded Unicode without BOM, in ID3v2.4.
                        // 03 – UTF-8 encoded Unicode, in ID3v2.4.
                        tag.EncodingSettings.EncodingType = Id3.Id3TextEncoding.Unicode;
                        mp3.WriteTag(tag, WriteConflictAction.NoAction);
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                }
            }
        }

        private static List<PropertyInfo> GetTextFrames(Id3Tag tag)
        {
            var result = new List<PropertyInfo>();
            var properties = tag.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetField | BindingFlags.GetField | BindingFlags.Instance);
            var textFrameType = typeof(TextFrame);
            foreach (var property in properties)
            {
                if (property.PropertyType.IsSubclassOf(textFrameType))
                {
                    result.Add(property);
                }
            }

            return result;
        }

        private void UpdateFrameList(ListTextFrame frame)
        {
            for (int i = 0; i < frame.Value.Count; i++)//authors
            {
                frame.Value[i] = LangUtil.FixEncoding(frame.Value[i], LangEncoding.HebrewEncoding);
                frame.EncodingType = Id3TextEncoding.Unicode;
            }
        }

        private void UpdateFrame(Id3Tag tag, PropertyInfo property)
        {
            var oldValue = property.GetValue(tag) as TextFrame;
            var newValue = LangUtil.FixEncoding(oldValue, LangEncoding.HebrewEncoding);
            oldValue.Value = newValue;
            oldValue.EncodingType = Id3TextEncoding.Unicode;
        }

        public void PrintTag(object tag)
        {
            var textFramesProperties = ID3DotNetConverter.GetTextFrames(tag as Id3Tag);
            foreach (var property in textFramesProperties)
            {
                var value = property.GetValue(tag) as TextFrame;
                if (_output != null)
                {
                    _output.WriteLine("{0}: {1}", property.Name, value.Value);
                }
            }
        }

        public void PrintTag(string file)
        {
            using (var mp3 = new Mp3File(file, Mp3Permissions.Read))
            {
                PrintTag(mp3);
            }
        }

        public void PrintTag()
        {
            foreach (var file in _files)
            {
                PrintTag(file);
            }
        }

        public void PrintTag(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                PrintTag(file);
            }
        }

        public void PrintTag(Mp3File mp3)
        {
            var tag = mp3.GetTag(Id3TagFamily.Version2x);
            if (tag != null)
            {
                PrintTag(tag);
            }
        }

        public void WriteToFile(object tag, string path)
        {
            var id3Tag = tag as Id3Tag;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "WriteLines.txt"), true))
            {
                var line = id3Tag.Title.Value;

                //foreach (string line in lines)
                outputFile.WriteLine("Title: {0}", line);
                outputFile.WriteLine("EncodingSettings Before: {0}", id3Tag.EncodingSettings.EncodingType);
                outputFile.WriteLine("EncodingSettings After: {0}", id3Tag.EncodingSettings.EncodingType);
            }
            //Console.WriteLine("tag: {0}", tag);
        }
    }
}