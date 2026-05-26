using EmpMng.Applications.Adapters;
using EmpMng.Applications.Domains;

namespace EmpMng.Presentations.ViewModels;

/// <summary>
/// EmployeeRegisterViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
/// </summary>
public class EmployeeRegisterViewModelAdapter : IRestorer<Employee, EmployeeRegisterViewModel>
{
    /// <summary>
    /// EmployeeRegisterViewModelをドメインオブジェクト:Employeeに変換する
    /// </summary>
    /// <param name="target">EmployeeRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Employee Restore(EmployeeRegisterViewModel target)
    {
        Department? department = null;
        if (target.DeptId.HasValue)
        {
            department = new Department(target.DeptId!.Value, target.DeptName, 0);
        }

        var employee = new Employee(target.EmpId, target.EmpName!, department, target.Gender!.Value);
        return employee;
    }

    /// <summary>
    /// ドメインモデル(Employee)からViewModel(EmployeeRegisterViewModel)へ変換する
    /// </summary>
    public EmployeeRegisterViewModel Convert(Employee domain)
    {
        var viewModel = new EmployeeRegisterViewModel
        {
            EmpId = domain.Id,
            EmpName = domain.Name,
            // 💡 変更ポイント: C#の正しい条件演算子(三項演算子)に修正しました
            Gender = domain.Gender,
            DeptId = domain.Department?.Id
        };

        return viewModel;
    }
}
