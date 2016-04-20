using System;
using System.Collections.Generic;
using System.Data.OleDb; 
using System.Text;
using System.Data;
using System.Configuration;

namespace BQ.Access
{
    /// <summary>
    /// OracleClient访问方式
    /// </summary>
    public class DBAccess 
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string connStr = string.Empty;

        /// <summary>
        /// 单例
        /// </summary>
        private static DBAccess instance = null;

        /// <summary>
        /// 异步控制对象
        /// </summary>
        private static object synObj = new object();

        /// <summary>
        /// 单例
        /// </summary>
        public static DBAccess Instance
        {            
            get
            {
                lock (synObj)
                {
                    connStr = ConfigurationManager.ConnectionStrings["Access"].ConnectionString;
                    if (instance == null)
                        instance = new DBAccess();                    
                    return instance;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string strSQL)
        {
            using (OleDbConnection connection = new OleDbConnection(connStr))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(strSQL, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    Log.WriteLogMessage("BQ.DBAccess", "Query(string strSQL)", ex.Message + strSQL, true);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string strSQL)
        {
            using (OleDbConnection connection = new OleDbConnection(connStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(strSQL, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        connection.Close();
                        Log.WriteLogMessage("BQ.DBAccess", "ExecuteSql(string strSQL)", ex.Message + strSQL, true);
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(IList<string> sqlList)
        {
            using (OleDbConnection connection = new OleDbConnection(connStr))
            {
                OleDbTransaction oraTran = connection.BeginTransaction();
                foreach (string strSQL in sqlList)
                {
                    using (OleDbCommand cmd = new OleDbCommand(strSQL, connection, oraTran))
                    {
                        try
                        {
                            connection.Open();
                            int rows = cmd.ExecuteNonQuery();
                        }
                        catch (System.Data.OleDb.OleDbException ex)
                        {
                            oraTran.Rollback();
                            connection.Close();
                            Log.WriteLogMessage("BQ.DBAccess", "ExecuteSql(string strSQL)", ex.Message + strSQL, true);
                            return -1;
                        }
                    }
                }
                oraTran.Commit();
            }
            return sqlList.Count;
        }        
        
    }
}
