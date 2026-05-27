using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;

namespace EmpMng.Presentations.Controllers;

/// <summary>
/// 従業員登録コントローラ
/// </summary>
[Route("EmployeeRegister")]
public class EmployeeRegisterController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<EmployeeRegisterController> _logger;
    /// <summary>
    /// 従業員登録サービスインターフェイス
    /// </summary>
    private readonly IEmployeeRegisterService _employeeRegisterService;
    /// <summary>
    /// 従業員登録ViewModelをEmployeeに変換するアダプター
    /// </summary>
    private readonly EmployeeRegisterViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<EmployeeRegisterViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EmployeeRegisterController(
        ILogger<EmployeeRegisterController> logger,
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
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        EmployeeRegisterViewModel? viewModel = null;
        // [戻る]ボタンへの対応
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            viewModel = new EmployeeRegisterViewModel();
        }
        // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
        PopulateDepartments(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// 入力画面の[完了]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost("Confirm")]
    public IActionResult Confirm(EmployeeRegisterViewModel viewModel)
    {
        // バリデーションチェック
        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel);
            return View("Enter", viewModel);
        }

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

        // 確認画面を表示する
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[登録]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost("Regiter")]
    public IActionResult Register(EmployeeRegisterViewModel viewModel)
    {
        // EmployeeRegisterViewModelをシリアライズして、TempDataに保存する
        _empDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    /// <summary>
    /// アクションメソッド:Regiter()のリダイレクト先
    /// PRGパターン
    /// </summary>
    /// <returns></returns>
    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        EmployeeRegisterViewModel? viewModel = null;
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Enter");
        }
        // EmployeeRegisterFormをドメインモデル:Employeeに変換する
        var employee = _adapter.Restore(viewModel!);
        // 新しい従業員を登録する
        _employeeRegisterService.Register(employee);
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    /// <returns></returns> 
    [HttpPost("Back")]
    public IActionResult Back(EmployeeRegisterViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        _empDataStore.Save(this, viewModel);
        return RedirectToAction("Enter");
    }

    /// <summary>
    /// 部署一覧を取得してViewModelに設定する(SelectListItem形式)
    /// </summary>
    private void PopulateDepartments(EmployeeRegisterViewModel viewModel)
    {
        var departments = _employeeRegisterService.GetDepartments();
        viewModel.SetDepartments(departments);
        _logger.LogInformation("部署リストを設定");
    }
}
