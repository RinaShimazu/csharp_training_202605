using EmpMng.Applications.Repositories;
using EmpMng.Applications.Domains;
using EmpMng.Exceptions;
using EmpMng.Infrastructures.Context;
namespace EmpMng.Applications.Services.Impls;
/// <summary>
/// 従業員登録サービスインターフェイスの実装
/// </summary>
public class DepartmentRegisterService : IDepartmentRegisterService
{

    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインオブジェクト:部署のCRUD操作インターフェイス
    /// </summary>
    private readonly IDepartmentRepository _departmentRepository;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">アプリケーション用DbContext</param>
    /// <param name="departmentRepository">部署のCRUD操作インターフェイス</param>
    public DepartmentRegisterService(
        AppDbContext context,
        IDepartmentRepository departmentRepository)
    {
        _context = context;
        _departmentRepository = departmentRepository;
    }

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns></returns>
    public Department GetDepartmentId(int id)
    {
        var result = _departmentRepository.FindById(id)!;
        if (result == null)
        {
            throw new NotFoundException($"部署Id{id}に該当する部署は存在しません");
        }
        return result;
    }

    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns></returns>
    public List<Department> GetDepartments()
    {
        return _departmentRepository.FindAll();
    }

    /// <summary>
    /// 新しい従業員を登録する
    /// </summary>
    /// <param name="employee"></param>
    public void Register(Department department)
    {
        try
        {
            // トランザクションの開始
            _context.Database.BeginTransaction();
            // 従業員の登録
            _departmentRepository.Create(department);
            // トランザクションのコミット
            _context.Database.CommitTransaction();
        }
        catch
        {
            // トランザクションのロールバック
            _context.Database.RollbackTransaction();
            throw;
        }
    }
    public void UpdateDepartmentId(Department department)
    {
        try
        {
            // トランザクションの開始
            _context.Database.BeginTransaction();
            // 従業員の登録
            _departmentRepository.UpdateDepartmentId(department);
            // トランザクションのコミット
            _context.Database.CommitTransaction();
        }
        catch
        {
            // トランザクションのロールバック
            _context.Database.RollbackTransaction();
            throw;
        }
    }
}