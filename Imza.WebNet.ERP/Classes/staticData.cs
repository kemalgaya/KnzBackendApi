using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Imza.WebTools.Classes;

namespace Imza.WebNet.Erp.Classes
{
    public static class staticData
    {
        public static DataTable _PersonelDataTable;
        public static bool isAdmin;
        public static void PersonelDataTable()
        {
            if (_PersonelDataTable ==null)
            {
                _PersonelDataTable = SQL.GetDataTable($"select ADISOYADI as ADI,ID from dbo.PERSONEL_TANIM where STATE = 1");
            }
            if (_PersonelDataTable.Rows.Count==0)
            {
                _PersonelDataTable = SQL.GetDataTable($"select ADISOYADI as ADI,ID from dbo.PERSONEL_TANIM where STATE = 1");
            }
        }

        public static void FncCheckIsAdmin()
        {
            
        }
    }
}