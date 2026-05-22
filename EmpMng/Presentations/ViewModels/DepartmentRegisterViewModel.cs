using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmpMng.Applications.Domains;
namespace EmpMng.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class DepartmentRegisterViewModel
{
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "部門名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public string? DeptName { get; set; } = string.Empty;


    /// <summary>
    /// 選択された部署名
    /// </summary>
    [Display(Name = "部門番号")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public int DeptId { get; set; }

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


    [Display(Name = "勤務地")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public int? Area { get; set; } = null;

    /// <summary>
    /// 勤務地の選択肢リスト
    /// </summary>
    public List<SelectListItem> AreaList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem{ Text="--未選択--", Value="" , Selected = true },
        new SelectListItem{ Text= "東京", Value= "0" },
        new SelectListItem{ Text= "大阪", Value= "1" }
    };

    public override string ToString()
    {
        return $"Name={DeptName} , Area={Area}";
    }

}