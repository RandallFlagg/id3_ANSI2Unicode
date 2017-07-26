using System.Text;
using System.Text.RegularExpressions;

namespace ID3Utils
{
    public static class LangUtil
    {
        public static string FixEncoding(string inputText, Encoding encoding)
        {
            var result = inputText;

            if (result != null)
            {
                var errorCounter = Regex.Matches(inputText, @"[א-ת]").Count;
                if (errorCounter == 0)
                {
                    errorCounter = Regex.Matches(inputText, @"[a-zA-Z]").Count;
                    if (errorCounter == 0)
                    {
                        //var bytesANSI = inputText.Select(c => (byte)c).ToArray();
                        var bytesANSI = LangEncoding.ANSI.GetBytes(inputText);

                        var utf8Chars = GetChars(bytesANSI, encoding);
                        // var asciiChars = GetChars(bytesANSI, ASCIIEncoding);
                        // var currentCultureChars = GetChars(bytesANSI, ANSI);

                        result = new string(utf8Chars);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the byte array into a char[] and then into a string
        /// </summary>
        /// <param name="bytesANSI">The ansi string as byte array</param>
        /// <param name="srcEncoding">The encoding of the source string</param>
        /// <returns>A char array containg the string encoded in UTF-8</returns>
        private static char[] GetChars(byte[] bytesANSI, Encoding srcEncoding)
        {
            var bytes = Encoding.Convert(srcEncoding, LangEncoding.UTF8, bytesANSI);
            var result = new char[LangEncoding.UTF8.GetCharCount(bytes, 0, bytes.Length)];
            LangEncoding.UTF8.GetChars(bytes, 0, bytes.Length, result, 0);
            return result;
        }
    }
}