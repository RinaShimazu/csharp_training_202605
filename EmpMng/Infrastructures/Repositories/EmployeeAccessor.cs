using EmpMng.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using EmpMng.Infrastructures.Context;
using EmpMng.Applications.Domains;
using EmpMng.Applications.Repositories;
using EmpMng.Infrastructures.Adapters;
using EmpMng.Exceptions;

namespace EmpMng.Infrastructures.Repositories;

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
    /// 社員を変更する
    /// </summary>
    /// <param name="id">変更対象の社員の主キー値</param>
    /// <returns></returns>
    public EmployeeEntity UpdateById(EmployeeEntity employee)
    {
        var result = _context.Employees.Find(employee.EmpId);
        if (employee == null)
        {
            return null!;
        }
        result!.EmpName = employee.EmpName;
        result.Gender = employee.Gender;
        // 変更を永続化する
        _context.SaveChanges();
        return employee;
    }

    /// <summary>
    /// 商品を削除する
    /// </summary>
    /// <param name="item">削除対象の商品</param>
    /// <returns>削除したエンティティ</returns>
    public EmployeeEntity? DeleteEmployeeId(EmployeeEntity employee)
    {
        var result = _context.Employees.Find(employee.EmpId);
        if (result == null)
        {
            return null;// 商品が見つからない場合はnullを返す
        }
        // 商品を削除する
        var delResult = _context.Employees.Remove(result);
        // 削除を永続化する
        _context.SaveChanges();
        return delResult.Entity;
    }
}