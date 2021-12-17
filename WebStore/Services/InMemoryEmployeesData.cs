using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class InMemoryEmployeesData : IEmployeesData
{
    private readonly ICollection<Employee> _Employees;
    private int _MaxFreeId;

    public InMemoryEmployeesData()
    {
        _Employees = TestData.Employees;
        _MaxFreeId = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
    }

    public IEnumerable<Employee> GetAll() => _Employees;

    public Employee? GetById(int id) => _Employees.FirstOrDefault(employee => employee.Id == id);

    public int Add(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        if (_Employees.Contains(employee)) // Когда это будет реализовано во БД - это писать НЕ НАДО!!!
            return employee.Id;

        employee.Id = _MaxFreeId++;
        _Employees.Add(employee);

        return employee.Id;
    }

    public bool Edit(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        if (_Employees.Contains(employee))// Когда это будет реализовано во БД - это писать НЕ НАДО!!!
            return true;

        var db_employee = GetById(employee.Id);
        if (db_employee is null)
            return false;

        db_employee.FirstName = employee.FirstName;
        db_employee.LastName = employee.LastName;
        db_employee.Patronymic = employee.Patronymic;
        db_employee.Age = employee.Age;

        // Когда будет БД, не забыть вызвать SaveChanges()

        return true;
    }

    public bool Delete(int id)
    {
        var employee = GetById(id);
        if (employee is null)
            return false;

        _Employees.Remove(employee);

        return true;
    }
}