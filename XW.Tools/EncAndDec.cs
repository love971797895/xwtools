using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace XW.Tools
{
    /// <summary>
    /// Encrypt Dectrypt
    /// </summary>
    public class EncAndDec
    {

        private static string Key = ConfigurationManager.AppSettings["DESSecretKey"];
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//默认密钥向量

        #region MD5
        /// <summary>
        /// 大写MD5
        /// </summary>
        /// <param name="str">需要转化的字符</param>
        /// <returns></returns>
        public static string Md532Upper(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            //创建一个Md5对象
            MD5 md5 = new MD5CryptoServiceProvider();
            //加密Byte[]数组
            byte[] result = md5.ComputeHash(data);
            return byteToHexStr(result);
        }

        /// <summary>
        /// 小写MD5
        /// </summary>
        /// <param name="str">需要转化的字符串</param>
        /// <returns></returns>
        public static string Md532Lower(string str)
        {
            return Md532Upper(str).ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// MD5 16位加密 加密后密码为大写
        /// </summary>
        /// <param name="str">需要加密的字符</param>
        /// <returns></returns>
        public static string Md516Upper(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD516Lower(string str)
        {
            return Md516Upper(str).ToLower();
        }
        #endregion

        #region SHA1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }
        #endregion

        #region DES
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString)
        {
            Key = GetSubString(Key, 0, 8, "");
            Key = Key.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString)
        {
            try
            {
                Key = GetSubString(Key, 0, 8, "");
                Key = Key.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV),
                    CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = GetSubString(encryptKey, 0, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = GetSubString(decryptKey, 0, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV),
                    CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        private static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;
            if(string.IsNullOrEmpty(p_SrcString))
            {
                p_SrcString = "xwtools";
            }
            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex, ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        #endregion

        #region RSA
        #endregion

        #region RC2
        /// <summary>
        /// RC2解密
        /// </summary>
        /// <param name="dencryptstr"></param>
        /// <returns></returns>
        public static string DencryptRC2(string dencryptstr)
        {
            try
            {
                RC2CryptoServiceProvider provider2 = new RC2CryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(Key),
                    IV = Encoding.Default.GetBytes(Key)
                };
                RC2CryptoServiceProvider provider = provider2;
                byte[] buffer = Convert.FromBase64String(dencryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// RC2解密
        /// </summary>
        /// <param name="dencryptstr">加密的字符串</param>
        /// <param name="denkey">加密的密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DencryptRC2(string dencryptstr, string denkey)
        {
            try
            {
                RC2CryptoServiceProvider provider2 = new RC2CryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(denkey),
                    IV = Encoding.Default.GetBytes(denkey)
                };
                RC2CryptoServiceProvider provider = provider2;
                byte[] buffer = Convert.FromBase64String(dencryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用RC2方式进行加密
        /// </summary>
        /// <param name="encryptstr"></param>
        /// <returns></returns>
        public static string EncryptRC2(string encryptstr)
        {
            try
            {
                RC2CryptoServiceProvider provider2 = new RC2CryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(Key),
                    IV = Encoding.Default.GetBytes(Key)
                };
                RC2CryptoServiceProvider provider = provider2;
                byte[] bytes = Encoding.Default.GetBytes(encryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用RC2方式进行加密
        /// </summary>
        /// <param name="encryptstr">需要加密的字符串</param>
        /// <param name="enckey">加密用的密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptRC2(string encryptstr, string enckey)
        {
            try
            {
                RC2CryptoServiceProvider provider2 = new RC2CryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(enckey),
                    IV = Encoding.Default.GetBytes(enckey)
                };
                RC2CryptoServiceProvider provider = provider2;
                byte[] bytes = Encoding.Default.GetBytes(encryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region AES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptstr"></param>
        /// <returns></returns>
        public static string EncryptAES(string encryptstr)
        {
            try
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = Encoding.Default.GetBytes(Key);
                rijndael.IV = Encoding.Default.GetBytes(Key.Substring(0, 0x10));
                byte[] bytes = Encoding.Default.GetBytes(encryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用AES方式进行加密
        /// </summary>
        /// <param name="encryptstr">需要加密的字符串</param>
        /// <param name="enckey">加密用的密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptAES(string encryptstr, string enckey)
        {
            try
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = Encoding.Default.GetBytes(enckey);
                rijndael.IV = Encoding.Default.GetBytes(enckey.Substring(0, 0x10));
                byte[] bytes = Encoding.Default.GetBytes(encryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dencryptstr"></param>
        /// <returns></returns>
        public static string DencryptAES(string dencryptstr)
        {
            try
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = Encoding.Default.GetBytes(Key);
                rijndael.IV = Encoding.Default.GetBytes(Key.Substring(0, 0x10));
                byte[] buffer = Convert.FromBase64String(dencryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解密由AES加密的字符串
        /// </summary>
        /// <param name="dencryptstr">加密字符串</param>
        /// <param name="denkey">加密密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DencryptAES(string dencryptstr, string denkey)
        {
            try
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = Encoding.Default.GetBytes(denkey);
                rijndael.IV = Encoding.Default.GetBytes(denkey.Substring(0, 0x10));
                byte[] buffer = Convert.FromBase64String(dencryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region TripleDES
        /// <summary>
        /// 解密由TripleDES加密的字符串
        /// </summary>
        /// <param name="dencryptstr"></param>
        /// <returns></returns>
        public static string DencryptTripleDES(string dencryptstr)
        {
            try
            {
                TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(Key),
                    IV = Encoding.Default.GetBytes(Key.Substring(0, 8))
                };
                TripleDESCryptoServiceProvider provider = provider2;
                byte[] buffer = Convert.FromBase64String(dencryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解密由TripleDES加密的字符串
        /// </summary>
        /// <param name="dencryptstr">加密的字符串</param>
        /// <param name="denkey">加密的密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DencryptTripleDES(string dencryptstr, string denkey)
        {
            try
            {
                TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(denkey),
                    IV = Encoding.Default.GetBytes(denkey.Substring(0, 8))
                };
                TripleDESCryptoServiceProvider provider = provider2;
                byte[] buffer = Convert.FromBase64String(dencryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解密由TripleDES加密的字符串
        /// </summary>
        /// <param name="encryptstr"></param>
        /// <returns></returns>
        public static string EncryptTripleDES(string encryptstr)
        {
            try
            {
                TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(Key),
                    IV = Encoding.Default.GetBytes(Key.Substring(0, 8))
                };
                TripleDESCryptoServiceProvider provider = provider2;
                byte[] bytes = Encoding.Default.GetBytes(encryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 使用TripleDES方式进行加密
        /// </summary>
        /// <param name="encryptstr">需要加密的字符串</param>
        /// <param name="enckey">加密用的密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptTripleDES(string encryptstr, string enckey)
        {
            try
            {
                TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.Default.GetBytes(enckey),
                    IV = Encoding.Default.GetBytes(enckey.Substring(0, 8))
                };
                TripleDESCryptoServiceProvider provider = provider2;
                byte[] bytes = Encoding.Default.GetBytes(encryptstr);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
