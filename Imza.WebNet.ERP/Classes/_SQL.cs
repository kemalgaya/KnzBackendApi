using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Imza.WebTools.Classes;

namespace Imza.WebNet.Erp.Classes
{

    public static class _SQL
    {
        private static string DataSourcedefault = "";
        private static string UserIdDefault = "sa";
        private static string InitialCatalogDefault = "IMZANETKASERVER";
        private static string PasswordDefault = "1+";

        public static string CheckLogin(string pUsername, string pPassword)
        {

            if (string.IsNullOrEmpty(_SQL.DataSourcedefault))
                _SQL.setConfig();
            if (!string.IsNullOrEmpty(pPassword))
                pPassword = iTools.Cryptation.Crypt(pPassword);
            if (_SQL.GetDataTable(string.Format("SELECT USERID,USERNAME,NAME,SURNAME,COMPANYID FROM dbo.AUTH_USERS WHERE USERNAME = '{0}' AND PASSWORD = '{1}'", (object)pUsername, (object)pPassword)).Rows.Count != 1)
                return "";
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddHours(8.0);
            return iTools.Cryptation.Crypt(dateTime.ToString());
        }

        private static string setConfig()
        {
            try
            {
                
                StreamReader streamReader = File.OpenText(HttpContext.Current.Server.MapPath("~/App_Data/Config.txt"));
                string str = iTools.Cryptation.DeCrypt(streamReader.ReadToEnd(), "49120570aisehifa20815287");
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strArray = str.Split('>');
                    if (strArray.Length != 0)
                    {
                        if (!string.IsNullOrEmpty(strArray[2].Replace("\n", "").Replace("\r", "")))
                            _SQL.ServerIp = strArray[2].Replace("\n", "").Replace("\r", "");
                        if (!string.IsNullOrEmpty(strArray[4].Replace("\n", "").Replace("\r", "")))
                            _SQL.InstanceName = Utility.Nvl((object)strArray[4].Replace("\n", "").Replace("\r",""));
                        if (!string.IsNullOrEmpty(_SQL.ServerIp))
                        {
                            if (!string.IsNullOrEmpty(_SQL.InstanceName))
                                _SQL.DataSourcedefault = string.Format("{0}\\{1}", (object)_SQL.ServerIp, (object)_SQL.InstanceName);
                            else if (string.IsNullOrEmpty(_SQL.InstanceName) || _SQL.InstanceName == "YOK")
                                _SQL.DataSourcedefault = string.Format("{0}", (object)_SQL.ServerIp);
                        }
                        if (!string.IsNullOrEmpty(strArray[6].Replace("\n", "").Replace("\r", "")))
                            _SQL.InitialCatalogDefault = strArray[6].Replace("\n", "").Replace("\r", "");
                        if (!string.IsNullOrEmpty(strArray[8].Replace("\n", "").Replace("\r", "")))
                            _SQL.UserIdDefault = strArray[8].Replace("\n", "").Replace("\r", "");
                        if (!string.IsNullOrEmpty(strArray[10].Replace("\n", "").Replace("\r", "")))
                            _SQL.PasswordDefault = strArray[10].Replace("\n", "").Replace("\r", "");
                    }
                }
                streamReader.Close();
            }
            catch (Exception ex)
            {
                return string.Format("Hata oluştu. Program açılmayacaktır!\nConfig.txt dosyasının olup olmadığını kontrol ediniz!\n\nHata Detayı:{0}", (object)ex.Message);
            }
            return "";
        }

        public static string InstanceName { get; set; }

        public static string ServerIp { get; set; }

