using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.Controllers;
using EmpMng.Presentations.ViewModels;
using EmpMng.Applications.Adapters;


namespace EmpMng.Presentations;

/// <summary>
/// 従業員更新コントローラ
/// </summary>
[Route("EmployeeUpdate")]
public class EmployeeUpdateController : Controller
{
    private readonly ILogger<EmployeeUpdateController> _logger;
    private readonly IEmployeeRegisterService _employeeRegisterService;

    private readonly EmployeeRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<EmployeeRegisterViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EmployeeUpdateController(
        ILogger<EmployeeUpdateController> logger,
        IEmployeeRegisterService employeeRegisterService,
        EmployeeRegisterViewModelAdapter employeeRegisterViewModelAdapter,
        TempDataStore<EmployeeRegisterViewModel> empDataStore)
    {
        _logger = logger;
        _employeeRegisterService = employeeRegisterService;
        _adapter = employeeRegisterViewModelAdapter;
        _empDataStore = empDataStore;
    }

    /// <summary>
    /// 従業員更新(入力)画面表示 アクションメソッド
    /// </summary>
    /// <param name="id">更新対象の従業員ID</param>
    [HttpGet("Enter/{id?}")]
    public IActionResult Enter(int? id)
    {
        EmployeeRegisterViewModel? viewModel = null;

        // [戻る]ボタンへの対応
        viewModel = _empDataStore.Load(this);

        if (viewModel == null && id.HasValue)
        {
            var employee = _employeeRegisterService.GetEmployeeId(id.Value);
            if (employee != null)
            {
                viewModel = _adapter.Convert(employee);
            }
        }

        if (viewModel == null)
        {
            viewModel = new EmployeeRegisterViewModel();
        }

        PopulateDepartments(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// 入力画面の[完了]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Confirm")]
    public IActionResult Confirm(EmployeeRegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel);
            return View("Enter", viewModel);
        }

        if (viewModel.DeptId.HasValue && viewModel.DeptId.Value != 0)
        {
            var department = _employeeRegisterService.GetById(viewModel.DeptId.Value);
            _logger.LogInformation($"部門番号:{viewModel.DeptId.Value}の部門を取得する");

            if (department != null)
            {
                viewModel.DeptName = string.IsNullOrEmpty(department.Name) ? "(名称未設定)" : department.Name;
            }
        }
        else
        {
            viewModel.DeptName = "未選択";
            viewModel.DeptId = null;
        }

        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[更新]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Register")]
    public IActionResult Register(EmployeeRegisterViewModel viewModel)
    {
        _empDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    /// <summary>
    /// アクションメソッド:Register()のリダイレクト先
    /// </summary>
    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        EmployeeRegisterViewModel? viewModel = null;
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Enter");
        }

        var employee = _adapter.Restore(viewModel!);

        _employeeRegisterService.UpdateEmployeeId(employee);

        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Back")]
    public IActionResult Back(EmployeeRegisterViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        _empDataStore.Save(this, viewModel);
        return RedirectToAction("Enter");
    }

    /// <summary>
    /// 部署一覧を取得してViewModelに設定する
    /// </summary>
    private void PopulateDepartments(EmployeeRegisterViewModel viewModel)
    {
        var departments = _employeeRegisterService.GetDepartments();
        viewModel.SetDepartments(departments);
        _logger.LogInformation("部門リストを設定");
    }
}
