using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Domains;

namespace EmpMng.Presentations.ViewModels;

/// <summary>
/// 従業員登録ViewModelクラス
/// </summary>
public class EmployeeRegisterViewModel
{
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "氏名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public string? Name { get; set; } = string.Empty;

    /// <summary>
    /// 所属部署ID
    /// </summary>
    [Display(Name = "所属部署")]
    public int? DeptId { get; set; } = null;

    /// <summary>
    /// 所属部署名
    /// </summary>
    [Display(Name = "所属部署")]
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 選択された性別の値
    /// </summary>
    [Display(Name = "性別")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public int? Gender { get; set; } = null;

    /// <summary>
    /// 性別の選択肢リスト
    /// </summary>
    public List<SelectListItem> GenderList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem{ Text="--未選択--", Value="" , Selected = true },
        new SelectListItem{ Text= "男性", Value= "1" },
        new SelectListItem{ Text= "女性", Value= "2" },
        new SelectListItem{ Text= "その他", Value= "3" },
    };

    /// <summary>
    /// 部署のリストをSelectListItemのリストに変換してプロパティに設定する
    /// </summary>
    /// <param name="departments"></param>
    public void SetDepartments(List<Department> departments)
    {
        // SelectListItemのリストを作成
        var selectItems = new List<SelectListItem>();
        foreach (var dept in departments)
        {
            if (dept.Id.HasValue)
            {
                var item = new SelectListItem();
                item.Value = dept.Id.Value.ToString();
                item.Text = string.IsNullOrEmpty(dept.Name) ? "(名称未設定)" : dept.Name;
                selectItems.Add(item);
            }
        }
        Departments = selectItems;
    }

    // 部署のリスト
    public List<SelectListItem>? Departments { get; set; } = null;

    public override string ToString()
    {
        return $"Name={Name} , DeptId={DeptId} , DeptName={DeptName} , Departments={Departments}, Gender={Gender}";
    }
}
