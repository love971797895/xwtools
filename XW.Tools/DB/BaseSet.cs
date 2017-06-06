using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XW.Tools.DB
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseSet
    {
        /// <summary>
        /// 
        /// </summary>
        public static string NULL
        {
            get { return "@null"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DateTimeShortNull
        {
            get { return DateTime.Now.ToShortDateString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DateTimeLongNull
        {
            get { return DateTime.Now.ToLongTimeString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string PrimaryKey
        {
            get { return "PrimaryKey"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string TableName
        {
            get { return "TableName"; }
        }
    }
}
