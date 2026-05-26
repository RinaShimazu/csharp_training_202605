using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Services;
using EmpMng.Presentations.ViewModels;
using EmpMng.Exceptions;

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
    /// 従業員更新(入力)画面の初期表示
    /// </summary>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        // 最初は空のモデルを用意するだけ
        var viewModel = _empDataStore.Load(this) ?? new EmployeeRegisterViewModel();

        PopulateDepartments(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// 💡 追加ポイント②: 入力された社員番号からデータを検索して画面にセットする処理
    /// </summary>
    [HttpPost("LoadEmployee")]
    public IActionResult LoadEmployee(EmployeeRegisterViewModel inputModel)
    {
        // 画面のテキストボックスに入力された社員番号を取得
        if (inputModel.EmpId.HasValue)
        {
            try
            {
                // データベースから社員を取得
                var employee = _employeeRegisterService.GetEmployeeId(inputModel.EmpId.Value);

                if (employee != null)
                {
                    // データをViewModelに詰め替える
                    var viewModel = _adapter.Convert(employee);

                    // 部署一覧を再セットして、入力欄にデータが入った状態で画面を返す
                    PopulateDepartments(viewModel);
                    return View("Enter", viewModel);
                }
            }
            catch (NotFoundException ex)
            {
                // 先ほど作った「該当する社員は存在しません」の例外をキャッチした場合
                // 画面にエラーメッセージを表示させる
                ModelState.AddModelError("EmpId", ex.Message);
            }
        }

        // 社員が見つからなかった、またはエラーの場合は空に近い状態のまま再表示
        PopulateDepartments(inputModel);
        return View("Enter", inputModel);
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
