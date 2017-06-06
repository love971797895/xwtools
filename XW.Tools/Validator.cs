using System;
using System.Text.RegularExpressions;

namespace XW.Tools
{
    /// <summary>
    /// 基础验证类
    /// </summary>
    public class Validator
    {
        #region 值类验证
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(string input)
        {
            if (input != null)
            {
                string str = input;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInt(string input)
        {
            return Regex.IsMatch(input, @"^[0-9]*$");
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDouble(object input)
        {
            if (input != null)
                return Regex.IsMatch(input.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }
        #endregion

        #region 常用验证
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="input">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string input)
        {
            return Regex.IsMatch(input, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        /// <summary>
        /// 检测当前电话号码是否合法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTelphone(string input)
        {
            Regex regex = new Regex(@"^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?$", RegexOptions.IgnoreCase);
            return regex.Match(input).Success;
        }

        /// <summary>
        /// 判断是否包含英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglishCh(string input)
        {
            return Regex.IsMatch(input, "^[A-Za-z]+$");
        }
        #endregion

        #region 检测颜色是否合法
        /// <summary>
        /// 检查颜色值是否为3/6位的合法颜色
        /// </summary>
        /// <param name="color">待检查的颜色</param>
        /// <returns></returns>
        public static bool CheckColorValue(string color)
        {
            if (string.IsNullOrEmpty(color))
                return false;
            color = color.Trim().Trim('#');
            if (color.Length != 3 && color.Length != 6)
                return false;
            //不包含0-9  a-f以外的字符
            if (!Regex.IsMatch(color, "[^0-9a-f]", RegexOptions.IgnoreCase))
                return true;

            return false;
        }
        #endregion

        #region 根据正则验证自己的值是否合理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsReasonable(string value,string regex)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value.ToString(), regex);
        }
        #endregion
    }
}