        public static string ExecuteScalar(
          string pSql = "",
          string DataSource = "",
          string InitialCatalog = "",
          string UserId = "",
          string Password = "")
        {
            if (string.IsNullOrEmpty(_SQL.DataSourcedefault))
                _SQL.setConfig();
            if (string.IsNullOrEmpty(DataSource))
            {
                DataSource = _SQL.DataSourcedefault;
                InitialCatalog = _SQL.InitialCatalogDefault;
                UserId = _SQL.UserIdDefault;
                Password = _SQL.PasswordDefault;
            }
            SqlConnection connection = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1}; User Id = {2}; Password = {3};", (object)DataSource, (object)InitialCatalog, (object)UserId, (object)Password));
            connection.Open();
            string str = Convert.ToString(new SqlCommand(pSql, connection).ExecuteScalar());
            connection.Close();
            return str;
        }

        public static string ExecuteNonQuery(
          string pSql = "",
          string DataSource = "",
          string InitialCatalog = "",
          string UserId = "",
          string Password = "")
        {
            if (string.IsNullOrEmpty(_SQL.DataSourcedefault))
                _SQL.setConfig();
            if (string.IsNullOrEmpty(DataSource))
            {
                DataSource = _SQL.DataSourcedefault;
                InitialCatalog = _SQL.InitialCatalogDefault;
                UserId = _SQL.UserIdDefault;
                Password = _SQL.PasswordDefault;
            }
            SqlConnection connection = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1}; User Id = {2}; Password = {3};", (object)DataSource, (object)InitialCatalog, (object)UserId, (object)Password));
            connection.Open();
            string str = Convert.ToString(new SqlCommand(pSql, connection).ExecuteNonQuery());
            connection.Close();
            return str;
        }

        public static DataTable GetDataTable(
          string pSql = "",
          string DataSource = "",
          string InitialCatalog = "",
          string UserId = "",
          string Password = "")
        {
            if (string.IsNullOrEmpty(_SQL.DataSourcedefault))
                _SQL.setConfig();
            if (string.IsNullOrEmpty(DataSource))
            {
                DataSource = _SQL.DataSourcedefault;
                InitialCatalog = _SQL.InitialCatalogDefault;
                UserId = _SQL.UserIdDefault;
                Password = _SQL.PasswordDefault;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1}; User Id = {2}; Password = {3};", (object)DataSource, (object)InitialCatalog, (object)UserId, (object)Password)))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(pSql, connection))
                    {
                        connection.Open();
                        DataTable dataTable = new DataTable();
                        dataTable.Load((IDataReader)sqlCommand.ExecuteReader());
                        connection.Close();
                        return dataTable;
                    }
                }
            }
            catch
            {
                return (DataTable)null;
            }
        }

        public static DataTable GetDatatableTimeOut(
          string pSql = "",
          int pTimeOut = 9999999,
          string DataSource = "",
          string InitialCatalog = "",
          string UserId = "",
          string Password = "")
        {
            if (string.IsNullOrEmpty(_SQL.DataSourcedefault))
                _SQL.setConfig();
            if (string.IsNullOrEmpty(DataSource))
            {
                DataSource = _SQL.DataSourcedefault;
                InitialCatalog = _SQL.InitialCatalogDefault;
                UserId = _SQL.UserIdDefault;
                Password = _SQL.PasswordDefault;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1}; User Id = {2}; Password = {3};", (object)DataSource, (object)InitialCatalog, (object)UserId, (object)Password)))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(pSql, connection))
                    {
                        sqlCommand.CommandTimeout = pTimeOut;
                        connection.Open();
                        DataTable datatableTimeOut = new DataTable();
                        datatableTimeOut.Load((IDataReader)sqlCommand.ExecuteReader());
                        connection.Close();
                        return datatableTimeOut;
                    }
                }
            }
            catch
            {
                return (DataTable)null;
            }
        }

        [Description("Tablo adı ve parametre dizesi vererek insert işleminin yapılmasını sağlar.")]
        public static object ExecuteTableInsert(
          string pTableName,
          dbStruct.StDbParamList pParams,
          bool pInsertBaseInfo = true,
          bool pShowSqlScript = false)
        {
            if (pInsertBaseInfo)
            {
                pParams.Add((object)"COMPANYID", (object)1);
                pParams.Add((object)"STATE", (object)1);
            }
            try
            {
                string str1 = "";
                string str2 = "";
                for (int index = 0; index < pParams.Count; ++index)
                {
                    if (!pParams.ParamList[index].DateType)
                    {
                        str1 = str1 + pParams.ParamList[index].ParamName + ",";
                        str2 = str2 + "'" + pParams.ParamList[index].ParamValue.Replace("'", "''") + "',";
                    }
                    else if (pParams.ParamList[index].ParamValue.IndexOf("getdate()", StringComparison.Ordinal) > -1)
                    {
                        str1 = str1 + pParams.ParamList[index].ParamName + ",";
                        str2 = str2 + "convert(datetime," + pParams.ParamList[index].ParamValue.Replace("'", "''") + ") ,";
                    }
                    else
                    {
                        str1 = str1 + pParams.ParamList[index].ParamName + ",";
                        str2 = str2 + "convert(datetime,'" + pParams.ParamList[index].ParamValue.Replace("'", "''") + "') ,";
                    }
                }
                string str3 = str1.Remove(str1.Length - 1, 1);
                string str4 = str2.Remove(str2.Length - 1, 1);
                string pSql = ("SET DATEFORMAT DMY INSERT INTO " + pTableName + "(" + str3 + ")" + " VALUES (" + str4 + ")").Replace("'GETDATE()'", "GETDATE()");
                return pShowSqlScript ? (object)pSql : (object)_SQL.ExecuteNonQuery(pSql);
            }
            catch (Exception ex)
            {
                return (object)("ExecuteSql " + ex.Message + ex.StackTrace);
            }
        }

        public static object ExecuteTableUpdate(
          string pTableName,
          dbStruct.StDbParamList pParams,
          string pWhere_Basinda_AND_Olmayacak,
          bool pShowSqlScript = false)
        {
            try
            {
                string str1 = "";
                for (int index = 0; index < pParams.Count; ++index)
                {
                    if (pParams.ParamList[index].ParamValue == "true" || pParams.ParamList[index].ParamValue == "false")
                        str1 = !(pParams.ParamList[index].ParamValue == "true") ? str1 + pParams.ParamList[index].ParamName + "='0'," : str1 + pParams.ParamList[index].ParamName + "='1',";
                    else
                        str1 = str1 + pParams.ParamList[index].ParamName + "='" + pParams.ParamList[index].ParamValue + "',";
                }
                string str2 = str1.Remove(str1.Length - 1, 1);
                string pSql = ("set dateformat dmy UPDATE  " + pTableName + " SET " + str2 + " WHERE " + (pWhere_Basinda_AND_Olmayacak.Length > 0 ? pWhere_Basinda_AND_Olmayacak : " 1=1 ")).Replace("'SYSDATE'", "SYSDATE");
                return pShowSqlScript ? (object)pSql : (object)_SQL.ExecuteNonQuery(pSql);
            }
            catch (Exception ex)
            {
               var x= "ExecuteSql \n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + ex.StackTrace;
                return (object)-1;
            }
        }
    }
}
 