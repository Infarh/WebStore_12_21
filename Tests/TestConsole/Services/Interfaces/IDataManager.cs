using TestConsole.Data;

namespace TestConsole.Services.Interfaces
{
    public interface IDataManager
    {
        void ProcessData(IEnumerable<DataValue> Values);
    }


    public interface IDataProcessor
    {
        void Process(DataValue Value);
    }
}
