namespace ID3Utils.Common
{
    public interface IOutput
    {
        void WriteLine(string formattedText, Severity severity);
        void WriteLine(string formattedText, params object[] values);
    }
}