using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XW.Tools
{
    /// <summary>
    /// SQLServerHelper
    /// </summary>
    public class SqlServerHelper
    {
        private static string connstring = ConfigurationManager.AppSettings["Dbconnstring"];

        #region Basic
        /// <summary>
        /// 执行增，删，改的方法，支持存储过程
        /// </summary>
        /// <param name="commandType">命令类型，如果是sql语句，则为CommandType.Text,否则为CommandType.StoredProcdure</param>
        /// <param name="sql">SQL语句或者存储过程名称</param>
        /// <param name="para">SQL参数，如果没有参数，则为null</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(CommandType commandType, string sql, params SqlParameter[] para)
        {
            int rtnNum = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    if (para != null)
                    {
                        foreach (SqlParameter sp in para)
                        {
                            cmd.Parameters.Add(sp);
                        }
                    }
                    rtnNum = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog(ex);
                rtnNum = 0;
            }
            return rtnNum;
        }
        /// <summary>
        /// 执行查询的方法，支持存储过程
        /// </summary>
        /// <param name="commandType">命令类型，如果是sql语句，则为CommandType.Text,否则为CommandType.StoredProcdure</param>
        /// <param name="sql">SQL语句或者存储过程名称</param>
        /// <param name="para">SQL参数，如果没有参数，则为null</param>
        /// <returns>读取器SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(CommandType commandType, string sql, params SqlParameter[] para)
        {

            SqlDataReader dr = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    if (para != null)
                    {
                        foreach (SqlParameter sp in para)
                        {
                            cmd.Parameters.Add(sp);
                        }
                    }
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog(ex);
            }
            return dr;
        }

        /// <summary>
        /// 执行查询的方法，支持存储过程
        /// </summary>
        /// <param name="commandType">命令类型，如果是sql语句，则为CommandType.Text,否则为CommandType.StoredProcdure</param>
        /// <param name="sql">SQL语句或者存储过程名称</param>
        /// <param name="para">SQL参数，如果没有参数，则为null</param>
        /// <returns>数据集</returns>
        public static DataSet GetDataSet(CommandType commandType, string sql, params SqlParameter[] para)
        {
            DataSet ds = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.Connection = conn;
                    da.SelectCommand.CommandText = sql;
                    da.SelectCommand.CommandType = commandType;
                    da.SelectCommand.CommandTimeout = 0;
                    if (para != null)
                    {
                        foreach (SqlParameter sp in para)
                        {
                            da.SelectCommand.Parameters.Add(sp);
                        }
                    }
                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog(ex);
            }
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(CommandType commandType, string sql, params SqlParameter[] para)
        {
            DataTable table = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.Connection = conn;
                    da.SelectCommand.CommandText = sql;
                    da.SelectCommand.CommandType = commandType;
                    da.SelectCommand.CommandTimeout = 0;
                    if (para != null)
                    {
                        foreach (SqlParameter sp in para)
                        {
                            da.SelectCommand.Parameters.Add(sp);
                        }
                    }
                    table = new DataTable();
                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog(ex);
            }
            return table;
        }

        /// <summary>
        /// 执行查询单个值的方法，支持存储过程
        /// </summary>
        /// <param name="commandType">命令类型，如果是sql语句，则为CommandType.Text,否则为CommandType.StoredProcdure</param>
        /// <param name="sql">SQL语句或者存储过程名称</param>
        /// <param name="para">SQL参数，如果没有参数，则为null</param>
        /// <returns>单个值</returns>
        public static object ExecuteScalar(CommandType commandType, string sql, params SqlParameter[] para)
        {
            object obj = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    if (para != null)
                    {
                        foreach (SqlParameter sp in para)
                        {
                            cmd.Parameters.Add(sp);
                        }
                    }
                    obj = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog(ex);
            }
            return obj;
        }

        /// <summary>
        /// 执行增，删，改的方法，支持存储过程
        /// </summary>
        /// <param name="commandType">命令类型，如果是sql语句，则为CommandType.Text,否则为CommandType.StoredProcdure</param>
        /// <param name="sql">SQL语句或者存储过程名称</param>
        /// <param name="para">SQL参数，如果没有参数，则为null</param>
        /// <returns>受影响的行数</returns>
        public static object ExecuteNonQueryObj(CommandType commandType, string sql, params SqlParameter[] para)
        {
            object obj = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    if (para != null)
                    {
                        foreach (SqlParameter sp in para)
                        {
                            cmd.Parameters.Add(sp);
                        }
                        cmd.Parameters[2].Direction = ParameterDirection.Output;
                    }
                    cmd.ExecuteNonQuery();
                    obj = cmd.Parameters[2].Value;
                }
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog(ex);
            }
            return obj;
        }
        #endregion

        #region Transaction
        /// <summary>
        /// 直接执行SQL语句的事务
        /// </summary>
        /// <param name="Sqlstr"></param>
        /// <returns></returns>
        public static int ExecTransaction(string[] Sqlstr)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                try
                {
                    if (Sqlstr.Length > 0)
                    {
                        for (int i = 0; i < Sqlstr.Length; i++)
                        {
                            cmd.CommandText = Sqlstr[i];
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.CreateLog(ex);
                    tran.Rollback();
                    return 0;
                }
                finally
                {
                    tran.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行带参数事务,请避免参数重复
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static int ExecTransaction(List<KeyValuePair<string, List<SqlParameter>>> strList)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                try
                {
                    if (strList.Count > 0)
                    {
                        for (int i = 0; i < strList.Count; i++)
                        {
                            cmd.CommandText = strList[i].Key;
                            if (strList[i].Value != null)
                            {
                                foreach (SqlParameter sp in strList[i].Value)
                                {
                                    cmd.Parameters.Add(sp);
                                }
                            }
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.CreateLog(ex);
                    tran.Rollback();
                    return 0;
                }
                finally
                {
                    tran.Dispose();
                }
            }
        }
        #endregion

        #region Paged query
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parimarykey"></param>
        /// <param name="lieName"></param>
        /// <param name="orderType"></param>
        /// <param name="orderByLie"></param>
        /// <returns></returns>
        public static DataTable GetListByPage(string tableName, string strWhere, int pageIndex, int pageSize, string parimarykey, string lieName = null, int orderType = 0, string orderByLie = null)
        {
            StringBuilder sbBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(parimarykey))
            {
                sbBuilder.Append(" SELECT TOP ").Append(pageSize).Append(" ");
                if (!string.IsNullOrEmpty(lieName))
                {
                    sbBuilder.Append(lieName).Append(" ");
                }
                else
                {
                    sbBuilder.Append("* ");
                }
                sbBuilder.Append("FROM ").Append(tableName);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    sbBuilder.Append(" WHERE ").Append(strWhere).Append(" AND ").Append(parimarykey);
                }
                else
                {
                    sbBuilder.Append(" WHERE ").Append(parimarykey);
                }
                sbBuilder.Append(" NOT IN (SELECT TOP ").Append(pageSize * pageIndex);
                sbBuilder.Append(" ").Append(parimarykey).Append(" FROM ").Append(tableName).Append(" ");
                if (!string.IsNullOrEmpty(strWhere))
                {
                    sbBuilder.Append(" WHERE ").Append(strWhere);
                }
                sbBuilder.Append(" ORDER BY ");
                if (!string.IsNullOrEmpty(orderByLie))
                {
                    sbBuilder.Append(orderByLie).Append(" ");
                }
                else
                {
                    sbBuilder.Append(parimarykey).Append(" ");
                }
                if (orderType == 0)
                {
                    sbBuilder.Append("ASC");
                }
                else
                {
                    sbBuilder.Append("DESC");
                }
                sbBuilder.Append(" ) ORDER BY ");
                if (!string.IsNullOrEmpty(orderByLie))
                {
                    sbBuilder.Append(orderByLie).Append(" ");
                }
                else
                {
                    sbBuilder.Append(parimarykey).Append(" ");
                }
                if (orderType == 0)
                {
                    sbBuilder.Append("ASC");
                }
                else
                {
                    sbBuilder.Append("DESC");
                }
                return GetDataTable(CommandType.Text, sbBuilder.ToString());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 仅支持sqlserver2005及以上
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parimarykey"></param>
        /// <param name="lieName"></param>
        /// <param name="orderType"></param>
        /// <param name="orderByLie"></param>
        /// <returns></returns>
        public static DataTable GetListByPage2(string tableName, string strWhere, int pageIndex, int pageSize, string parimarykey, string lieName = null, int orderType = 0, string orderByLie = null)
        {
            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(parimarykey))
            {
                StringBuilder sbBuilder = new StringBuilder();
                sbBuilder.Append(" SELECT ");
                if (!string.IsNullOrEmpty(lieName))
                {
                    sbBuilder.Append(lieName).Append(" ");
                }
                else
                {
                    sbBuilder.Append("* ");
                }
                sbBuilder.Append(" FROM (SELECT ROW_NUMBER() OVER(");
                if (!string.IsNullOrEmpty(orderByLie))
                {
                    sbBuilder.Append(" ORDER BY T." + orderByLie);
                }
                else
                {
                    sbBuilder.Append("ORDER BY ").Append("T.").Append(parimarykey);
                }
                if (orderType == 0)
                {
                    sbBuilder.Append(" ASC");
                }
                else
                {
                    sbBuilder.Append(" DESC");
                }
                sbBuilder.Append(") AS Row,");
                if (!string.IsNullOrEmpty(lieName))
                {
                    sbBuilder.Append(" T.").Append(lieName).Append(" ");
                }
                else
                {
                    sbBuilder.Append(" T.* ");
                }
                sbBuilder.Append(" FROM ").Append(tableName).Append(" T ");
                if (!string.IsNullOrEmpty(strWhere))
                {
                    sbBuilder.Append(" WHERE ").Append(strWhere);
                }
                sbBuilder.Append(" ) TT");
                int startIndex = (pageSize * pageIndex > 0) ? (pageSize * pageIndex) + 1 : 1;
                sbBuilder.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1} ", startIndex, pageSize * (pageIndex + 1));
                return GetDataTable(CommandType.Text, sbBuilder.ToString());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 仅支持sqlserver2012及以上
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parimaryKey"></param>
        /// <param name="lieName"></param>
        /// <param name="orderType"></param>
        /// <param name="orderByLie"></param>
        /// <returns></returns>
        public static DataTable GetListByPage3(string tableName, string strWhere, int pageIndex, int pageSize, string parimaryKey, string lieName = null, int orderType = 0, string orderByLie = null)
        {
            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(parimaryKey))
            {
                StringBuilder sbBuilder = new StringBuilder();
                sbBuilder.Append(" SELECT ");
                if (!string.IsNullOrEmpty(lieName))
                {
                    sbBuilder.Append(lieName).Append(" ");
                }
                else
                {
                    sbBuilder.Append("* ");
                }
                sbBuilder.Append("FROM ").Append(tableName).Append(" ORDER BY ");
                if (!string.IsNullOrEmpty(orderByLie))
                {
                    sbBuilder.Append(orderByLie);
                }
                else
                {
                    sbBuilder.Append(parimaryKey);
                }
                if (orderType == 0)
                {
                    sbBuilder.Append(" ASC ");
                }
                else
                {
                    sbBuilder.Append(" DESC ");
                }
                sbBuilder.Append("OFFSET ").Append(pageSize * pageIndex).Append(" ROWS ").Append(" FETCH NEXT ").Append(pageSize).Append(" ROWS ONLY ");
                return GetDataTable(CommandType.Text, sbBuilder.ToString());
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Generic query

        /// <summary>
        /// 通过泛型插入数据
        /// </summary>
        /// <typeparam name="T">类名称</typeparam>
        /// <param name="obj">类对象,如果要插入空值，请使用@NULL</param>
        /// <returns>插入的新记录ID</returns>
        public static int Insert<T>(T obj)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL = GetInsertSQL(obj);
            object result = ExecuteScalar(CommandType.Text, strSQL.ToString(), null);// 插入到数据库中
            return Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
        }

        /// <summary>
        /// 获取实体的插入语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">实体对象</param>
        /// <returns>返回插入语句</returns>
        private static StringBuilder GetInsertSQL<T>(T obj)
        {
            var type = obj.GetType();//获得该类的Type
            string tableKey = type.GetProperties().FirstOrDefault().Name;
            string tableName = type.Name;
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("insert into " + tableName + "(");
            string fields = "";
            string values = "";
            foreach (PropertyInfo pi in type.GetProperties())//再用Type.GetProperties获得PropertyInfo[]
            {
                object name = pi.Name;//用pi.GetValue获得值
                // 替换Sql注入符
                string value1 = Convert.ToString(pi.GetValue(obj, null)).Replace("'", "''");
                string properName = name.ToString().ToLower();
                if (!string.IsNullOrEmpty(value1) && properName != tableKey.ToLower() && properName != DB.BaseSet.PrimaryKey.ToLower() && properName != DB.BaseSet.TableName.ToLower() && value1 != DB.BaseSet.DateTimeLongNull && value1 != DB.BaseSet.DateTimeShortNull)
                {
                    if (value1 == DB.BaseSet.NULL)  // 判断是否为空
                    {
                        value1 = "";
                    }
                    fields += Convert.ToString(name) + ",";
                    values += "'" + value1 + "',";
                }
            }
            // 去掉最后一个,
            fields = fields.TrimEnd(',');
            values = values.TrimEnd(',');
            // 拼接Sql串
            strSQL.Append(fields);
            strSQL.Append(") values (");
            strSQL.Append(values);
            strSQL.Append(")");
            strSQL.Append(";SELECT @@IDENTITY;");
            return strSQL;
        }

        /// <summary>
        /// 通过泛型更新数据
        /// </summary>
        /// <typeparam name="T">类名称</typeparam>
        /// <param name="obj">类对象,如果要更新空值，请使用@NULL</param>
        /// <returns>更新结果,大于0为更新成功</returns>
        public static int Update<T>(T obj)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL = GetUpdateSQL(obj);
            if (String.IsNullOrEmpty(strSQL.ToString()))
            {
                return 0;
            }
            // 更新到数据库中
            object result = ExecuteNonQuery(CommandType.Text, strSQL.ToString(), null);
            return Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
        }

        /// <summary>
        /// 获取实体的更新SQL串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">实体对象</param>
        /// <returns>返回插入语句</returns>
        private static StringBuilder GetUpdateSQL<T>(T obj)
        {
            //var prop = obj.GetType().GetProperties();
            //string tableKey = obj.GetType().GetProperties().FirstOrDefault().Name;//ParimaryKey
            //string keyValue = obj.GetType().GetProperties().GetValue(0).ToString();
            string condition = String.Empty; string keyValue = string.Empty; string tableKey = string.Empty;
            StringBuilder strSQL = new StringBuilder();
            //if (string.IsNullOrEmpty(keyValue))
            //{
            //    return strSQL;
            //}
            Type t = obj.GetType();//获得该类的Type
            var propes = t.GetProperties();
            strSQL.Append("update " + t.Name + " set ");
            string subSQL = "";
            //再用Type.GetProperties获得PropertyInfo[]
            tableKey = propes.FirstOrDefault().Name;
            keyValue = Convert.ToString(propes.FirstOrDefault().GetValue(obj, null));
            //string tableName = obj.GetType().Name;
            foreach (PropertyInfo pi in propes)
            {
                object name = pi.Name;//用pi.GetValue获得值
                // 替换Sql注入符
                string value1 = Convert.ToString(pi.GetValue(obj, null)).Replace("'", "''");
                string properName = name.ToString().ToLower();
                if (!string.IsNullOrEmpty(value1) && properName != tableKey.ToLower() && properName != DB.BaseSet.PrimaryKey.ToLower() && properName != DB.BaseSet.TableName.ToLower() && value1 != DB.BaseSet.DateTimeLongNull && value1 != DB.BaseSet.DateTimeShortNull)
                {
                    if (value1 == DB.BaseSet.NULL)// 判断是否为空
                    {
                        value1 = "";
                    }
                    subSQL += Convert.ToString(name) + "='" + value1 + "',";
                }
            }
            condition = " where " + tableKey + "='" + keyValue.Replace("'", "''") + "'";
            subSQL = subSQL.TrimEnd(','); // 去掉最后一个,
            strSQL.Append(subSQL);// 拼接上更新子句
            strSQL.Append(condition); // 加入更新条件
            return strSQL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int Delete<T>(T obj)
        {
            StringBuilder sb = new StringBuilder();
            var type = obj.GetType();
            var propes = type.GetProperties();
            sb.Append(" delete from ").Append(type.Name).Append(" where ").Append(propes.FirstOrDefault().Name).Append(" = '");
            sb.Append(Convert.ToString(propes.FirstOrDefault().GetValue(obj, null))).Append("' ");
            object result = ExecuteNonQuery(CommandType.Text, sb.ToString(), null);
            return Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete<T>(int id)
        {
            StringBuilder sb = new StringBuilder();
            T type = (T)Activator.CreateInstance(typeof(T));
            sb.Append(" delete from ").Append(type.GetType().Name).Append(" where ").Append(type.GetType().GetProperties().FirstOrDefault().Name).Append(" = ");
            sb.Append(id).Append(" ");
            object result = ExecuteNonQuery(CommandType.Text, sb.ToString(), null);
            return Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete<T>(string id)
        {
            StringBuilder sb = new StringBuilder();
            T type = (T)Activator.CreateInstance(typeof(T));
            sb.Append(" delete from ").Append(type.GetType().Name).Append(" where ").Append(type.GetType().GetProperties().FirstOrDefault().Name).Append(" = '");
            sb.Append(id).Append("' ");
            object result = ExecuteNonQuery(CommandType.Text, sb.ToString(), null);
            return Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
        }


        /// <summary>
        /// 默认主键为第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static T GetModelById<T>(string entityId)
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);//先创建这个对象
            string columns = string.Join(",", type.GetProperties().Select(o => o.Name));
            string strSql = string.Format("SELECT {0} FROM {1} WHERE " + type.GetProperties().FirstOrDefault().Name + " = '" + entityId + "' ", columns, type.Name);
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                SqlCommand command = new SqlCommand(strSql, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        if (reader[item.Name] != System.DBNull.Value)
                        {
                            item.SetValue(t, reader[item.Name]);//赋值
                        }
                    }
                    return t;
                }
                else
                {
                    return default(T);
                }
            }

        }

        /// <summary>
        /// 默认主键为第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static T GetModelById<T>(int entityId)
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);//先创建这个对象
            string columns = string.Join(",", type.GetProperties().Select(o => o.Name));
            string strSql = string.Format("SELECT {0} FROM {1} WHERE " + type.GetProperties().FirstOrDefault().Name + " = " + entityId + " ", columns, type.Name);
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                SqlCommand command = new SqlCommand(strSql, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        if (reader[item.Name] != System.DBNull.Value)
                        {
                            item.SetValue(t, reader[item.Name]);//赋值
                        }
                    }
                    return t;
                }
                else
                {
                    return default(T);
                }
            }

        }

        /// <summary>
        /// 泛型根据唯一标示和主键直接查询（须保证当前只能查询到一条数据）
        /// </summary>
        /// <typeparam name="T">需要查询的实体对象</typeparam>
        /// <param name="entityId">实体ID</param>
        /// <param name="primaryKey">键位名称</param>
        /// <returns></returns>
        public static T GetModelById<T>(string entityId, string primaryKey)
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);//先创建这个对象
            string columns = string.Join(",", type.GetProperties().Select(o => o.Name));
            string strSql = string.Format("SELECT {0} FROM {1} WHERE " + primaryKey + " = " + entityId + " ", columns, type.Name);
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                SqlCommand command = new SqlCommand(strSql, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        if (reader[item.Name] != System.DBNull.Value)
                        {
                            item.SetValue(t, reader[item.Name]);//赋值
                        }
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 泛型根据当前传输的对象和得到的对应列表
        /// </summary>
        /// <typeparam name="T">需要查询的实体</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public static List<T> GetList<T>(string strWhere = null)
            where T : new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);//先创建这个对象
            string columns = string.Join(",", type.GetProperties().Select(o => o.Name));//列名称
            string sql = string.Format("SELECT {0} FROM {1} ", columns, type.Name);
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql += " WHERE " + strWhere;
            }
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandText = sql;
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandTimeout = 0;
                DataTable table = new DataTable();
                da.Fill(table);
                if (table != null && table.Rows.Count > 0)
                {
                    list = BaseTool.TableToList<T>(table);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string strSql, string strWhere = null)
            where T:new ()
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(strSql))
            {
                return list;
            }
            string sql = strSql;
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql += " WHERE " + strWhere;
            }
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandText = sql;
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandTimeout = 0;
                DataTable table = new DataTable();
                da.Fill(table);
                if (table != null && table.Rows.Count > 0)
                {
                    list = BaseTool.TableToList<T>(table);
                }
            }
            return list;
        }

        /// <summary>
        /// 泛型分页查询
        /// </summary>
        /// <typeparam name="T">需要查询的类的名称</typeparam>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parimarykey"></param>
        /// <param name="lieName"></param>
        /// <param name="orderType"></param>
        /// <param name="orderByLie"></param>
        /// <param name="sqlserverversion"></param>
        /// <returns></returns>
        public static List<T> GetListByPage<T>(string strWhere, int pageIndex, int pageSize, string parimarykey, string lieName = null, int orderType = 0, string orderByLie = null, int sqlserverversion = 0)
            where T : new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);//先创建这个对象
            string columns = string.Join(",", type.GetProperties().Select(o => o.Name));//列名称
            var table = new DataTable();
            switch (sqlserverversion)
            {
                case 2005:
                case 2008:
                    table = GetListByPage2(type.Name, strWhere, pageIndex, pageSize, parimarykey, lieName, orderType, orderByLie);
                    break;
                case 2012:
                case 2014:
                    table = GetListByPage3(type.Name, strWhere, pageIndex, pageSize, parimarykey, lieName, orderType, orderByLie);
                    break;
                default:
                    table = GetListByPage(type.Name, strWhere, pageIndex, pageSize, parimarykey, lieName, orderType, orderByLie);
                    break;
            }
            if (table != null && table.Rows.Count > 0)
            {
                list = BaseTool.TableToList<T>(table);
            }
            return list;
        }

        #endregion
    }
}
