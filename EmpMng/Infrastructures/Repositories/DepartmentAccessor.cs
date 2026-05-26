using EmpMng.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using EmpMng.Infrastructures.Context;
using EmpMng.Applications.Domains;
using EmpMng.Applications.Repositories;
using EmpMng.Infrastructures.Adapters;
using EmpMng.Exceptions;

namespace EmpMng.Infrastructures.Repositories;
/// <summary>
/// itemテーブルにアクセスするクラス
/// </summary>
/// <author>Fullness,Inc.</author>
/// <date>2025-11-16</date>
/// <version>1.0.0</version>
public class DepartmentAccessor
{
    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインモデル:従業員と従業員エンティティの相互変換インターフェイスの実装
    /// </summary>
    public DepartmentAccessor(AppDbContext context, DepartmentEntityAdapter adapter)
    {
        _context = context;
    }



    /// <summary>
    /// 商品を変更する
    /// </summary>
    /// <param name="id">変更対象の商品の主キー値</param>
    /// <returns></returns>
    public DepartmentEntity UpdateById(DepartmentEntity department)
    {

        // 商品Idを指定して商品を取得する
        var result = _context.Departments.Find(department.DeptId);
        if (department == null)
        {
            return null!; // 商品が見つからない場合はnullを返す
        }
        // 商品名と単価を変更する
        result!.DeptName = department.DeptName;
        result.Area = department.Area;
        // 変更を永続化する
        _context.SaveChanges();
        return department;
    }
}