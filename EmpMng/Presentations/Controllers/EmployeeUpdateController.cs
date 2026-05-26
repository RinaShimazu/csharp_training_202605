using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;

namespace EmpMng.Presentations.Controllers;

/// <summary>
/// 従業員更新コントローラ
/// </summary>
[Route("EmployeeUpdate")]
public class EmployeeUpdateController : Controller
{
    private readonly ILogger<EmployeeUpdateController> _logger;
    private readonly IEmployeeRegisterService _employeeRegisterService;

    // 💡 型を「Register用」に一統し、変数名だけ更新でも違和感のない名前にしています
    private readonly EmployeeRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<EmployeeRegisterViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EmployeeUpdateController(
        ILogger<EmployeeUpdateController> logger, // 💡 ここも EmployeeUpdateController に修正
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
    [HttpGet("Enter/{id?}")] // 💡 URLからIDを受け取れるように変更
    public IActionResult Enter(int? id)
    {
        EmployeeRegisterViewModel? viewModel = null;

        // [戻る]ボタンへの対応
        viewModel = _empDataStore.Load(this);

        // 💡 初めて画面を開いたとき（idがある場合）、現在のデータをDBから引っ張ってくる処理
        if (viewModel == null && id.HasValue)
        {
            // ※もし GetById などのメソッド名が異なれば、サービスにあるデータ取得メソッド名に変えてください
            var employee = _employeeRegisterService.GetEmployeeId(id.Value);
            if (employee != null)
            {
                // ドメインモデルからViewModelに変換
                // ※アダプターに Convert メソッド（ドメイン→VM）がなければ自力で詰め替えてもOKです
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
            _logger.LogInformation($"部署Id:{viewModel.DeptId.Value}の部署を取得する");

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

        // 💡 登録(Register)ではなく、更新(Update)を呼び出す
        // ※ サービス側に「Update(Employee employee)」というメソッドを追加してください！
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
        _logger.LogInformation("部署リストを設定");
    }
}
