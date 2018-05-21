using System;

namespace ID3Utils.Common
{
    public static class Output
    {
        public static IOutput Factory(OutputOptions output)
        {
            IOutput result;
            switch (output)
            {
                case OutputOptions.Console:
                    result = new OutputConsole();
                    break;
                case OutputOptions.File:
                    result = new OutputFile();
                    break;
                case OutputOptions.Null:
                    result = new OutputNull();
                    break;
                default:
                    throw new NotImplementedException($"output: {output}");
            }

            return result;
        }
    }
}