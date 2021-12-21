using TestConsole.Data;
using TestConsole.Services.Interfaces;

namespace TestConsole.Services;

public class WriteToFileProcessor : IDataProcessor
{
    public static string DataFileName { get; set; } = "data.txt";

    public void Process(DataValue Value)
    {
        using var writer = File.AppendText(DataFileName);
        writer.WriteLine("[{0}]({1}):{2}", Value.Id, Value.Time, Value.Value);
    }
}
