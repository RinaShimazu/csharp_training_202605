using Microsoft.AspNetCore.Mvc;
using EmpMng.Applications.Repositories;
using EmpMng.Applications.Domains;

namespace EmpMng.Presentations.Controllers
{
    public class AllEmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AllEmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IActionResult Show()
        {
            try
            {
                List<Employee> employees = _employeeRepository.FindAll();

                return View(employees);
            }
            catch (Exception)
            {
                return RedirectToAction("EmployeeSelect", "SelectPage");
            }
        }
    }
}
