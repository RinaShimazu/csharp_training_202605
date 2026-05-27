using EmpMng.Applications.Adapters;
using EmpMng.Applications.Domains;
namespace EmpMng.Presentations.ViewModels;
/// <summary>
/// EmployeeRegisterViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Employee</typeparam>
/// <typeparam name="TTarget">EmployeeRegisterForm</typeparam>
public class DepartmentRegisterViewModelAdapter : IRestorer<Department, DepartmentRegisterViewModel>
{
    /// <summary>
    /// EmployeeRegisterViewModelをドメインオブジェクト:Employeeに変換する
    /// </summary>
    /// <param name="target">EmployeeRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Department Restore(DepartmentRegisterViewModel target)
    {
        var department = new Department(target.DeptId, target.DeptName, target.Area ?? 0);
        return department;
    }
    public DepartmentRegisterViewModel Convert(Department domain)
    {
        var viewModel = new DepartmentRegisterViewModel
        {
            DeptId = domain.Id ?? 0,
            DeptName = domain.Name,
            Area = domain.Area,
        };

        return viewModel;
    }
}