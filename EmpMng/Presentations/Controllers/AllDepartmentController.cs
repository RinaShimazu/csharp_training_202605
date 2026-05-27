using Microsoft.AspNetCore.Mvc;
using EmpMng.Applications.Repositories;
using EmpMng.Applications.Domains;

namespace EmpMng.Presentations.Controllers
{
    public class AllDepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public AllDepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public IActionResult Show()
        {
            try
            {
                List<Department> departments = _departmentRepository.FindAll();
                return View(departments);
            }
            catch (Exception)
            {
                return RedirectToAction("DepartmentSelect", "SelectPage");
            }
        }
    }
}
