using EmpMng.Applications.Domains;
namespace EmpMng.Applications.Services;
/// <summary>
/// 従業員登録サービスインターフェイス
/// </summary>
public interface IDepartmentRegisterService
{
    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns></returns>
    List<Department> GetDepartments();

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns></returns>
    Department GetDepartmentId(int id);

    /// <summary>
    /// 新しい部門を登録する
    /// </summary>
    /// <param name="department"></param>
    void Register(Department department);

    /// <summary>
    /// 部門を更新する
    /// </summary>
    void UpdateDepartmentId(Department department);

    void DeleteDepartmentId(Department department);
}