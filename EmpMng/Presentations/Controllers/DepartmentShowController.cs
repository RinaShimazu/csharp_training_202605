using Microsoft.AspNetCore.Mvc;
using EmpMng.Applications.Repositories;
using EmpMng.Applications.Domains;

namespace EmpMng.Presentations.Controllers
{
    public class DepartmentShowController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DepartmentShowController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // メニュー画面の asp-action="Show" に対応
        public IActionResult Show()
        {
            // リポジトリからドメインモデルのリストを取得
            var employees = _employeeRepository.FindAll();

            // Views/AllEmployee/Show.cshtml にデータを渡す
            return View(employees);
        }
    }
}
