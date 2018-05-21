using System;

namespace ID3Utils.Common
{
    public class OutputNull : IOutput
    {
        public void WriteLine(string formattedText, params object[] values)
        {
            
        }

        public void WriteLine(string text, Severity severity = Severity.Info)
        {
            
        }
    }
}