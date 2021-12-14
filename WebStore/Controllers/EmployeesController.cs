using Microsoft.AspNetCore.Mvc;

using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 23 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 27 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
        };

        public IActionResult Index()
        {
            var result = __Employees;
            return View(result);
        }

        public IActionResult Details(int Id)
        {
            ViewData["TestValue"] = 123;

            var employee = __Employees.FirstOrDefault(e => e.Id == Id);

            if (employee is null)
                return NotFound();

            ViewBag.SelectedEmployee = employee;

            return View(employee);
        }
    }
}
