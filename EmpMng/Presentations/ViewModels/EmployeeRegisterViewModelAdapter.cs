using EmpMng.Applications.Adapters;
using EmpMng.Applications.Domains;
namespace EmpMng.Presentations.ViewModels;
/// <summary>
/// EmployeeRegisterViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Employee</typeparam>
/// <typeparam name="TTarget">EmployeeRegisterForm</typeparam>
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
            // Department(部署)を作成する
            department = new Department(target.DeptId!.Value, target.DeptName, 0);
        }   // 登録するEmployee(従業員)を作成するs
        var employee = new Employee(null, target.Name!, department, target.Gender!.Value);
        return employee;
    }
}