using TestConsole.Data;
using TestConsole.Services.Interfaces;

namespace TestConsole.Services
{
    public class ConsolePrintProcessor : IDataProcessor
    {
        public void Process(DataValue Value)
        {
            Console.WriteLine("[{0}]({1}):{2}", Value.Id, Value.Time, Value.Value);
        }
    }
}
