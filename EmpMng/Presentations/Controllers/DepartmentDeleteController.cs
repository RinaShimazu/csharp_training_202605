using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;
using EmpMng.Exceptions;

namespace EmpMng.Presentations.Controllers;

/// <summary>
/// 従業員削除コントローラ
/// </summary>
[Route("DepartmentDelete")]
public class DepartmentDeleteController : Controller
{
    private readonly ILogger<DepartmentDeleteController> _logger;
    private readonly IDepartmentRegisterService _departmentRegisterService;
    private readonly DepartmentRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<DepartmentRegisterViewModel> _deptDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DepartmentDeleteController(
        ILogger<DepartmentDeleteController> logger,
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
    /// 従業員削除(入力)画面の初期表示
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
    [HttpPost("LoadDepartment")]
    public IActionResult LoadDepartment(DepartmentRegisterViewModel inputModel)
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
    /// 確認画面へ進むアクションメソッド
    /// </summary>
    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentRegisterViewModel viewModel)
    {

        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[削除]ボタンクリックアクションメソッド
    /// </summary>
    [HttpPost("Delete")]
    public IActionResult Delete(DepartmentRegisterViewModel viewModel)
    {
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    /// <summary>
    /// アクションメソッド:Delete()のリダイレクト先
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
        _departmentRegisterService.DeleteDepartmentId(department);

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
        _logger.LogInformation("部門リストを設定");
    }
}
