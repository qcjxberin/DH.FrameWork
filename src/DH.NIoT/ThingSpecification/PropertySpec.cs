﻿using NewLife.Collections;

namespace NewLife.IoT.ThingSpecification;

/// <summary>
/// 属性规范
/// </summary>
public class PropertySpec : SpecBase, IDictionarySource
{
    #region 属性
    /// <summary>
    /// 访问模式
    /// </summary>
    public String AccessMode { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public TypeSpec DataType { get; set; }

    /// <summary>
    /// 采集点位置信息
    /// </summary>
    public String Address { get; set; }
    #endregion

    #region 创建
    /// <summary>快速创建属性</summary>
    /// <param name="id">标识</param>
    /// <param name="name">名称</param>
    /// <param name="type">类型</param>
    /// <param name="length">长度</param>
    /// <param name="address">点位地址</param>
    /// <returns></returns>
    public static PropertySpec Create(String id, String name, String type, Int32 length = 0, String address = null)
    {
        var ps = new PropertySpec
        {
            Id = id,
            Name = name,
            Address = address
        };

        if (type != null)
        {
            ps.DataType = new TypeSpec { Type = type };

            if (length > 0)
                ps.DataType.Specs = new DataSpecs { Length = length };
        }

        return ps;
    }
    #endregion

    #region 方法
    /// <summary>转字典。根据不同类型，提供不一样的序列化能力</summary>
    /// <returns></returns>
    public IDictionary<String, Object> ToDictionary()
    {
        var dic = new Dictionary<String, Object>();
        if (!Id.IsNullOrEmpty())
            dic.Add("identifier", Id);
        if (!Name.IsNullOrEmpty())
            dic.Add(nameof(Name), Name);
        if (Required)
            dic.Add(nameof(Required), Required);

        if (!AccessMode.IsNullOrEmpty())
            dic.Add(nameof(AccessMode), AccessMode);

        var dt = DataType?.ToDictionary();
        if (dt != null)
            dic.Add(nameof(DataType), dt);

        if (!Address.IsNullOrEmpty())
            dic.Add(nameof(Address), Address);

        return dic;
    }

    /// <summary>
    /// 已重载。友好显示
    /// </summary>
    /// <returns></returns>
    public override String ToString() => $"{Id} {Name} {DataType}";
    #endregion
}