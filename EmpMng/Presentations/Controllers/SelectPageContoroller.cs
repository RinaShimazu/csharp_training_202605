using Microsoft.AspNetCore.Mvc;

public class SelectPageController : Controller
{
    [HttpGet]
    public IActionResult EmployeeSelect()
    {
        return View("EmployeeSelect");
    }

    [HttpGet]
    public IActionResult DepartmentSelect()
    {
        return View("DepartmentSelect");
    }
}
