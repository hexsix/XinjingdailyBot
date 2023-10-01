using SqlSugar;
using XinjingdailyBot.Model.Base;
using XinjingdailyBot.Model.Columns;

namespace XinjingdailyBot.Model.Models;

/// <summary>
/// 广告消息表
/// </summary>
[SugarTable("review_status", TableDescription = "审核统计")]
public sealed record ReviewStatus : BaseModel, IModifyAt, ICreateAt
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    /// <summary>
    /// 原会话ID
    /// </summary>
    public long ChatID { get; set; }
    /// <summary>
    /// 原消息ID
    /// </summary>
    public long MessageID { get; set; }
    /// <summary>
    /// 是否被删除
    /// </summary>
    public bool Deleted { get; set; }
    /// <inheritdoc cref="ICreateAt"/>
    public DateTime CreateAt { get; set; } = DateTime.Now;
    /// <inheritdoc cref="IModifyAt"/>
    public DateTime ModifyAt { get; set; } = DateTime.Now;
}
