using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;
using EmpMng.Exceptions;

namespace EmpMng.Presentations.Controllers;

/// <summary>
/// 従業員削除コントローラ
/// </summary>
[Route("EmployeeDelete")]
public class EmployeeDeleteController : Controller
{
    private readonly ILogger<EmployeeDeleteController> _logger;
    private readonly IEmployeeRegisterService _employeeRegisterService;
    private readonly EmployeeRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<EmployeeRegisterViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EmployeeDeleteController(
        ILogger<EmployeeDeleteController> logger,
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
    /// 従業員削除(入力)画面の初期表示
    /// </summary>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        var viewModel = _empDataStore.Load(this) ?? new EmployeeRegisterViewModel();

        PopulateDepartments(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// 入力された社員番号からデータを検索
    /// </summary>
    [HttpPost("LoadEmployee")]
    public IActionResult LoadEmployee(EmployeeRegisterViewModel inputModel)
    {
        if (inputModel.EmpId.HasValue)
        {
            try
            {
                var employee = _employeeRegisterService.GetEmployeeId(inputModel.EmpId.Value);

                if (employee != null)
                {
                    var viewModel = _adapter.Convert(employee);

                    if (viewModel.DeptId.HasValue && viewModel.DeptId.Value != 0)
                    {
                        var department = _employeeRegisterService.GetById(viewModel.DeptId.Value);
                        if (department != null)
                        {
                            viewModel.DeptName = string.IsNullOrEmpty(department.Name) ? "(名称未設定)" : department.Name;
                        }
                    }
                    else
                    {
                        viewModel.DeptName = "未選択";
                    }

                    PopulateDepartments(viewModel);
                    return View("Enter", viewModel);
                }
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("EmpId", ex.Message);
            }
        }

        PopulateDepartments(inputModel);
        return View("Enter", inputModel);
    }

    /// <summary>
    /// 確認画面へ進むアクションメソッド
    /// </summary>
    [HttpPost("Confirm")]
    public IActionResult Confirm(EmployeeRegisterViewModel viewModel)
    {

        if (viewModel.DeptId.HasValue && viewModel.DeptId.Value != 0)
        {
            var department = _employeeRegisterService.GetById(viewModel.DeptId.Value);
            _logger.LogInformation($"部門Id:{viewModel.DeptId.Value}の部門を取得する");

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
    /// 確認画面の[削除]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Delete")]
    public IActionResult Delete(EmployeeRegisterViewModel viewModel)
    {
        _empDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    /// <summary>
    /// アクションメソッド:Delete()のリダイレクト先
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
        _employeeRegisterService.DeleteEmployeeId(employee);

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
