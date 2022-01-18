using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    //[Route("empl/[action]/{id?}")]
    //[Route("Staff/{action=Index}/{Id?}")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        public IActionResult Index()
        {
            var result = _EmployeesData.GetAll();
            return View(result);
        }

        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int Id)
        {
            ViewData["TestValue"] = 123;

            //var employee = __Employees.FirstOrDefault(e => e.Id == Id);
            var employee = _EmployeesData.GetById(Id);

            if (employee is null)
                return NotFound();

            ViewBag.SelectedEmployee = employee;

            return View(employee);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            //var employee = __Employees.FirstOrDefault(e => e.Id == id);
            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
            {
                _Logger.LogWarning("При редактировании сотрудника с id:{0} он не был найден", id);
                return NotFound();
            }

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (Model.LastName == "Асама" && Model.Name == "Бин" && Model.Patronymic == "Ладен")
                ModelState.AddModelError("", "Террористов на работу не берём!");

            if (!ModelState.IsValid)
                return View(Model);

            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.Name,
                Patronymic = Model.Patronymic,
                Age = Model.Age,
            };

            if (Model.Id == 0)
            {
                _EmployeesData.Add(employee);
                _Logger.LogInformation("Создан новый сотрудник {0}", employee);
            }
            else if (!_EmployeesData.Edit(employee))
            {
                _Logger.LogInformation("Информация о сотруднике {0} изменена", employee);
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_EmployeesData.Delete(id))
                return NotFound();

            _Logger.LogInformation("Сотрудник с id:{0} удалён", id);

            return RedirectToAction("Index");
        }
    }
}
