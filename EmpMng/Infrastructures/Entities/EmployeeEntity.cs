using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EmpMng.Infrastructures.Entities;
/// <summary>
/// 従業員テーブル(employee)を扱うEntity Framework Coreのエンティティクラス
/// </summary>
[Table("employee")]
public class EmployeeEntity
{
    /// <summary>
    /// 従業員Id(主キー)
    /// </summary>
    [Key]
    [Column("id")]
    public int EmpId { get; set; }

    /// <summary>
    /// 従業員名
    /// </summary>
    [Column("name")]
    public string EmpName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部署Id(外部キー)
    /// </summary>
    [Column("dept_id")]
    public int? DeptId { get; set; }

    /// <summary>
    /// 性別ID
    /// </summary>
    [Column("gender_id")]
    public int Gender { get; set; }

    /// <summary>
    /// 所属部門
    /// </summary>
    [ForeignKey("DeptId")]
    public virtual DepartmentEntity? Department { get; set; }

}