using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace ID3Utils.Common
{
    public static class LangEncoding
    {
        public static readonly Encoding ANSI;
        public static readonly Encoding ASCIIEncoding;//ISO_8859_1
        public static readonly Encoding UTF8;
        public static readonly Encoding HebrewEncoding;

        // https://msdn.microsoft.com/en-us/library/system.text.codepagesencodingprovider(v=vs.110).aspx
        static LangEncoding()
        {
            ANSI = CodePagesEncodingProvider.Instance.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
            UTF8 = Encoding.UTF8;
            HebrewEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1255);
            // ASCIIEncoding = CodePagesEncodingProvider.Instance.GetEncoding("ISO-8859-1");
            // ASCIIEncoding = CodePagesEncodingProvider.Instance.GetEncoding(28591);
            // ASCIIEncoding = CodePagesEncodingProvider.Instance.GetEncoding(CultureInfo.CreateSpecificCulture("ISO-8859-1").TextInfo.ANSICodePage);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //HebrewEncoding = Encoding.GetEncoding("windows-1255");
            ASCIIEncoding = Encoding.GetEncoding("ISO-8859-1");
        }
    }
}