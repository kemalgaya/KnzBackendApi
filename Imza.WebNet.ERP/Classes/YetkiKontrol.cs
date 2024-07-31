using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imza.WebNet.Erp.Classes;
using Imza.WebNet.Erp.Models.App;
using Imza.WebNet.Erp.Models.Mobil;
using Imza.WebNet.Erp.Models.TableModel;
using Imza.WebNet.ERP.Models.TableModel;
using Imza.WebTools.Classes;

namespace Imza.WebNet.ERP.Classes
{
    public class YetkiKontrol
    {
        public bool CheckAuthState(string pUSERID,string pFORMID)
        {
            var dtTable = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                 $"ISNULL(MENUFORM,0) MENUFORM," +
                 $"dbo.fncGetAUTH_STATE(ID,{pUSERID}) AUTH_STATE," +//original where state =1
                 $"dbo.fncGetAUTH_ADD(ID,{pUSERID}) AUTH_ADD," +//original where state =1
                 $"dbo.fncGetAUTH_UPDATE(ID,{pUSERID}) AUTH_UPDATE," +//original where state =1
                 $"dbo.fncGetAUTH_DELETE(ID,{pUSERID}) AUTH_DELETE FROM APP_FORMSWEB where ID={pFORMID}");//original where state =1


            var checkUser = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[0]["AUTH_STATE"], "0"));
            if (checkUser)
            {
                return true;
            }


            var dtUserGroup = _SQL.GetDataTable($"select * from AUTH_GROUPUSERSLINK where USERID={pUSERID}");

            for(int i = 0; i < dtUserGroup.Rows.Count; i++)
            {
                var dtAuthGroup = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                $"ISNULL(MENUFORM,0) MENUFORM," +
                $"dbo.fncGetAUTH_STATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_STATE," +//original where state =1
                $"dbo.fncGetAUTH_ADD_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_ADD," +//original where state =1
                $"dbo.fncGetAUTH_UPDATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_UPDATE," +//original where state =1
                $"dbo.fncGetAUTH_DELETE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_DELETE FROM APP_FORMSWEB  where ID={pFORMID}");//original where state =1

                var checkGroup = Convert.ToBoolean(Utility.Nvl(dtAuthGroup.Rows[0]["AUTH_STATE"], "0"));

                if (checkGroup)
                {
                    return true;
                }
            }
             

            return false;
        }
        public bool CheckAuthAdd(string pUSERID, string pFORMID)
        {
            var dtTable = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                 $"ISNULL(MENUFORM,0) MENUFORM," +
                 $"dbo.fncGetAUTH_STATE(ID,{pUSERID}) AUTH_STATE," +//original where state =1
                 $"dbo.fncGetAUTH_ADD(ID,{pUSERID}) AUTH_ADD," +//original where state =1
                 $"dbo.fncGetAUTH_UPDATE(ID,{pUSERID}) AUTH_UPDATE," +//original where state =1
                 $"dbo.fncGetAUTH_DELETE(ID,{pUSERID}) AUTH_DELETE FROM APP_FORMSWEB where ID={pFORMID}");//original where state =1


            var checkUser = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[0]["AUTH_ADD"], "0"));
            if (checkUser)
            {
                return true;
            }


            var dtUserGroup = _SQL.GetDataTable($"select * from AUTH_GROUPUSERSLINK where USERID={pUSERID}");

            for (int i = 0; i < dtUserGroup.Rows.Count; i++)
            {
                var dtAuthGroup = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                $"ISNULL(MENUFORM,0) MENUFORM," +
                $"dbo.fncGetAUTH_STATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_STATE," +//original where state =1
                $"dbo.fncGetAUTH_ADD_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_ADD," +//original where state =1
                $"dbo.fncGetAUTH_UPDATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_UPDATE," +//original where state =1
                $"dbo.fncGetAUTH_DELETE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_DELETE FROM APP_FORMSWEB  where ID={pFORMID}");//original where state =1

                var checkGroup = Convert.ToBoolean(Utility.Nvl(dtAuthGroup.Rows[0]["AUTH_ADD"], "0"));

                if (checkGroup)
                {
                    return true;
                }
            }


            return false;

             
        }
        public bool CheckAuthEdit(string pUSERID, string pFORMID)
        {
            var dtTable = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                  $"ISNULL(MENUFORM,0) MENUFORM," +
                  $"dbo.fncGetAUTH_STATE(ID,{pUSERID}) AUTH_STATE," +//original where state =1
                  $"dbo.fncGetAUTH_ADD(ID,{pUSERID}) AUTH_ADD," +//original where state =1
                  $"dbo.fncGetAUTH_UPDATE(ID,{pUSERID}) AUTH_UPDATE," +//original where state =1
                  $"dbo.fncGetAUTH_DELETE(ID,{pUSERID}) AUTH_DELETE FROM APP_FORMSWEB where ID={pFORMID}");//original where state =1


            var checkUser = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[0]["AUTH_UPDATE"], "0"));
            if (checkUser)
            {
                return true;
            }


            var dtUserGroup = _SQL.GetDataTable($"select * from AUTH_GROUPUSERSLINK where USERID={pUSERID}");

            for (int i = 0; i < dtUserGroup.Rows.Count; i++)
            {
                var dtAuthGroup = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                $"ISNULL(MENUFORM,0) MENUFORM," +
                $"dbo.fncGetAUTH_STATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_STATE," +//original where state =1
                $"dbo.fncGetAUTH_ADD_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_ADD," +//original where state =1
                $"dbo.fncGetAUTH_UPDATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_UPDATE," +//original where state =1
                $"dbo.fncGetAUTH_DELETE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_DELETE FROM APP_FORMSWEB  where ID={pFORMID}");//original where state =1

                var checkGroup = Convert.ToBoolean(Utility.Nvl(dtAuthGroup.Rows[0]["AUTH_UPDATE"], "0"));

                if (checkGroup)
                {
                    return true;
                }
            }


            return false;
        }
        public bool CheckAuthDelete(string pUSERID, string pFORMID)
        {
            var dtTable = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                 $"ISNULL(MENUFORM,0) MENUFORM," +
                 $"dbo.fncGetAUTH_STATE(ID,{pUSERID}) AUTH_STATE," +//original where state =1
                 $"dbo.fncGetAUTH_ADD(ID,{pUSERID}) AUTH_ADD," +//original where state =1
                 $"dbo.fncGetAUTH_UPDATE(ID,{pUSERID}) AUTH_UPDATE," +//original where state =1
                 $"dbo.fncGetAUTH_DELETE(ID,{pUSERID}) AUTH_DELETE FROM APP_FORMSWEB where ID={pFORMID}");//original where state =1


            var checkUser = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[0]["AUTH_DELETE"], "0"));
            if (checkUser)
            {
                return true;
            }


            var dtUserGroup = _SQL.GetDataTable($"select * from AUTH_GROUPUSERSLINK where USERID={pUSERID}");

            for (int i = 0; i < dtUserGroup.Rows.Count; i++)
            {
                var dtAuthGroup = _SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                $"ISNULL(MENUFORM,0) MENUFORM," +
                $"dbo.fncGetAUTH_STATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_STATE," +//original where state =1
                $"dbo.fncGetAUTH_ADD_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_ADD," +//original where state =1
                $"dbo.fncGetAUTH_UPDATE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_UPDATE," +//original where state =1
                $"dbo.fncGetAUTH_DELETE_GROUP(ID,{Utility.Nvl(dtUserGroup.Rows[i]["GROUPID"], "0")}) AUTH_DELETE FROM APP_FORMSWEB  where ID={pFORMID}");//original where state =1

                var checkGroup = Convert.ToBoolean(Utility.Nvl(dtAuthGroup.Rows[0]["AUTH_DELETE"], "0"));

                if (checkGroup)
                {
                    return true;
                }
            }


            return false;
        }
    }
}