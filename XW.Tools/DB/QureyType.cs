using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XW.Tools.DB
{
    /// <summary>
    /// 常见类型转换
    /// </summary>
    public enum QureyType
    {
        /// <summary>
        /// 不近似
        /// </summary>
        NLike,
        /// <summary>
        /// 近似
        /// </summary>
        Like,
        /// <summary>
        /// 相等
        /// </summary>
        Equal,
        /// <summary>
        /// 大于
        /// </summary>
        Greater,
        /// <summary>
        /// 小于
        /// </summary>
        Smaller,
        /// <summary>
        /// 大于等于
        /// </summary>
        EGreater,
        /// <summary>
        /// 小于等于
        /// </summary>
        ESmaller,
        /// <summary>
        /// 不等于
        /// </summary>
        NEqual,
        /// <summary>
        /// 包含
        /// </summary>
        Inner,
        /// <summary>
        /// 不包含
        /// </summary>
        NInner,
        /// <summary>
        /// 为空(DBNull.Value)
        /// </summary>
        IsNull,
        /// <summary>
        /// 不为空(DBNull.Value)
        /// </summary>
        IsNotNull
    }
}
