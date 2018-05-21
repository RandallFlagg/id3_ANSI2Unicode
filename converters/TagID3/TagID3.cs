using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ID3Utils.Common;
using Id3;
//using Id3.Frames;

namespace ID3Utils
{
    //TODO: Are this frames being takeמ care or not? CHECK!
    // UpdateFrameList(tag.Composers);//TODO: Open this
    // UpdateFrameList(tag.Artists);//TODO: Open this
    // UpdateFrameList(tag.Comments);
    // UpdateFrameList(tag.PrivateData);
    public class TagID3 : ITag<TextFrameBase, Id3TextEncoding>
    {
        byte ITag.Encoding => (byte)_tag.EncodingSettings.EncodingType;

        public Id3TextEncoding Encoding
        {
            get
            {

                return _tag.EncodingSettings.EncodingType;
            }
            set
            {
                _tag.EncodingSettings.EncodingType = value;
            }
        }

        private Id3TextEncoding _encoding;
        private Id3Tag _tag;
        private string _file;
        private IList<TextFrameBase> _textFrames;
        private IOutput _output;

        public TagID3()
        {

        }

        public void Init(string file, IOutput output)
        {
            _output = output;
            _file = file;
            using (var mp3 = new Mp3(file, Mp3Permissions.Read))
            {
                _tag = mp3.GetTag(Id3TagFamily.Version2X);
            }
            _encoding = (_tag.Version == Id3Version.V23 || _tag.Version == Id3Version.V1X) ? Id3TextEncoding.Unicode : (Id3TextEncoding)03;//TODO: Should be changed to utf-8 when implemented
        }

        public void ConvertTo(int major, int minor)
        {
            switch (major)
            {
                case 1:
                    _tag.ConvertTo(Id3Version.V1X);
                    break;
                case 2:
                    if (minor == 3)
                    {
                        _tag.ConvertTo(Id3Version.V23);
                    }
                    else
                    {
                        throw new NotImplementedException("The library doesn't support yet ID3 2.4");
                    }
                    break;
            }
        }

        public IList<TextFrameBase> TextFrames
        {
            get
            {
                if (_textFrames == null)
                {
                    var _textFrames1 = new List<TextFrameBase>();
                    var _textFrames2 = new List<TextFrameBase>();

                    var properties = _tag.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetField | BindingFlags.GetField | BindingFlags.Instance);
                    //var textFrameType = typeof(TextFrame);
                    var textFrameType = typeof(TextFrameBase);
                    foreach (var property in properties)
                    {
                        if (property.PropertyType.IsSubclassOf(textFrameType))
                        {
                            var value = property.GetValue(_tag);
                            _textFrames1.Add(value as TextFrameBase);
                        }
                    }

                    //TODO: Check if _textFrames2 can be used insted of _textFrames1
                    _textFrames2 = _tag.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetField | BindingFlags.GetField | BindingFlags.Instance)
                    .Where(property => property.PropertyType.IsSubclassOf(textFrameType)).Select(prop => prop.GetValue(_tag) as TextFrameBase).ToList();

                    _textFrames = _textFrames2;
                }

                return _textFrames;
            }
        }

        public void Save()
        {
            using (var mp3 = new Mp3(_file, Mp3Permissions.Write))
            {
                mp3.WriteTag(_tag, WriteConflictAction.NoAction);
            }
        }

        public void Print()
        {
            foreach (var frame in TextFrames)
            {
                TextFrame value1 = null;
                ListTextFrame value2;
                if (frame is TextFrame)
                {
                    value1 = frame as TextFrame;
                }
                else if (frame is ListTextFrame)
                {
                    value2 = frame as ListTextFrame;
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (_output != null)
                {
                    _output.WriteLine($"{value1?.ToString()}: {value1?.Value}", Severity.Info);
                }
            }
        }

        public void UpdateAllFrames(Encoding srcEncoding)
        {
            foreach (var frame in TextFrames)
            {
                UpdateFrame(frame, srcEncoding);


                //fixme.RequiresFix()
                //Output.WriteLine($"After: Value: {frame?.Value}, Encoding: {frame.EncodingType}");
            }
        }

        public void UpdateFrame(TextFrameBase fixme, Encoding srcEncoding)
        {
            TextFrame value1 = fixme as TextFrame;

            //_output.WriteLine("Before: Value: {0}, Encoding: {1}", frame.Value, frame.EncodingType);

            if (value1 != null)
            {
                UpdateFrame(value1, srcEncoding);
            }
            else
            {
                ListTextFrame value2 = fixme as ListTextFrame;
                if (value2 != null)
                {
                    UpdateFrameList(value2, srcEncoding);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        //TODO: Remove 2nd paprameter and auto detect encoding
        private void UpdateFrame(TextFrame fixme, Encoding srcEncoding)
        {
            var oldValue = fixme.Value;
            var newValue = LangUtil.FixEncoding(oldValue, srcEncoding);
            fixme.Value = newValue;
            fixme.EncodingType = _encoding;
        }

        private void UpdateFrameList(ListTextFrame frame, Encoding srcEncoding)
        {
            for (int i = 0; i < frame.Value.Count; i++)//authors
            {
                frame.Value[i] = LangUtil.FixEncoding(frame.Value[i], LangEncoding.HebrewEncoding);
                frame.EncodingType = Id3TextEncoding.Unicode;
            }
        }

        public void PrintToFile(string path)
        {
            using (var outputFile = new StreamWriter(path, true))
            {
                var line = _tag.Title;

                outputFile.WriteLine("Title: {0}", line);
                outputFile.WriteLine("TextEncoding: {0}", _tag.EncodingSettings.EncodingType);
                outputFile.WriteLine($"TagVersion: {_tag.Version}");
            }
            _output.WriteLine("tag: {0}", _tag);
        }
    }
}