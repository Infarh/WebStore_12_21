using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddresses.Employees)] // http://localhost:5001/api/employees
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _EmployeesData;

    public EmployeesApiController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

    /// <summary>Получение всех сотрудников</summary>
    [HttpGet]
    public IActionResult Get()
    {
        var employees = _EmployeesData.GetAll();
        return Ok(employees);
    }

    /// <summary>Получение сотрудника по его идентификатору</summary>
    /// <param name="Id">Идентификатор сотрудника</param>
    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int Id)
    {
        var employee = _EmployeesData.GetById(Id);
        if (employee is null)
            return NotFound();

        return Ok(employee);
    }

    /// <summary>Добавление нового сотрудника</summary>
    /// <param name="employee">Новый сотрудник</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Employee))]
    public IActionResult Add(Employee employee)
    {
        var id = _EmployeesData.Add(employee);
        return CreatedAtAction(nameof(GetById), new { Id = id }, employee);
    }

    /// <summary>Обновление информации о сотруднике</summary>
    /// <param name="employee">Структура с информацией о сотруднике</param>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public IActionResult Update(Employee employee)
    {
        var success = _EmployeesData.Edit(employee);
        return Ok(success);
    }

    /// <summary>Удаление сотрудника по его идентификатору</summary>
    /// <param name="Id">Идентификатор удаляемого сотрудника</param>
    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public IActionResult Delete(int Id)
    {
        var result = _EmployeesData.Delete(Id);
        return result
            ? Ok(true)
            : NotFound(false);
    }
}