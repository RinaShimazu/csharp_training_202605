using EmpMng.Applications.Domains;

namespace EmpMng.Applications.Repositories;

/// <summary>
/// ドメインオブジェクト:部署のCRUD操作インターフェイス
/// </summary>
public interface IDepartmentRepository
{
    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns>部署のリスト</returns>
    List<Department> FindAll();
    Department? FindById(int id);

    /// <summary>
    /// 部署を永続化する
    /// </summary>
    /// <param name="department">永続化対象の部署</param>
    void Create(Department department);

    /// <summary>
    /// 部署を更新する
    /// </summary>
    void UpdateDepartmentId(Department department);
    void DeleteDepartmentId(Department department);
}
