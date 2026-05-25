using EmpMng.Applications.Adapters;
using EmpMng.Applications.Domains;
using EmpMng.Infrastructures.Entities;

namespace EmpMng.Infrastructures.Adapters;

/// <summary>
/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換インターフェイスの実装
/// </summary>
public class EmployeeEntityAdapter :
    IConverter<Employee, EmployeeEntity>, IRestorer<Employee, EmployeeEntity>
{
    /// <summary>
    /// ドメインオブジェクト:EmployeeをEmployeeEntityに変換する
    /// </summary>
    /// <param name="domain">ドメインモデル:従業員</param>
    /// <returns>EmployeeEntity</returns>
    public EmployeeEntity Convert(Employee domain)
    {
        var entity = new EmployeeEntity
        {
            EmpName = domain.Name,
            // 💡 性別(Gender)を無条件、またはドメインの仕様に合わせてセット
            Gender = domain.Gender
        };

        if (domain.Id != null)
        {
            entity.EmpId = domain.Id.Value;
        }

        if (domain.Department != null)
        {
            entity.DeptId = domain.Department.Id;
        }

        return entity;
    }

    /// <summary>
    /// EmployeeEntityからドメインオブジェクト:Employeeを復元する
    /// </summary>
    /// <param name="target">EmployeeEntity</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Employee Restore(EmployeeEntity target)
    {
        Department? domainDept = null;

        if (target.Department != null)
        {
            domainDept = new Department(
                target.Department.DeptId,
                target.Department.DeptName,
                target.Department.Area
            );
        }
        else if (target.DeptId.HasValue)
        {
            domainDept = new Department(target.DeptId.Value, "部門不明", 0);
        }

        // 組み立てた部署データをドメインに渡す
        var employee = new Employee(
            target.EmpId,
            target.EmpName,
            domainDept,
            target.Gender
        );

        return employee;
    }

}
