using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace XW.Tools
{
    /// <summary>
    /// 日志记录（适用于CS和BS程序）
    /// </summary>
    public class WriteLog
    {
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        private static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="ex">异常类</param>
        public static void CreateLog(Exception ex)
        {
            string path = GetMapPath("~/Log/");
            if (!Directory.Exists(path))
            {
                //创建日志文件夹
                Directory.CreateDirectory(path);
            }
            //发生异常每天都创建一个单独的日子文件[*.log],每天的错误信息都在这一个文件里。方便查找
            path += DateTime.Now.ToString("yyyy-MM-dd") + "Error.log";
            WriteLogInfo(ex, path);
        }
        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="ex">异常类</param>
        /// <param name="path">日志文件存放路径</param>
        private static void WriteLogInfo(Exception ex, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
            {
                sw.WriteLine("******************************XW.Tools【"
                               + DateTime.Now.ToLongTimeString()
                               + "】*****************************************");
                if (ex != null)
                {
                    sw.WriteLine("【ErrorType】" + ex.GetType());
                    sw.WriteLine("【TargetSite】" + ex.TargetSite);
                    sw.WriteLine("【Message】" + ex.Message);
                    sw.WriteLine("【Source】" + ex.Source);
                    sw.WriteLine("【StackTrace】" + ex.StackTrace);
                }
                else
                {
                    sw.WriteLine("Exception is NULL");
                }
                sw.WriteLine();
            }
        }

        /// <summary>
        /// 操作日志记录
        /// </summary>
        /// <param name="record">记录信息</param>
        /// <param name="info">换行处理的信息</param>
        public static void WriteRecord(string record, string info = null)
        {
            string path = GetMapPath("~/Log/");
            if (!Directory.Exists(path))
            {
                //创建日志文件夹
                Directory.CreateDirectory(path);
            }
            //发生异常每天都创建一个单独的日子文件[*.log],每天的错误信息都在这一个文件里。方便查找
            path += DateTime.Now.ToString("yyyy-MM-dd") + "Record.log";
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
            {
                sw.WriteLine("********************************XW.Tools【"
                          + DateTime.Now.ToLongTimeString()
                          + "】*****************************************");
                sw.WriteLine("Record：" + record);
                if (!string.IsNullOrEmpty(info))
                { sw.WriteLine("Info:" + info); }
                sw.WriteLine();
            }
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteRecord(List<string> info)
        {
            if (info != null && info.Count > 0)
            {
                string path = GetMapPath("~/Log/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);//创建日志文件夹
                }
                //发生异常每天都创建一个单独的日子文件[*.log],每天的错误信息都在这一个文件里。方便查找
                path += DateTime.Now.ToString("yyyy-MM-dd") + "Record.log";
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                {
                    sw.WriteLine("*****************************XW.Tools【"
                              + DateTime.Now.ToLongTimeString()
                              + "】*****************************************");
                    foreach (var item in info)
                    {
                        sw.WriteLine(item);
                    }
                }
            }
        }
    }
}
