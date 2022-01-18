namespace WebStore.Interfaces.TestAPI;

public interface IValuesService
{
    IEnumerable<string> GetValues();

    int Count();

    string? GetById(int Id);

    void Add(string Value);

    void Edit(int Id, string Value);

    bool Delete(int Id);
}