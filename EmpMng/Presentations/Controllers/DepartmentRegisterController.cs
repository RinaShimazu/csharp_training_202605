using Microsoft.AspNetCore.Mvc;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;

namespace EmpMng.Presentations.Controllers;
/// <summary>
/// 従業員登録コントローラ
/// </summary>
[Route("DepartmentRegister")]
public class DepartmentRegisterController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<DepartmentRegisterController> _logger;
    /// <summary>
    /// 従業員登録サービスインターフェイス
    /// </summary>
    private readonly IDepartmentRegisterService _departmentRegisterService;
    /// <summary>
    /// 従業員登録ViewModelをEmployeeに変換するアダプター
    /// </summary>
    private readonly DepartmentRegisterViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<DepartmentRegisterViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="departmentRegisterService">従業員登録サービスインターフェイス</param>
    /// <param name="departmentRegisterViewModelAdapter">従業員登録ViewModelをEmployeeに変換するアダプター</param>
    /// <param name="deptDataStore">TempDataを通じて一時的にViewModelを保存・復元するためのクラス</param>
    public DepartmentRegisterController RegisterController(
        ILogger<DepartmentRegisterController> logger,
        IDepartmentRegisterService employeeRegisterService,
        DepartmentRegisterViewModelAdapter employeeRegisterViewModelAdapter,
        TempDataStore<DepartmentRegisterViewModel> deptDataStore)
    {
        _logger = logger;
        _departmentRegisterService = departmentRegisterService;
        _adapter = departmentRegisterViewModelAdapter;
        _deptDataStore = deptDataStore;
    }

    /// <summary>
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        DepartmentRegisterViewModel? viewModel = null;
        // [戻る]ボタンへの対応
        // TempDataからEmployeeRegisterViewModelを取得する
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            // 従業員登録ViewModelを生成する
            viewModel = new DepartmentRegisterViewModel();
        }
        // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
        PopulateDepartments(viewModel);
        // viewModelをviewに渡して画面表示する
        return View(viewModel);
    }

    /// <summary>
    /// 入力画面の[完了]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentRegisterViewModel viewModel)
    {
        // バリデーションチェック
        if (!ModelState.IsValid) // バリデーションエラーあり
        {
            // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
            PopulateDepartments(viewModel);
            // 入力画面の表示
            return View("Enter", viewModel);
        }
        // 選択された部署のIdで部署データを取得する
        var department = _departmentRegisterService.GetById(viewModel.DeptId ?? 0);
        _logger.LogInformation($"部署Id:{viewModel.DeptId ?? 0}の部署を取得する");
        // ViewModelに部署名を設定する
        viewModel.DeptName = department.Name;
        // 確認画面を表示する
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[登録]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost("Regiter")]
    public IActionResult Register(DepartmentRegisterViewModel viewModel)
    {
        // EmployeeRegisterViewModelをシリアライズして、TempDataに保存する
        _deptDataStore.Save(this, viewModel);
        // 登録処理GETアクションメソッドにリダイレクトする
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
        DepartmentRegisterViewModel? viewModel = null;
        // TempDataからEmployeeRegisterViewModelを取得する
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            // データが存在しない場合、入力画面にリダイレクト
            return RedirectToAction("Enter");
        }
        // EmployeeRegisterFormをドメインモデル:Employeeに変換する
        var department = _adapter.Restore(viewModel!);
        // 新しい従業員を登録する
        _departmentRegisterService.Register(employee);
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    /// <returns></returns> 
    [HttpPost("Back")]
    public IActionResult Back(DepartmentRegisterViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        // EmployeeRegisterViewModelをシリアライズして、TempDataに保存する
        _deptDataStore.Save(this, viewModel);
        // 入力画面を出力するアクションメソッドにリダイレクトする
        return RedirectToAction("Enter");
    }

    /// <summary>
    /// 部署一覧を取得してViewModelに設定する(SelectListItem形式)
    /// </summary>
    private void PopulateDepartments(DepartmentRegisterViewModel viewModel)
    {
        // 従業員登録サービスから部署一覧を取得する
        var departments = _departmentRegisterService.GetDepartments();
        // 部署一覧をEmployeeRegisterViewModelに登録する
        viewModel.SetDepartments(departments);
        _logger.LogInformation("部署リストを設定");
    }
}