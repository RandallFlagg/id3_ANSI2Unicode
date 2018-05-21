using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ID3Utils.Common;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

namespace ID3Utils
{
    public class TagSharp : ITag<ITextFrame, EncodingType>
    {
        byte ITag.Encoding => (byte)_tag.Languages.TextEncoding;

        public EncodingType Encoding
        {
            get
            {
                return _tag.Languages.TextEncoding;
            }
            set
            {
                _tag.Languages.TextEncoding = value;
            }
        }

        private EncodingType _encoding;
        private ID3v2Tag _tag;
        private string _file;
        private IList<ITextFrame> _textFrames;
        private IOutput _output;

        // public TagSharp(ID3v2Tag tag)
        // {
        //     _tag = tag;
        // }

        public TagSharp()
        {

        }

        public void Init(string file, IOutput output)
        {
            _output = output;
            _file = file;
            _tag = new ID3v2Tag(_file);
            _encoding = _tag.Header.TagVersion == ID3v2TagVersion.ID3v24 ? EncodingType.UTF8 : EncodingType.Unicode;
        }

        public void ConvertTo(int major, int minor)
        {
            throw new NotImplementedException("ConvertTo");
        }

        public IList<ITextFrame> TextFrames
        {
            get
            {
                if (_textFrames == null)
                    //_textFrames = _tag.GetFrames<ITextFrame>();
                    _textFrames = _tag.GetFrames().OfType<ITextFrame>().ToList();

                return _textFrames;
            }
        }

        public void Save()
        {
            _tag.Save(_file);
        }

        public void Print()
        {
            var textFramesProperties = TextFrames;
            foreach (var property in textFramesProperties)
            {
                var value = property.Value;
                if (_output != null)
                {
                    _output.WriteLine($"{property}: {value}", Severity.Info);
                }
            }
        }

        public void UpdateAllFrames(Encoding srcEncoding)
        {
            foreach (var frame in TextFrames)
            {
                _output.WriteLine("Before: Value: {0}, Encoding: {1}", frame.Value, frame.TextEncoding);
                UpdateFrame(frame, srcEncoding);
                //fixme.RequiresFix()
                _output.WriteLine("After: Value: {0}, Encoding: {1}", frame.Value, frame.TextEncoding);
            }
        }

        //TODO: Remove 2nd paprameter and auto detect encoding
        public void UpdateFrame(ITextFrame fixme, Encoding srcEncoding)
        {
            var oldValue = fixme.Value;
            var newValue = LangUtil.FixEncoding(oldValue, srcEncoding);
            fixme.Value = newValue;
            fixme.TextEncoding = _encoding;
        }

        public void PrintToFile(string path)
        {
            using (var outputFile = new StreamWriter(path, true))
            {
                var line = _tag.Title;

                outputFile.WriteLine("Title: {0}", line);
                outputFile.WriteLine("TextEncoding: {0}", _tag.Languages.TextEncoding);
                outputFile.WriteLine("TagVersion: {0}", _tag.Header.TagVersion);
            }
            _output.WriteLine("tag: {0}", _tag);
        }
    }
}