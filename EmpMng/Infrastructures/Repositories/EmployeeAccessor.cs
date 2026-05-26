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
public class EmployeeAccessor
{
    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインモデル:従業員と従業員エンティティの相互変換インターフェイスの実装
    /// </summary>
    public EmployeeAccessor(AppDbContext context, EmployeeEntityAdapter adapter)
    {
        _context = context;
    }



    /// <summary>
    /// 商品を変更する
    /// </summary>
    /// <param name="id">変更対象の商品の主キー値</param>
    /// <returns></returns>
    public EmployeeEntity UpdateById(EmployeeEntity employee)
    {

        // 商品Idを指定して商品を取得する
        var result = _context.Employees.Find(employee.EmpId);
        if (employee == null)
        {
            return null!; // 商品が見つからない場合はnullを返す
        }
        // 商品名と単価を変更する
        result!.EmpName = employee.EmpName;
        result.Gender = employee.Gender;
        // 変更を永続化する
        _context.SaveChanges();
        return employee;
    }
}