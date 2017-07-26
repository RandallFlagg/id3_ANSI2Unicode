using System;

class OutputPrintConsole : IOutputPrint
{
    public void WriteLine(string formattedText, params object[] values)
    {
        Console.WriteLine(formattedText, values);
    }
}