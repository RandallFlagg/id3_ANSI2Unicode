using System.IO;

namespace ID3Utils.Common
{
    public interface IID3Converter
    {
        // bool Init(string path, string ext, SearchOption searchOption)
        bool Init((string path, string ext, SearchOption searchOption) configuration);

        void Execute<T>() where T : ITag, new();
    }
}