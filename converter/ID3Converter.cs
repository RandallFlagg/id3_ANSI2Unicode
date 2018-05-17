using ID3Utils.Common;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ID3Utils
{
    public class ID3Converter : IID3Converter
    {
        private IEnumerable<string> _files;
        private IOutput _output;

        public ID3Converter(IOutput output = null)
        {
            _output = output;
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

        public void Execute<T>() where T : ITag, new()
        {
            foreach (var file in _files)
            {
                _output.WriteLine(file);
                var tag = new T();
                tag.Init(file, _output);

                if (tag != null)//TODO: Not a good check. Change to a boolean check. something like: isValid?
                {
                    _output.WriteLine($"Encoding Before: {tag.Encoding}");
                    tag.Print();
					//TODO: Move the commented lies to a test file
                    // var frame = tag.TextFrames[0];
                    // tag.UpdateFrame(frame, LangEncoding.HebrewEncoding);
                    tag.UpdateAllFrames(LangEncoding.HebrewEncoding);

                    //tag.ConvertTo(2, 4);
                    _output.WriteLine($"Encoding After: {tag.Encoding}");
                    tag.Print();
                    tag.Save();
                }
                else
                {
                    _output.WriteLine("TAG IS NULL");
                }
            }
        }
    }
}