using System;

namespace ID3Utils.Common
{
    class OutputFile : IOutput
    {
        public void WriteLine(string formattedText, params object[] values)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string formattedText, Severity severity)
        {
            throw new NotImplementedException();
        }
    }
}