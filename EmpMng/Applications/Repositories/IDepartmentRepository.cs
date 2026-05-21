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

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns>取得して部署</returns>
    Department? FindById(int id);

    /// <summary>
    /// 部署を永続化する
    /// </summary>
    /// <param name="department">永続化対象の部署</param>
    void Create(Department department); // ← セミコロンで終わらせる
}
