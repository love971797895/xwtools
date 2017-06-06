using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XW.Tools.DB
{
    /// <summary>
    /// SQL语句操作辅助类
    /// </summary>
    public class QureyPara
    {
        /// <summary>
        /// 公用属性
        /// </summary>
        public QureyPara() { }

        /// <summary>
        /// 查询实例
        /// </summary>
        /// <param name="Feild">字段名</param>
        /// <param name="feildvalue">字段值</param>
        /// <param name="FeildType">运算类型</param>
        public QureyPara(string Feild, object feildvalue, QureyType FeildType)
        {
            this._feild = Feild;
            this._feildvalue = feildvalue;
            this._feildtype = FeildType;
        }
        string _feild = "";
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Feild
        {
            get { return _feild; }
            set { _feild = value; }
        }

        object _feildvalue;
        /// <summary>
        /// 字段的值
        /// </summary>
        public object FeildValue
        {
            get { return _feildvalue; }
            set { _feildvalue = value; }
        }

        QureyType _feildtype;
        /// <summary>
        /// 字段的操作类型
        /// </summary>
        public QureyType FeildType
        {
            get { return _feildtype; }
            set { _feildtype = value; }
        }

        /// <summary>
        /// 字段的操作类型（只读）
        /// </summary>
        public string FeildTypeValue
        {
            get { return GetFeildType(); }
        }

        private int _isor = 0;
        /// <summary>
        /// 默认值为0,为and，若为1，则说明用or
        /// </summary>
        public int IsOr
        {
            get { return _isor; }
            set { _isor = value; }
        }

        /// <summary>
        /// 根据字段的操作类型
        /// </summary>
        /// <returns></returns>
        private string GetFeildType()
        {
            string feildtypevalue = "";
            switch (_feildtype)
            {
                case QureyType.EGreater:
                    feildtypevalue = " >= ";
                    break;
                case QureyType.ESmaller:
                    feildtypevalue = " <= ";
                    break;
                case QureyType.Greater:
                    feildtypevalue = " > ";
                    break;
                case QureyType.Like:
                    feildtypevalue = " like ";
                    break;
                case QureyType.NLike:
                    feildtypevalue = " not like ";
                    break;
                case QureyType.NEqual:
                    feildtypevalue = " <> ";
                    break;
                case QureyType.Smaller:
                    feildtypevalue = " < ";
                    break;
                case QureyType.Inner:
                    feildtypevalue = " in ";
                    break;
                case QureyType.NInner:
                    feildtypevalue = " not in ";
                    break;
                case QureyType.IsNull:
                    feildtypevalue = " is ";//null    pq.FeildValue = DBNull.Value;
                    break;
                case QureyType.IsNotNull:
                    feildtypevalue = " is not ";//null    pq.FeildValue = DBNull.Value;
                    break;
                default:
                    feildtypevalue = " = ";
                    break;
            }
            return feildtypevalue;
        }

        /// <summary>
        /// 运算符下拉类型转换
        /// </summary>
        /// <param name="qts">运算符下拉值</param>
        /// <returns>QureyType型值</returns>
        public QureyType GetQT(string qts)
        {
            switch (qts)
            {
                case "EGreater":
                    return QureyType.EGreater;

                case "ESmaller":
                    return QureyType.ESmaller;

                case "Greater":
                    return QureyType.Greater;

                case "Like":
                    return QureyType.Like;

                case "NEqual":
                    return QureyType.NEqual;

                case "Smaller":
                    return QureyType.Smaller;

                case "Inner":
                    return QureyType.Inner;

                case "NInner":
                    return QureyType.NInner;

                case "IsNull":
                    return QureyType.IsNull;

                case "IsNotNull":
                    return QureyType.IsNotNull;
                case "NLike":
                    return QureyType.NLike;

                default:
                    return QureyType.Equal;
            }
        }
    }
}
