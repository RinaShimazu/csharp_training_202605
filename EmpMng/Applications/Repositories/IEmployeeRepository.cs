using EmpMng.Applications.Domains;
namespace EmpMng.Applications.Repositories;
/// <summary>
/// ドメインオブジェクト:従業員のCRUD操作インターフェイス
/// </summary>
public interface IEmployeeRepository
{
    List<Employee> FindAll();
    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">永続化対象の従業員</param>
    void Create(Employee employee);
    /// <summary>
    /// すべての社員を取得する
    /// </summary>
    Employee? FindById(int id);
    /// <summary>
    /// 従業員を更新する
    /// </summary>
    void UpdateEmployeeId(Employee employee);
    /// <summary>
    /// 従業員を削除する
    /// </summary>
    void DeleteEmployeeId(Employee employee);

}