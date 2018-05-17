using System;

namespace ID3Utils.Common
{
    public class OutputConsole : IOutput
    {
        public void WriteLine(string formattedText, params object[] values)
        {
            Console.WriteLine(formattedText, values);
        }

        public void WriteLine(string text, Severity severity = Severity.Info)
        {
            var currentColor = Console.ForegroundColor;
            switch (severity)
            {
                case Severity.Info:
                    //Console.ForegroundColor = 
                    break;
                case Severity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Severity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Severity.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    throw new NotImplementedException($"severity: {severity}");
            }

            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}