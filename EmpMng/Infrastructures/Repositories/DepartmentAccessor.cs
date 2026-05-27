using EmpMng.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using EmpMng.Infrastructures.Context;
using EmpMng.Applications.Domains;
using EmpMng.Applications.Repositories;
using EmpMng.Infrastructures.Adapters;
using EmpMng.Exceptions;

namespace EmpMng.Infrastructures.Repositories;

public class DepartmentAccessor
{
    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインモデル:部門と部門エンティティの相互変換インターフェイスの実装
    /// </summary>
    public DepartmentAccessor(AppDbContext context, DepartmentEntityAdapter adapter)
    {
        _context = context;
    }



    /// <summary>
    /// 部門を変更する
    /// </summary>
    /// <param name="id">変更対象の部門の主キー値</param>
    /// <returns></returns>
    public DepartmentEntity UpdateDepartmentId(DepartmentEntity department)
    {

        var result = _context.Departments.Find(department.DeptId);
        if (department == null)
        {
            return null!;
        }
        result!.DeptName = department.DeptName;
        result.Area = department.Area;
        // 変更を永続化する
        _context.SaveChanges();
        return department;
    }
}