using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XW.Tools.DB
{
    /// <summary>
    /// DB辅助对象转化为SQL语句
    /// </summary>
    public class QureyParaConvert
    {
        /// <summary>
        /// 条件查询转换拼接
        /// </summary>
        /// <param name="lQureyPara"></param>
        /// <returns></returns>
        public static string ConvertString(List<QureyPara> lQureyPara)
        {
            string where = " 1=1 ";
            StringBuilder strSql = new StringBuilder();
            lQureyPara.ForEach(item => where += GerStrSql(item));
            return strSql.Append(where).ToString();
        }

        /// <summary>
        /// 类型转化
        /// </summary>
        /// <param name="item">QureyPara对象</param>
        /// <returns></returns>
        public static string GerStrSql(QureyPara item)
        {
            string str = "";
            if (item.IsOr == 0)
            {
                switch (item.FeildType)
                {
                    case QureyType.Inner:
                    case QureyType.NInner:
                        str = " AND " + item.Feild + " " + item.FeildTypeValue + " " + "(" + item.FeildValue + ")";
                        break;
                    case QureyType.Like:
                    case QureyType.NLike:
                        str = " AND " + item.Feild + " " + item.FeildTypeValue + " '%" + item.FeildValue + "%' ";
                        break;
                    default:
                        str = " AND " + item.Feild + " " + item.FeildTypeValue + " '" + item.FeildValue + "' ";
                        break;
                }
            }
            else
            {
                switch (item.FeildType)
                {
                    case QureyType.Inner:
                    case QureyType.NInner:
                        str = " OR " + item.Feild + " " + item.FeildTypeValue + " " + "(" + item.FeildValue + ")";
                        break;
                    case QureyType.Like:
                    case QureyType.NLike:
                        str = " OR " + item.Feild + " " + item.FeildTypeValue + " '%" + item.FeildValue + "%' ";
                        break;
                    default:
                        str = " OR " + item.Feild + " " + item.FeildTypeValue + " '" + item.FeildValue + "' ";
                        break;
                }
            }
            return str;
        }
    }
}
