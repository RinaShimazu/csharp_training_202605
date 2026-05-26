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
    Employee? FindById(int id);

    void UpdateEmployeeId(Employee employee);
}