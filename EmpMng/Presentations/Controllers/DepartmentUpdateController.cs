using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;
using EmpMng.Exceptions;

namespace EmpMng.Presentations.Controllers;


/// <summary>
/// 従業員更新コントローラ
/// </summary>
[Route("DepartmentUpdate")]
public class DepartmentUpdateController : Controller
{

    private readonly ILogger<DepartmentUpdateController> _logger;
    private readonly IDepartmentRegisterService _departmentRegisterService;
    private readonly DepartmentRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<DepartmentRegisterViewModel> _deptDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DepartmentUpdateController(
        ILogger<DepartmentUpdateController> logger,
        IDepartmentRegisterService departmentRegisterService,
        DepartmentRegisterViewModelAdapter departmentRegisterViewModelAdapter,
        TempDataStore<DepartmentRegisterViewModel> deptDataStore)
    {
        _logger = logger;
        _departmentRegisterService = departmentRegisterService;
        _adapter = departmentRegisterViewModelAdapter;
        _deptDataStore = deptDataStore;
    }

    /// <summary>
    /// 従業員更新(入力)画面の初期表示
    /// </summary>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        var viewModel = _deptDataStore.Load(this) ?? new DepartmentRegisterViewModel();

        PopulateDepartments(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// 入力された社員番号からデータを検索
    /// </summary>
    [HttpPost("LoadEmployee")]
    public IActionResult LoadEmployee(DepartmentRegisterViewModel inputModel)
    {
        if (inputModel.DeptId != 0)
        {
            try
            {
                var department = _departmentRegisterService.GetDepartmentId(inputModel.DeptId);

                if (department != null)
                {
                    var viewModel = _adapter.Convert(department);

                    PopulateDepartments(viewModel);
                    return View("Enter", viewModel);
                }
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("DeptId", ex.Message);
            }
        }

        PopulateDepartments(inputModel);
        return View("Enter", inputModel);
    }

    /// <summary>
    /// 入力画面の[完了]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentRegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel);
            return View("Enter", viewModel);
        }

        if (viewModel.DeptId != 0)
        {
            var department = _departmentRegisterService.GetDepartmentId(viewModel.DeptId);
            _logger.LogInformation($"部署Id:{viewModel.DeptId}の部署を取得する");

            if (department != null)
            {
                viewModel.DeptName = string.IsNullOrEmpty(department.Name) ? "(名称未設定)" : department.Name;
            }
        }
        else
        {
            viewModel.DeptName = "未選択";
            viewModel.DeptId = 0;
        }

        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[更新]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Register")]
    public IActionResult Register(DepartmentRegisterViewModel viewModel)
    {
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    /// <summary>
    /// アクションメソッド:Register()のリダイレクト先
    /// </summary>
    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        DepartmentRegisterViewModel? viewModel = null;
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Enter");
        }

        var department = _adapter.Restore(viewModel!);

        _departmentRegisterService.UpdateDepartmentId(department);

        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Back")]
    public IActionResult Back(DepartmentRegisterViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Enter");
    }

    /// <summary>
    /// 部署一覧を取得してViewModelに設定する
    /// </summary>
    private void PopulateDepartments(DepartmentRegisterViewModel viewModel)
    {
        var departments = _departmentRegisterService.GetDepartments();
        viewModel.SetDepartments(departments);
        _logger.LogInformation("部署リストを設定");
    }
}
