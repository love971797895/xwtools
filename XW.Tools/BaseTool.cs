using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XW.Tools
{
    /// <summary>
    /// 基础方法
    /// </summary>
    public class BaseTool
    {
        #region Datetable转Json
        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <param name="isdispose">数据表转换结束后是否dispose掉</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJson(DataTable dt, bool isdispose)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[\r\n");

            //数据表字段名和类型数组
            string[] dt_field = new string[dt.Columns.Count];
            int i = 0;
            string formatStr = "{{";
            string fieldtype = "";
            foreach (DataColumn dc in dt.Columns)
            {
                dt_field[i] = dc.Caption.ToLower().Trim();
                formatStr += "'" + dc.Caption.ToLower().Trim() + "':";
                fieldtype = dc.DataType.ToString().Trim().ToLower();
                if (fieldtype.IndexOf("int") > 0 || fieldtype.IndexOf("deci") > 0 ||
                    fieldtype.IndexOf("floa") > 0 || fieldtype.IndexOf("doub") > 0 ||
                    fieldtype.IndexOf("bool") > 0)
                {
                    formatStr += "{" + i + "}";
                }
                else
                {
                    formatStr += "'{" + i + "}'";
                }
                formatStr += ",";
                i++;
            }
            if (formatStr.EndsWith(","))
                formatStr = formatStr.Substring(0, formatStr.Length - 1);//去掉尾部","号
            formatStr += "}},";
            i = 0;
            object[] objectArray = new object[dt_field.Length];
            foreach (DataRow dr in dt.Rows)
            {
                foreach (string fieldname in dt_field)
                {   //对 \ , ' 符号进行转换 
                    objectArray[i] = dr[dt_field[i]].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'");
                    switch (objectArray[i].ToString())
                    {
                        case "True":
                            {
                                objectArray[i] = "true"; break;
                            }
                        case "False":
                            {
                                objectArray[i] = "false"; break;
                            }
                        default: break;
                    }
                    i++;
                }
                i = 0;
                stringBuilder.Append(string.Format(formatStr, objectArray));
            }
            if (stringBuilder.ToString().EndsWith(","))
                stringBuilder.Remove(stringBuilder.Length - 1, 1);//去掉尾部","号
            if (isdispose)
                dt.Dispose();
            return stringBuilder.Append("\r\n];");
        }
        #endregion

        #region 把DateTable转换为List
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="dt">要转化的DateTable</param>
        /// <returns></returns>
        public static List<T> TableToList<T>(DataTable dt)
            where T : new()
        {
            if (dt == null) return null;
            if (dt.Rows.Count <= 0) return null;

            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();  //获取泛型的属性
            List<DataColumn> listColumns = dt.Columns.Cast<DataColumn>().ToList();  //获取数据集的表头，以便于匹配
            T t;
            foreach (DataRow dr in dt.Rows)
            {
                t = new T();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {
                        DataColumn dColumn = listColumns.Find(name => name.ToString().ToUpper() == propertyInfo.Name.ToUpper());  //查看是否存在对应的列名
                        if (dColumn != null)
                        {
                            if (dr[propertyInfo.Name] != DBNull.Value)//必须判断对象是否为DBnull值 否则会报错
                            {
                                propertyInfo.SetValue(t, dr[propertyInfo.Name], null);  //赋值
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.CreateLog(ex);
                    }
                }
                list.Add(t);
            }
            return list;
        }
        #endregion

        #region 将全角数字转换为数字
        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }
        #endregion

        #region RMB转换
        private const string DXSZ = "零壹贰叁肆伍陆柒捌玖";
        private const string DXDW = "毫厘分角元拾佰仟萬拾佰仟亿拾佰仟萬兆拾佰仟萬亿京拾佰仟萬亿兆垓";
        private const string SCDW = "元拾佰仟萬亿京兆垓";

        /// <summary>
        /// 转换整数为大写金额
        /// 最高精度为垓，保留小数点后4位，实际精度为亿兆已经足够了，理论上精度无限制，如下所示：
        /// 序号:...30.29.28.27.26.25.24  23.22.21.20.19.18  17.16.15.14.13  12.11.10.9   8 7.6.5.4  . 3.2.1.0
        /// 单位:...垓兆亿萬仟佰拾        京亿萬仟佰拾       兆萬仟佰拾      亿仟佰拾     萬仟佰拾元 . 角分厘毫
        /// 数值:...1000000               000000             00000           0000         00000      . 0000
        /// 下面列出网上搜索到的数词单位：
        /// 元、十、百、千、万、亿、兆、京、垓、秭、穰、沟、涧、正、载、极
        /// </summary>
        /// <param name="capValue">整数值</param>
        /// <returns>返回大写金额</returns>
        public string ConvertIntToUppercaseAmount(string capValue)
        {
            string currCap = "";    //当前金额
            string capResult = "";  //结果金额
            string currentUnit = "";//当前单位
            string resultUnit = ""; //结果单位           
            int prevChar = -1;      //上一位的值
            int currChar = 0;       //当前位的值
            int posIndex = 4;       //位置索引，从"元"开始

            if (Convert.ToDouble(capValue) == 0) return "";
            for (int i = capValue.Length - 1; i >= 0; i--)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (posIndex > 30)
                {
                    //已超出最大精度"垓"。注：可以将30改成22，使之精确到兆亿就足够了
                    break;
                }
                else if (currChar != 0)
                {
                    //当前位为非零值，则直接转换成大写金额
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    //防止转换后出现多余的零,例如：3000020
                    switch (posIndex)
                    {
                        case 4: currCap = "元"; break;
                        case 8: currCap = "萬"; break;
                        case 12: currCap = "亿"; break;
                        case 17: currCap = "兆"; break;
                        case 23: currCap = "京"; break;
                        case 30: currCap = "垓"; break;
                        default: break;
                    }
                    if (prevChar != 0)
                    {
                        if (currCap != "")
                        {
                            if (currCap != "元") currCap += "零";
                        }
                        else
                        {
                            currCap = "零";
                        }
                    }
                }
                //对结果进行容错处理               
                if (capResult.Length > 0)
                {
                    resultUnit = capResult.Substring(0, 1);
                    currentUnit = DXDW.Substring(posIndex, 1);
                    if (SCDW.IndexOf(resultUnit) > 0)
                    {
                        if (SCDW.IndexOf(currentUnit) > SCDW.IndexOf(resultUnit))
                        {
                            capResult = capResult.Substring(1);
                        }
                    }
                }
                capResult = currCap + capResult;
                prevChar = currChar;
                posIndex += 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// 转换小数为大写金额
        /// </summary>
        /// <param name="capValue">小数值</param>
        /// <param name="addZero">是否增加零位</param>
        /// <returns>返回大写金额</returns>
        private string ConvertDecToUppercaseAmount(string capValue, bool addZero)
        {
            string currCap = "";
            string capResult = "";
            int prevChar = addZero ? -1 : 0;
            int currChar = 0;
            int posIndex = 3;

            if (Convert.ToInt16(capValue) == 0) return "";
            for (int i = 0; i < capValue.Length; i++)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (currChar != 0)
                {
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    if (Convert.ToInt16(capValue.Substring(i)) == 0)
                    {
                        break;
                    }
                    else if (prevChar != 0)
                    {
                        currCap = "零";
                    }
                }
                capResult += currCap;
                prevChar = currChar;
                posIndex -= 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// 人民币大写金额
        /// </summary>
        /// <param name="value">人民币数字金额值</param>
        /// <returns>返回人民币大写金额</returns>
        public string RMBAmount(double value)
        {
            string capResult = "";
            string capValue = string.Format("{0:f4}", value);       //格式化
            int dotPos = capValue.IndexOf(".");                     //小数点位置
            bool addInt = (Convert.ToInt32(capValue.Substring(dotPos + 1)) == 0);//是否在结果中加"整"
            bool addMinus = (capValue.Substring(0, 1) == "-");      //是否在结果中加"负"
            int beginPos = addMinus ? 1 : 0;                        //开始位置
            string capInt = capValue.Substring(beginPos, dotPos);   //整数
            string capDec = capValue.Substring(dotPos + 1);         //小数

            if (dotPos > 0)
            {
                capResult = ConvertIntToUppercaseAmount(capInt) +
                    ConvertDecToUppercaseAmount(capDec, Convert.ToDouble(capInt) != 0 ? true : false);
            }
            else
            {
                capResult = ConvertIntToUppercaseAmount(capDec);
            }
            if (addMinus) capResult = "负" + capResult;
            if (addInt) capResult += "整";
            return capResult;
        }
        #endregion

        #region 根据URL获取源文件内容
        /// <summary>
        /// 根据Url获得源文件内容（20秒超时）
        /// </summary>
        /// <param name="url">合法的Url地址</param>
        /// <returns></returns>
        public static string GetSourceTextByUrl(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 30000;//20秒超时
            WebResponse response = request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            return sr.ReadToEnd();
        }
        #endregion

        #region 获取当前文件的扩展名
        /// <summary>
        /// 获取指定文件的扩展名
        /// </summary>
        /// <param name="fileName">指定文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOf('.') <= 0)
                return "";

            fileName = fileName.ToLower().Trim();

            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }
        #endregion

        #region Web请求(含Client）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geturl"></param>
        /// <param name="isException"></param>
        /// <returns></returns>
        public static string GetRequest(string geturl, bool isException = true)
        {
            HttpWebRequest myReq = null;//创建httprequest
            HttpWebResponse response = null; //创建httpresponse
            try
            {
                myReq = (HttpWebRequest)WebRequest.Create(geturl);
                myReq.ContentType = "text/html;charset=UTF-8";
                myReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                response = (HttpWebResponse)myReq.GetResponse();
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                string exStr = ex.ToString();
                if (isException)
                {
                    List<string> info = new List<string>();
                    info.Add("Error：" + exStr);
                    info.Add("PostURL：" + geturl);
                    WriteLog.WriteRecord(info);
                }
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode errorCode = ((HttpWebResponse)ex.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)ex.Response).StatusDescription;
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        throw new Exception(sr.ReadToEnd());
                    }
                }
                else
                {
                    return exStr;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (myReq != null)
                {
                    myReq.Abort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geturl"></param>
        /// <param name="isException"></param>
        /// <param name="isDispose"></param>
        /// <returns></returns>
        public static async Task<string> GetRequest(string geturl, bool isException = true, bool isDispose = true)
        {
            HttpClient client = null;
            try
            {
                client = new HttpClient();
                return await client.GetStringAsync(geturl);
            }
            catch (WebException ex)
            {
                string exStr = ex.ToString();
                if (isException)
                {
                    List<string> info = new List<string>();
                    info.Add("Error：" + exStr);
                    info.Add("PostURL：" + geturl);
                    WriteLog.WriteRecord(info);
                }
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode errorCode = ((HttpWebResponse)ex.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)ex.Response).StatusDescription;
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        throw new Exception(sr.ReadToEnd());
                    }
                }
                else
                {
                    return exStr;
                }
            }
            finally
            {
                if (isDispose)
                {
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posturl">请求的链接地址</param>
        /// <param name="parm">请求的参数</param>
        /// <param name="isException">是否记录当前操作失败请求，默认记录</param>
        /// <returns></returns>
        public static string PostRequest(string posturl, List<KeyValuePair<string, string>> parm, bool isException = true)
        {
            HttpWebRequest myReq = null;//创建httprequest
            HttpWebResponse response = null; //创建httpresponse
            string _parm = String.Empty;
            parm.ForEach(item => { _parm += item.Key + "=" + item.Value + "&"; });
            _parm = _parm.Substring(0, _parm.Length - 1);
            try
            {
                myReq = (HttpWebRequest)WebRequest.Create(posturl);//利用工厂机制（factory mechanism）通过Create()方法来创建的
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";
                byte[] bs = UTF8Encoding.UTF8.GetBytes(_parm);
                myReq.ContentLength = bs.Length;
                using (Stream reqStream = myReq.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }
                response = (HttpWebResponse)myReq.GetResponse();
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException e)
            {
                string exStr = e.ToString();
                if (isException)
                {
                    List<string> info = new List<string>();
                    info.Add("Error：" + exStr);
                    info.Add("PostURL：" + posturl);
                    info.Add("ParmStr：" + _parm);
                    WriteLog.WriteRecord(info);
                }
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode errorCode = ((HttpWebResponse)e.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)e.Response).StatusDescription;
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)e.Response).GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        throw new Exception(sr.ReadToEnd());
                    }
                }
                return exStr;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (myReq != null)
                {
                    myReq.Abort();
                }
            }
        }

        public static string PostRequest(string posturl, string parm)
        {
            HttpWebRequest myReq = null;
            HttpWebResponse response = null;
            try
            {
                myReq = (HttpWebRequest)WebRequest.Create(posturl);
                myReq.Method = "POST";
                myReq.ContentType = "application/json";
                //utf8编码
                byte[] bs = UTF8Encoding.UTF8.GetBytes(parm);
                myReq.ContentLength = bs.Length;
                using (Stream reqStream = myReq.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }

                response = (HttpWebResponse)myReq.GetResponse();
                HttpStatusCode statusCode = response.StatusCode;
                if (Equals(response.StatusCode, HttpStatusCode.OK))
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            //异常处理
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode errorCode = ((HttpWebResponse)e.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)e.Response).StatusDescription;
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)e.Response).GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        return sr.ReadToEnd();
                    }
                }
                Tools.WriteLog.WriteRecord("Base/Push:", e.ToString());
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (myReq != null)
                {
                    myReq.Abort();
                }
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="parm"></param>
        /// <param name="isException">是否记录错误日志（默认记录）</param>
        /// <param name="isDispose">是否释放当前链接（默认释放）</param>
        /// <returns></returns>
        public static async Task<string> PostRequest(string posturl, List<KeyValuePair<string, string>> parm, bool isException = true, bool isDispose = true)
        {
            HttpClient client = null;
            try
            {
                client = new HttpClient();
                var content = new FormUrlEncodedContent(parm);
                var response = await client.PostAsync(posturl, content);
                return await response.Content.ReadAsStringAsync();
            }
            catch (WebException ex)
            {
                string exStr = ex.ToString();
                if (isException)
                {
                    List<string> info = new List<string>();
                    info.Add("Error：" + exStr);
                    info.Add("PostURL：" + posturl);
                    string _parm = String.Empty;
                    parm.ForEach(item => { _parm += item.Key + "=" + item.Value + "&"; });
                    info.Add("ParmStr：" + _parm.Substring(0, _parm.Length - 1));
                    WriteLog.WriteRecord(info);
                }
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode errorCode = ((HttpWebResponse)ex.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)ex.Response).StatusDescription;
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        throw new Exception(sr.ReadToEnd());
                    }
                }
                else
                {
                    return exStr;
                }
            }
            finally
            {
                if (isDispose)
                {
                    client.Dispose();
                }
            }
        }
        #endregion

        #region 二级制、文件、string互相转化
        /// <summary>
        /// 将文件转换成二进制
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static byte[] FileToBinary(string path)
        {
            FileStream stream = new FileInfo(path).OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            return buffer;
        }

        /// <summary>
        /// 二进制流转图片
        /// </summary>
        /// <param name="streambyte">二进制流</param>
        /// <returns>图片</returns>
        public static System.Drawing.Image BinaryToImage(byte[] streambyte)
        {
            MemoryStream ms = new MemoryStream(streambyte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="imagepath">图片地址</param>
        /// <returns>二进制</returns>
        public static byte[] ImageToBinary(string imagepath)
        {
            //根据图片文件的路径使用文件流打开，并保存为byte[]
            FileStream fs = new FileStream(imagepath, FileMode.Open);//可以是其他重载方法
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <returns>二进制</returns>
        public static byte[] ImageToBinary(System.Drawing.Image img)
        {
            MemoryStream mstream = new MemoryStream();//将Image转换成流数据，并保存为byte[]
            img.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }
        #endregion
    }
}
