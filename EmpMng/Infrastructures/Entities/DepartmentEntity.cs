using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EmpMng.Infrastructures.Entities;
/// <summary>
/// 部署テーブル(department)を扱うEntity Framework Coreのエンティティクラス
/// </summary>
[Table("department")]
public class DepartmentEntity
{
    /// <summary>
    /// 部署Id(主キー)
    /// </summary> 
    [Key]
    [Column("id")]
    public int Id { get; set; }
    /// <summary>
    /// 部署名
    /// </summary> 
    [Column("name")]
    public string Name { get; set; } = string.Empty;
}