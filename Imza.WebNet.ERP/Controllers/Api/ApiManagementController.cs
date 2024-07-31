using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Imza.WebNet.Erp.Classes;
using Imza.WebNet.ERP.Classes;
using Imza.WebNet.Erp.Models.App;
using Imza.WebNet.Erp.Models.Mobil;
using Imza.WebNet.Erp.Models.TableModel;
using Imza.WebNet.ERP.Models.TableModel;
using Imza.WebTools.Classes;

namespace Imza.WebNet.ERP.Controllers.Api
{
    public class ApiManagementController : Controller
    {
        YetkiKontrol AuthControl = new YetkiKontrol();
        [System.Web.Mvc.HttpPost]
        public JsonResult CheckLogin([FromBody] mobilGiris item)
        {
            try
            {

                var vToken = SQL.CheckLogin(item.USERNAME, item.PASSWORD);

                if (!string.IsNullOrEmpty(vToken))
                {


                    var dtUser = SQL.GetDataTable($"SELECT USERID,USERNAME,NAME,SURNAME,COMPANYID FROM dbo.AUTH_USERS WHERE USERNAME = '{item.USERNAME}' AND PASSWORD = '{WebTools.Classes.iTools.Cryptation.Crypt(item.PASSWORD)}'");//_db.AUTH_USERS.Count(f => f.USERNAME.Contains(item.USERNAME) && f.PASSWORD.Contains(item.PASSWORD));

                    if (dtUser.Rows.Count == 1)
                    {

                        var vClass = new mobilGiris
                        {
                            USERNAME = Utility.Nvl(dtUser.Rows[0]["USERNAME"]),
                            USERID = Utility.Nvl(dtUser.Rows[0]["USERID"]),
                            NAME = Utility.Nvl(dtUser.Rows[0]["NAME"]),
                            SURNAME = Utility.Nvl(dtUser.Rows[0]["SURNAME"]),
                            TOKEN = vToken
                        };

                        return Json(new { USERDATA = vClass, success = true, status = 200, statusText = "OK" });
                    }

                }


                return Json(new { success = false, status = 401, statusText = "Kullanıcı adı veya şifre yanlış" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, status = 401, statusText = "Giriş yapılırken hata oluştu detay=>" + e.Message });
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult CheckServer([FromBody] mobilGiris item, string pToken)
        {

            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            #region Yetki Kontrolü


            try
            {
                var dtUser = SQL.GetDataTable($"SELECT top 1 ID FROM dbo.APP_COMPANY");//_db.AUTH_USERS.Count(f => f.USERNAME.Contains(item.USERNAME) && f.PASSWORD.Contains(item.PASSWORD));

                if (dtUser.Rows.Count == 1)
                {
                    return Json(new { data = "Sunucu Aktif", success = true, status = 200, statusText = "Sunucu Aktif" });
                }
                else
                {
                    return Json(new { success = false, status = 401, statusText = "Sunucuya Ulaşılamıyor" });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, status = 401, statusText = "Sunucuya Ulaşılamıyor" });
            }



            #endregion
        }


        #region Kullanıcı İŞLEMLERİ
        public List<UsersManagement.AUTH_USERS> DefAuthUsers(string PKID = "", string GROUPID = "", string state = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where USERID={PKID}");
            }
            else
            {
                sb.Append($"");
            }

            if (!string.IsNullOrEmpty(GROUPID) && state == "1")
            {
                sb.Append($" where USERID in (select USERID from AUTH_GROUPUSERSLINK where GROUPID={GROUPID})");
            }
            if (!string.IsNullOrEmpty(GROUPID) && state == "2")
            {
                sb.Append($" where USERID not in (select USERID from AUTH_GROUPUSERSLINK where GROUPID={GROUPID})");
            }

            var dtTable = SQL.GetDataTable($"select * from AUTH_USERS {sb.ToString()}");//original where state =1


            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.AUTH_USERS> iDefGenel = new List<UsersManagement.AUTH_USERS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.AUTH_USERS def = new UsersManagement.AUTH_USERS
                    {
                        USERID = Convert.ToInt32(dtTable.Rows[i]["USERID"]),

                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        SURNAME = Utility.Nvl(dtTable.Rows[i]["SURNAME"]),
                        USERNAME = Utility.Nvl(dtTable.Rows[i]["USERNAME"]),
                        KIMLIKNO = Utility.Nvl(dtTable.Rows[i]["KIMLIKNO"]),
                        EMAIL = Utility.Nvl(dtTable.Rows[i]["EMAIL"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public List<UsersManagement.AUTH_USERS> DefAuthUsersById(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where USERID={PKID}");
            }
            else
            {
                sb.Append($" where STATE=1");
            }
            var dtTable = SQL.GetDataTable($"select * from AUTH_USERS {sb.ToString()}");//original where state =1

            CultureInfo cultures = new CultureInfo("en-US");
            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.AUTH_USERS> iDefGenel = new List<UsersManagement.AUTH_USERS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.AUTH_USERS def = new UsersManagement.AUTH_USERS
                    {
                        USERID = Convert.ToInt32(dtTable.Rows[i]["USERID"]),

                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        SURNAME = Utility.Nvl(dtTable.Rows[i]["SURNAME"], ""),
                        USERNAME = Utility.Nvl(dtTable.Rows[i]["USERNAME"], ""),
                        // PASSWORD = Utility.Nvl(dtTable.Rows[i]["PASSWORD"], ""),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        //INSERT_DATE = Convert.ToDateTime(Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]), System.Globalization.CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(value, "MM/yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0")),

                        // PSWCHANGEDATE =Convert.ToDateTime( Utility.Nvl(dtTable.Rows[i]["PSWCHANGEDATE"])),

                        EMAIL = Utility.Nvl(dtTable.Rows[i]["EMAIL"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }

        public JsonResult getAuthUsers(string pToken, string PKID = "", string GROUPID = "", string state = "")
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion



                getTableAuthUsersData tumveriler = new getTableAuthUsersData
                {
                    AUTH_USERS = DefAuthUsers(PKID, GROUPID, state),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Kullanıcı Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult getAuthUserById(string pToken, string PKID = "")
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion



                getTableAuthUsersData tumveriler = new getTableAuthUsersData
                {
                    AUTH_USERS = DefAuthUsersById(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Kullanıcı Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddAuthUsers([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = "AUTH_USERS" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_AUTH_USERS_ID"));
                var temp = Utility.Nvl(collection["PASSWORD"]);
                temp = WebTools.Classes.ImzaBase.ImzaCrypt.Cryptation.Crypt(temp);
                collection["PASSWORD"] = temp;
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "USERID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Kullanıcı Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Kullanıcı Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Kullanıcı Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult deneme(string pUSERID, string pToken)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
            //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
            //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
            //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
            //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
            //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
            //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


            WebTools.Classes.ImzaBase.ImzaCrypt.Cryptation.Crypt(pUSERID);



            return Json(new { data = "", success = true, status = 999, statusText = "Personel Sisteme Başarıyla Kaydedildi." });


        }
        
        public JsonResult deneme(string pUSERID)
        {



            //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
            //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
            //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
            //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
            //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
            //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
            //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


            var temp = WebTools.Classes.ImzaBase.ImzaCrypt.Cryptation.Crypt(pUSERID);



            return Json(new { data = temp, success = true, status = 999, statusText = "Personel Sisteme Başarıyla Kaydedildi." });


        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EditAuthUsers([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(PKID))
                {
                    sb.Append($" where USERID={PKID}");
                }
                else
                {
                    sb.Append($"");
                }
                var dtTable = SQL.GetDataTable($"select * from AUTH_USERS {sb.ToString()}");
                //var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));


                    var vdbOp = new ImzaData.Ops { _TableName = "AUTH_USERS" };
                    var temp = Utility.Nvl(collection["PASSWORD"]);
                    if (!string.IsNullOrEmpty(temp))
                    {
                        temp = WebTools.Classes.ImzaBase.ImzaCrypt.Cryptation.Crypt(temp);
                        collection["PASSWORD"] = temp;
                    }

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "AUTH_USERS", $" USERID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Kullanıcı Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Kullanıcı Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Kullanıcı Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteAuthUsers(string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var x = SQL.ExecuteNonQuery($"delete from AUTH_USERS where USERID={PKID}");
                    var checkGroup = SQL.ExecuteNonQuery($"delete from AUTH_GROUPUSERSLINK where  USERID={PKID} ");
                    //var checkAuth = SQL.ExecuteNonQuery($"delete AUTH_AUTHORIZE_USERSWEB where USERID={PKID}");
                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Kullanıcı bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region Kullanıcı Grup Tanımlama 

        public List<UsersManagement.AUTH_GROUPS> DefAuthGroups(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where GROUPID={PKID}");
            }
            else
            {
                sb.Append($"");
            }
            var dtTable = SQL.GetDataTable($"select * from AUTH_GROUPS {sb.ToString()}");//original where state =1


            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.AUTH_GROUPS> iDefGenel = new List<UsersManagement.AUTH_GROUPS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.AUTH_GROUPS def = new UsersManagement.AUTH_GROUPS
                    {
                        GROUPID = Convert.ToInt32(dtTable.Rows[i]["GROUPID"]),

                        GROUPNAME = Utility.Nvl(dtTable.Rows[i]["GROUPNAME"]),
                        CODE = Utility.Nvl(dtTable.Rows[i]["CODE"]),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"])),
                        // INSERT_DATE = Convert.ToDateTime(Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]), System.Globalization.CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(value, "MM/yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"])),
                        //STATE = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["STATE"]))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }

        public JsonResult getAuthGroups(string pToken, string PKID = "")
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion



                getTableAuthGroupsData tumveriler = new getTableAuthGroupsData
                {
                    AUTH_GROUPS = DefAuthGroups(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Grup Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddAuthGroup([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = "AUTH_GROUPS" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_AUTH_GROUPS_ID"));
                if (true)
                {
                }
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID), "Yeni kayıt başarıyla tamamlandı.", "");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {
                    return Json(new { data = ID, success = true, status = 999, statusText = "Grup Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Grup Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Grup Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EditAuthGroup([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(PKID))
                {
                    sb.Append($" where GROUPID={PKID}");
                }
                else
                {
                    sb.Append($"");
                }
                var dtTable = SQL.GetDataTable($"select * from AUTH_GROUPS {sb.ToString()}");
                //var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));


                    var vdbOp = new ImzaData.Ops { _TableName = "AUTH_GROUPS" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "AUTH_GROUPS", $" GROUPID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Grup Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Grup Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Grup Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DeleteAuthGroup(string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtTable = SQL.GetDataTable($"select * from AUTH_GROUPS where GROUPID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var x = SQL.ExecuteNonQuery($"delete from AUTH_GROUPS where GROUPID={PKID}");
                    var checkGroup = SQL.ExecuteNonQuery($"delete from AUTH_GROUPUSERSLINK where   GROUPID={PKID}");
                    var checkAuth = SQL.GetDataTable($"select * from AUTH_GROUPS where GROUPID={ PKID}");
                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Grup bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

        #region Kullanıcı Grup Ekleme/Çıkarma

        public JsonResult AddUserByGroup(string pToken, string pUSERID, string USERID, string GROUPID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtUser = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {USERID} ");

                if (dtUser.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Kullanıcı bulunamadığı için işlem iptal edildi." });
                }
                var dtGroup = SQL.GetDataTable($"select * from AUTH_GROUPS where GROUPID = {GROUPID} ");

                if (dtGroup.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "İlgili Grup bulunamadığı için işlem iptal edildi." });
                }
                var dtable = SQL.GetDataTable($" select * from AUTH_GROUPUSERSLINK where USERID={USERID} and GROUPID={GROUPID}");
                if (dtable.Rows.Count > 0)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Kullanıcı zaten gruba ekli." });
                }




                if (dtGroup.Rows.Count == 1 && dtUser.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var SQE_ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_AUTH_GROUPUSERSLINK_ID"));
                    var a = $"INSERT INTO AUTH_GROUPUSERSLINK VALUES({SQE_ID},{Utility.Nvl(dtGroup.Rows[0]["GROUPID"])},{Utility.Nvl(dtUser.Rows[0]["USERID"])}," +
                        $"'{SQE_ID}',{pUSERID},'{DateTime.Now}',1)";
                    var x = SQL.ExecuteNonQuery($"INSERT INTO AUTH_GROUPUSERSLINK VALUES({SQE_ID},{Utility.Nvl(dtGroup.Rows[0]["GROUPID"])},{Utility.Nvl(dtUser.Rows[0]["USERID"])}," +
                        $"'{SQE_ID}',{pUSERID},'{DateTime.Now}',1)");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Gruba Eklendi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Eklenirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"Hata Oluştu" });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteUserByGroup(string pToken, string pUSERID, string USERID, string GROUPID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtUser = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {USERID} ");

                if (dtUser.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Kullanıcı bulunamadığı için işlem iptal edildi." });
                }
                var dtGroup = SQL.GetDataTable($"select * from AUTH_GROUPS where GROUPID = {GROUPID} ");

                if (dtGroup.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "İlgili Grup bulunamadığı için işlem iptal edildi." });
                }

                var dtable = SQL.GetDataTable($" select * from AUTH_GROUPUSERSLINK where USERID={USERID} and GROUPID={GROUPID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");

                    var x = SQL.ExecuteNonQuery($"delete from AUTH_GROUPUSERSLINK where  USERID={USERID} and GROUPID={GROUPID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Gruptan ÇIKARILDI" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Gruptan çıkarılırken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"Hata Oluştu" });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }


        #endregion

        #region Parametre Grubu
        public List<UsersManagement.APP_PARAMETER_GROUPS> DefAppParameterGroups(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where ID={PKID}");
            }
            else
            {
                sb.Append($"");
            }
            var dtTable = SQL.GetDataTable($"select *,dbo.FNCGETADI_APP_MODULES2(MODULEID) as MODULENAME from APP_PARAMETER_GROUPS {sb.ToString()}");//original where state =1


            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.APP_PARAMETER_GROUPS> iDefGenel = new List<UsersManagement.APP_PARAMETER_GROUPS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.APP_PARAMETER_GROUPS def = new UsersManagement.APP_PARAMETER_GROUPS
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        MODULEID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MODULEID"], "0")),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"], "0"),
                        MODULENAME = Utility.Nvl(dtTable.Rows[i]["MODULENAME"], "0"),
                        DESCRIPTIONS = Utility.Nvl(dtTable.Rows[i]["DESCRIPTIONS"], "0"),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        // INSERT_DATE = Convert.ToDateTime(Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]), System.Globalization.CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(value, "MM/yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }

        public JsonResult getAppParameterGroups(string pToken, string PKID = "")
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion



                getTableAppParameterGroupsData tumveriler = new getTableAppParameterGroupsData
                {
                    APP_PARAMETER_GROUPS = DefAppParameterGroups(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Parametre Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddParameterGroups([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = "APP_PARAMETER_GROUPS" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_APP_PARAMETER_GROUPS_ID"));

                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Parametre Grubu Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Parametre Grubu Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Parametre Grubu Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EditAppParameterGroup([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(PKID))
                {
                    sb.Append($" where ID={PKID}");
                }
                else
                {
                    sb.Append($"");
                }
                var dtTable = SQL.GetDataTable($"select * from APP_PARAMETER_GROUPS {sb.ToString()}");
                //var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));


                    var vdbOp = new ImzaData.Ops { _TableName = "APP_PARAMETER_GROUPS" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "APP_PARAMETER_GROUPS", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Parametre Grubu Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Parametre Grubu Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DeleteAppParameterGroups(string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtTable = SQL.GetDataTable($"select * from APP_PARAMETER_GROUPS where ID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var x = SQL.ExecuteNonQuery($"delete from APP_PARAMETER_GROUPS where ID={PKID}");
                    var checkGroup = SQL.ExecuteNonQuery($"delete from APP_PARAMETERS where GRUPID={PKID}");
                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Parametre Grubu bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

        #region Parameter

        public List<UsersManagement.APP_PARAMETERS> DefAppParameter(string pGROUPID, string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where ID={PKID} and GRUPID={pGROUPID}");
            }
            else
            {
                sb.Append($" where GRUPID={pGROUPID}");
            }
            var dtTable = SQL.GetDataTable($"select * from APP_PARAMETERS {sb.ToString()}");//original where state =1


            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.APP_PARAMETERS> iDefGenel = new List<UsersManagement.APP_PARAMETERS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.APP_PARAMETERS def = new UsersManagement.APP_PARAMETERS
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        DEV_NAME = Utility.Nvl(dtTable.Rows[i]["DEV_NAME"], "0"),
                        DEV_VALUE = Utility.Nvl(dtTable.Rows[i]["DEV_VALUE"], "0"),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"], "0"),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"], "0"),
                        GRUPID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUPID"], "0")),
                        SIRA = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SIRA"], "0")),
                        TIP = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["TIP"], "0")),
                        MODULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MODULID"], "0")),
                        CODE = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["CODE"], "0")),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        // INSERT_DATE = Convert.ToDateTime(Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]), System.Globalization.CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(value, "MM/yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getAppParameters(string pToken, string pGROUPID, string PKID = "")
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion



                getTableAppParametersData tumveriler = new getTableAppParametersData
                {
                    APP_PARAMETERS = DefAppParameter(pGROUPID, PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Parametre Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddParameter([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var dtable = SQL.GetDataTable($"select * from APP_PARAMETER_GROUPS where ID={ Utility.Nvl(collection["GRUPID"])}");
                if (dtable.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Parametre Grubu doğru değil. Böyle bir grup bulunamadı." });
                }

                var dbOp = new ImzaData.Ops { _TableName = "APP_PARAMETERS" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_APP_PARAMETERS_ID"));
                collection["CODE"] = ID;
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Parametre  Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Parametre  Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Parametre  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EditAppParameter([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(PKID))
                {
                    sb.Append($" where ID={PKID}");
                }
                else
                {
                    sb.Append($"");
                }
                var dtTable = SQL.GetDataTable($"select * from APP_PARAMETERS {sb.ToString()}");
                //var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));


                    var vdbOp = new ImzaData.Ops { _TableName = "APP_PARAMETERS" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "APP_PARAMETERS", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Parametre Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Parametre Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DeleteAppParameter(string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtTable = SQL.GetDataTable($"select * from APP_PARAMETERS where ID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var x = SQL.ExecuteNonQuery($"delete from APP_PARAMETERS where ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Parametre  bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

        #region Kullanıcı Yetkilendirme

        public List<UsersManagement.APP_FORMSWEB> DefAuthAuthorizeUsersWeb(string pUSERID)
        {
            StringBuilder sb = new StringBuilder();

            var dtTable = SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                $"ISNULL(MENUFORM,0) MENUFORM," +
                $"dbo.fncGetAUTH_STATE(ID,{pUSERID}) AUTH_VIEW," +//original where state =1
                $"dbo.fncGetAUTH_ADD(ID,{pUSERID}) AUTH_ADD," +//original where state =1
                $"dbo.fncGetAUTH_UPDATE(ID,{pUSERID}) AUTH_UPDATE," +//original where state =1
                $"dbo.fncGetAUTH_DELETE(ID,{pUSERID}) AUTH_DELETE FROM APP_FORMSWEB ");//original where state =1


            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.APP_FORMSWEB> iDefGenel = new List<UsersManagement.APP_FORMSWEB>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.APP_FORMSWEB def = new UsersManagement.APP_FORMSWEB
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        MENUFORM = Utility.Nvl(dtTable.Rows[i]["MENUFORM"], "0"),
                        MODULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MODULID"], "0")),
                        ICONID = Utility.Nvl(dtTable.Rows[i]["ICONID"], "0"),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"], "0"),
                        NAMESPACE = Utility.Nvl(dtTable.Rows[i]["NAMESPACE"], "0"),
                        AUTH_ADD = Utility.Nvl(dtTable.Rows[i]["AUTH_ADD"], "0"),
                        AUTH_UPDATE = Utility.Nvl(dtTable.Rows[i]["AUTH_UPDATE"], "0"),
                        AUTH_VIEW = Utility.Nvl(dtTable.Rows[i]["AUTH_VIEW"], "0"),
                        AUTH_DELETE = Utility.Nvl(dtTable.Rows[i]["AUTH_DELETE"], "0")


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getAuthAuthorizeUsersWeb(string pToken, string pUSERID)//, string pFORMID
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion

                //var x = test.CheckAuthState(pUSERID, pFORMID);
                //if (x == false)
                //{
                //    return Json(new { data = "", success = true, status = 999, statusText = "Bu Formu Görüntülemeye yetkiniz yoktur..." });
                //}
                getTableAuthAuthorizeUsersWebData tumveriler = new getTableAuthAuthorizeUsersWebData
                {
                    APP_FORMSWEB = DefAuthAuthorizeUsersWeb(pUSERID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddAuthAuthorizeUsersWeb([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var dtable = SQL.GetDataTable($"select * from AUTH_USERS where USERID={ Utility.Nvl(collection["USERID"])}");
                if (dtable.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Kullanıcı doğru değil. Böyle bir grup bulunamadı." });
                }

                var dtable2 = SQL.GetDataTable($"select * from APP_FORMSWEB where ID={ Utility.Nvl(collection["FORMID"])}");
                if (dtable2.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Form doğru değil. Böyle bir grup bulunamadı." });
                }

                //var deleteAuthorize = SQL.ExecuteNonQuery($"delete AUTH_AUTHORIZE_USERSWEB where USERID={Utility.Nvl(collection["USERID"])}");

                var dbOp = new ImzaData.Ops { _TableName = "AUTH_AUTHORIZE_USERSWEB" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_AUTH_AUTHORIZE_USERSWEB_ID"));

                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Yetki  Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult EditAuthAuthorizeUsersWeb(string pToken, string pUSERID, string pFORMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var dtable = SQL.GetDataTable($"select * from AUTH_USERS where USERID={ pUSERID}");
                if (dtable.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Kullanıcı doğru değil. Böyle bir grup bulunamadı." });
                }

                //var dtable2 = SQL.GetDataTable($"select * from APP_FORMSWEB where ID={ Utility.Nvl(collection["FORMID"])}");
                //if (dtable2.Rows.Count < 1)
                //{
                //    return Json(new { data = "", success = true, status = 404, statusText = "Form doğru değil. Böyle bir grup bulunamadı." });
                //}

                var deleteAuthorize = SQL.ExecuteNonQuery($"delete AUTH_AUTHORIZE_USERSWEB where USERID={pUSERID} ");


                if (deleteAuthorize == "1")
                {



                    return Json(new { data = "", success = true, status = 999, statusText = "Kullanıcının geçmiş yetkileri silindi  " });
                }
                else
                {

                    return Json(new { data = "", success = false, status = 1806, statusText = "Kullanıcıya ait bir yetki bulunamadı" });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }




        #endregion

        #region YetkiKontrol

        public JsonResult CheckAuthState(string pToken, string pUSERID, string pFORMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var check = AuthControl.CheckAuthState(pUSERID, pFORMID);
                
                if (check)
                {



                    return Json(new { data = check, success = true, status = 999, statusText = "Kullanıcının Görüntüleme Yetkisi var  " });
                }
                else
                {

                    return Json(new { data = check, success = false, status = 402, statusText = "Görüntüleme Yetkisi Bulunmamaktadır....." });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult CheckAuthAdd(string pToken, string pUSERID, string pFORMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var check = AuthControl.CheckAuthAdd(pUSERID, pFORMID);

                if (check)
                {



                    return Json(new { data = check, success = true, status = 999, statusText = "Kullanıcının Ekleme Yetkisi var  " });
                }
                else
                {

                    return Json(new { data = check, success = false, status = 402, statusText = "Ekleme Yetkisi Bulunmamaktadır....." });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult CheckAuthUpdate(string pToken, string pUSERID, string pFORMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var check = AuthControl.CheckAuthEdit(pUSERID, pFORMID);

                if (check)
                {



                    return Json(new { data = check, success = true, status = 999, statusText = "Kullanıcının Görüntüleme Yetkisi var  " });
                }
                else
                {

                    return Json(new { data = check, success = false, status = 402, statusText = "Görüntüleme Yetkisi Bulunmamaktadır....." });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult CheckAuthDelete(string pToken, string pUSERID, string pFORMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var check = AuthControl.CheckAuthDelete(pUSERID, pFORMID);

                if (check)
                {



                    return Json(new { data = check, success = true, status = 999, statusText = "Kullanıcının Görüntüleme Yetkisi var  " });
                }
                else
                {

                    return Json(new { data = check, success = false, status = 402, statusText = "Görüntüleme Yetkisi Bulunmamaktadır....." });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        #endregion

        #region Grup Yetkilendirme
        public List<UsersManagement.APP_FORMSWEB> DefAuthAuthorizeUsersGroupWeb(string pGROUPID)
        {
            StringBuilder sb = new StringBuilder();

            var dtTable = SQL.GetDataTable($"SELECT ID,ISNULL(MENUFORM,0) MENUFORM,MODULID,ICONID, NAME,NAMESPACE," +
                $"ISNULL(MENUFORM,0) MENUFORM," +
                $"dbo.fncGetAUTH_STATE_GROUP(ID,{pGROUPID}) AUTH_VIEW," +//original where state =1
                $"dbo.fncGetAUTH_ADD_GROUP(ID,{pGROUPID}) AUTH_ADD," +//original where state =1
                $"dbo.fncGetAUTH_UPDATE_GROUP(ID,{pGROUPID}) AUTH_UPDATE," +//original where state =1
                $"dbo.fncGetAUTH_DELETE_GROUP(ID,{pGROUPID}) AUTH_DELETE FROM APP_FORMSWEB ");//original where state =1

            if (dtTable.Rows.Count > 0)
            {

                List<UsersManagement.APP_FORMSWEB> iDefGenel = new List<UsersManagement.APP_FORMSWEB>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UsersManagement.APP_FORMSWEB def = new UsersManagement.APP_FORMSWEB
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        MENUFORM = Utility.Nvl(dtTable.Rows[i]["MENUFORM"], "0"),
                        MODULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MODULID"], "0")),
                        ICONID = Utility.Nvl(dtTable.Rows[i]["ICONID"], "0"),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"], "0"),
                        NAMESPACE = Utility.Nvl(dtTable.Rows[i]["NAMESPACE"], "0"),
                        AUTH_ADD = Utility.Nvl(dtTable.Rows[i]["AUTH_ADD"], "0"),
                        AUTH_UPDATE = Utility.Nvl(dtTable.Rows[i]["AUTH_UPDATE"], "0"),
                        AUTH_VIEW = Utility.Nvl(dtTable.Rows[i]["AUTH_VIEW"], "0"),
                        AUTH_DELETE = Utility.Nvl(dtTable.Rows[i]["AUTH_DELETE"], "0")


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getAuthAuthorizeUsersGroupWeb(string pToken, string pGROUPID)
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion

                //var x = test.CheckAuthState(pUSERID, pFORMID);
                //if (x == false)
                //{
                //    return Json(new { data = "", success = true, status = 999, statusText = "Bu Formu Görüntülemeye yetkiniz yoktur..." });
                //}
                getTableAuthAuthorizeUsersWebData tumveriler = new getTableAuthAuthorizeUsersWebData
                {
                    APP_FORMSWEB = DefAuthAuthorizeUsersGroupWeb(pGROUPID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddAuthAuthorizeUsersGroupWeb([FromBody] FormCollection collection, string pToken, string pUSERID, string pGROUPID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var dtable = SQL.GetDataTable($"select * from AUTH_GROUPS where GROUPID={ pGROUPID}");
                if (dtable.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Kullanıcı doğru değil. Böyle bir grup bulunamadı." });
                }

                var dtable2 = SQL.GetDataTable($"select * from APP_FORMSWEB where ID={ Utility.Nvl(collection["FORMID"])}");
                if (dtable2.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Form doğru değil. Böyle bir grup bulunamadı." });
                }

                var dbOp = new ImzaData.Ops { _TableName = "AUTH_AUTHORIZE_GROUPSWEB" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_AUTH_AUTHORIZE_GROUPSWEB_ID"));

                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Yetki  Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        public JsonResult EditAuthAuthorizeUsersGroupWeb(string pToken, string pGROUPID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {
                //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
                //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server);
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);


                var dtable = SQL.GetDataTable($"select * from AUTH_GROUPS where GROUPID={ pGROUPID}");
                if (dtable.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 404, statusText = "Grup doğru değil. Böyle bir grup bulunamadı." });
                }

                //var dtable2 = SQL.GetDataTable($"select * from APP_FORMSWEB where ID={ Utility.Nvl(collection["FORMID"])}");
                //if (dtable2.Rows.Count < 1)
                //{
                //    return Json(new { data = "", success = true, status = 404, statusText = "Form doğru değil. Böyle bir grup bulunamadı." });
                //}

                var deleteAuthorize = SQL.ExecuteNonQuery($"delete AUTH_AUTHORIZE_GROUPSWEB where GROUPID={pGROUPID}");


                if (deleteAuthorize == "1")
                {



                    return Json(new { data = "", success = true, status = 999, statusText = "Kullanıcının geçmiş yetkileri silindi  " });
                }
                else
                {

                    return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu" });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Yetki  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }


        #endregion

        #region SequenceGüncelleme
        public JsonResult UpdateSequenceActionJsonResult(string pToken)
        {
            try
            {
                #region Token Kontrolü

                var vCheckToken = new iTools.token().CheckToken(pToken);
                if (new iTools.token().CheckToken(pToken) != "1")
                {
                    return Json(new { success = false, status = 555, statusText = vCheckToken });
                }

                #endregion

                var vdbOp = new devUtils();

                if (vdbOp.updateAllSequence())
                {
                    return Json(new { data = "", success = true, status = 200, statusText = "Sequence'ler Başarıyla Güncellendi" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { data = "", success = false, status = 999, statusText = "Sequence'ler Başarıyla Güncellendi" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Sequence Güncellenirken Hata oluştu => " + e.Message });
            }
            
        }
        #endregion


        #region Form Tanımlama
        [System.Web.Mvc.HttpPost]
        public JsonResult getAppForms(string pUSERID, string pToken, string PKID = "",string pNAMESPACE="")
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {

                getAllMenu tumveriler = new getAllMenu
                {
                    API_APP_FORMS = APP_FORMS(pUSERID,PKID, pNAMESPACE)

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });




                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddAppForms([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dbOp = new ImzaData.Ops { _TableName = "APP_FORMSWEB" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_APP_FORMSWEB_ID"));

                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Form  Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Form  Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Form  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult EditAppForms([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(PKID))
                {
                    sb.Append($" where ID={PKID}");
                }
                else
                {
                    sb.Append($"");
                }
                var dtTable = SQL.GetDataTable($"select * from APP_FORMSWEB {sb.ToString()}");
                //var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));
                    //collection["ICONID"] = pUSERID;

                    var vdbOp = new ImzaData.Ops { _TableName = "APP_FORMSWEB" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "APP_FORMSWEB", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Form Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Form Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteAppForms(string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtTable = SQL.GetDataTable($"select * from APP_FORMSWEB where ID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var x = SQL.ExecuteNonQuery($"delete from APP_FORMSWEB where ID={PKID}");
                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Form bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult kemaltest()
        {
            string a = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" class=\"feather feather-code\"><polyline points=\"16 18 22 12 16 6\"></polyline><polyline points=\"8 6 2 12 8 18\"></polyline></svg>";

            string s = HttpUtility.HtmlDecode(a);

            return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => "  });
        }
        #endregion

        #region Menü Tanımlana
        public JsonResult getAppModules(string pUSERID, string pToken, string PKID = "")
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {

                getAllMenu tumveriler = new getAllMenu
                {
                    API_APP_MODULES = APP_MODULES(PKID)

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });




                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddAppModules([FromBody] FormCollection collection, string pToken, string pUSERID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dbOp = new ImzaData.Ops { _TableName = "APP_MODULESWEB" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_APP_MODULESWEB_ID"));

                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "MODULID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Modül  Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Modül  Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Modül  Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult EditAppModules([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(PKID))
                {
                    sb.Append($" where MODULID={PKID}");
                }
                else
                {
                    sb.Append($"");
                }
                var dtTable = SQL.GetDataTable($"select * from APP_MODULESWEB {sb.ToString()}");
                //var dtTable = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));


                    var vdbOp = new ImzaData.Ops { _TableName = "APP_MODULESWEB" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "APP_MODULESWEB", $" MODULID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Modül Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Modül Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteAppModules(string pToken, string pUSERID, string PKID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion

            try
            {


                var dtTable = SQL.GetDataTable($"select * from APP_MODULESWEB where MODULID = {PKID} ");

                if (dtTable.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var x = SQL.ExecuteNonQuery($"delete from APP_MODULESWEB where MODULID={PKID}");
                    var y = SQL.ExecuteNonQuery($"delete from APP_FORMSWEB where MODULID={PKID}");
                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata Oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Form bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        #endregion


        #region Get Menü


        [System.Web.Mvc.HttpPost]
        public JsonResult getMenu(string pUSERID, string pToken)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {
                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }



        public List<API_APP_MODULES> APP_MODULES(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and MODULID={PKID}");
            }
            var dtTable = SQL.GetDataTable($"SELECT MODULID,MODULNAME,ROWNUMBER FROM dbo.APP_MODULESWEB where STATE = 1 {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<API_APP_MODULES> iDefGenel = new List<API_APP_MODULES>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    API_APP_MODULES def = new API_APP_MODULES
                    {
                        MODULID = Convert.ToInt32(dtTable.Rows[i]["MODULID"]),

                        MODULNAME = Utility.Nvl(dtTable.Rows[i]["MODULNAME"]),

                        ROWNUMBER = Convert.ToInt32(dtTable.Rows[i]["ROWNUMBER"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }




        public List<API_APP_FORMS> APP_FORMS(string pUSERID,string PKID = "", string pNameSpace="")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }
            if (!string.IsNullOrEmpty(pNameSpace))
            {
                sb.Append($" and NAMESPACE='{pNameSpace}'");
            }
           // var dtTable = SQL.GetDataTable($"select ID,NAME,NAMESPACE,ICONID,MODULID,ROWNUMBER,ISNULL(MENUFORM,0) MENUFORM,MENUFORMID from APP_FORMSWEB where STATE = 1  {sb.ToString()}");
           var dtTable = SQL.GetDataTable($"select ID,NAME,NAMESPACE,ICONID,MODULID,ROWNUMBER,ISNULL(MENUFORM,0) MENUFORM,MENUFORMID from APP_FORMSWEB where STATE = 1 and dbo.fncGetAUTH_STATE(ID,{pUSERID}) = 1 {sb.ToString()}");
            //string a = $@"emal gaya{dfbdfb}   ""böyle dedi""  ";
            // string svgIcon = $@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""feather feather-activity"" ><polyline points=""22 12 18 12 15 21 9 3 6 12 2 12"" ></polyline></svg>";
            if (dtTable.Rows.Count > 0)
            {

                List<API_APP_FORMS> iDefGenel = new List<API_APP_FORMS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    API_APP_FORMS def = new API_APP_FORMS
                    {
                        //activity
                        //<i data-feather='alert-circle'></i>
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        NAMESPACE = Utility.Nvl(dtTable.Rows[i]["NAMESPACE"]),
                        // ICONID = Utility.Nvl(dtTable.Rows[i]["ICONID"]).Replace("&lt;i data-feather=&#39;", "").Replace("&#39;&gt;&lt;/i&gt;", ""),
                        ICONID = Utility.Nvl(dtTable.Rows[i]["ICONID"], "0").Replace("&lt;","<").Replace("&gt;",">").Replace("&quot;", "\""),
                        MODULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MODULID"], "0")),
                        ROWNUMBER = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ROWNUMBER"], "0")),
                        MENUFORM = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["MENUFORM"], "0")),
                        MENUFORMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MENUFORMID"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }



        [System.Web.Mvc.HttpPost]
        public JsonResult getMenuHam(string pUSERID, string pToken)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {

                getAllMenu tumveriler = new getAllMenu
                {
                    API_APP_FORMS = APP_FORMS(pUSERID),
                    API_APP_MODULES = APP_MODULES()
                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });


                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }
        #endregion

        #region DashBoard
        public List<GenelAnaliz.PERSONELTURUSAYI> DefPersonelTuruSayi(string DONEMID)
        {
             
            var dtTable = SQL.GetDataTable($"select (select COUNT(ID) from PERSONEL_TANIM where PERSONELTURUID=t.ID and DONEMID={DONEMID} and STATE=1) SAYISI,* from def_PERSONEL_TURU t");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelAnaliz.PERSONELTURUSAYI> iDefGenel = new List<GenelAnaliz.PERSONELTURUSAYI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelAnaliz.PERSONELTURUSAYI def = new GenelAnaliz.PERSONELTURUSAYI
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"],"0")),

                        SAYISI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SAYISI"],"0")),

                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getPersonelTuruSayi(string pUSERID, string pToken, string pDONEMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {


                getDashboardData tumveriler = new getDashboardData
                {
                    PERSONELTURUSAYI = DefPersonelTuruSayi(pDONEMID)
                   
                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });


                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }
        //select COUNT(ID) TOPLAMPERSONELSAYI from PERSONEL_TANIM where  DONEMID=5 and STATE=1
        public List<GenelAnaliz.DASHBOARDSAYI> DefPersonelSayi(string DONEMID)
        {

            var dtTable = SQL.GetDataTable($"select COUNT(ID) TOPLAMPERSONELSAYI from PERSONEL_TANIM where  DONEMID={DONEMID} and STATE=1");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelAnaliz.DASHBOARDSAYI> iDefGenel = new List<GenelAnaliz.DASHBOARDSAYI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelAnaliz.DASHBOARDSAYI def = new GenelAnaliz.DASHBOARDSAYI
                    {
                        TOPLAMPERSONELSAYI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["TOPLAMPERSONELSAYI"], "0"))
                      

                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getPersonelSayi(string pUSERID, string pToken, string pDONEMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {


                getDashboardData tumveriler = new getDashboardData
                {
                    DASHBOARDSAYI = DefPersonelSayi(pDONEMID)

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });


                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }
        //select COUNT(ID) ARACSAYISI from DEF_PERSONEL_SERVISARAC where STATE=1
        public List<GenelAnaliz.DASHBOARDSAYI> DefAracSayi()
        {

            var dtTable = SQL.GetDataTable($"select COUNT(ID) ARACSAYISI from DEF_PERSONEL_SERVISARAC where STATE=1");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelAnaliz.DASHBOARDSAYI> iDefGenel = new List<GenelAnaliz.DASHBOARDSAYI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelAnaliz.DASHBOARDSAYI def = new GenelAnaliz.DASHBOARDSAYI
                    {
                        ARACSAYISI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ARACSAYISI"], "0"))


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getAracSayi(string pUSERID, string pToken, string pDONEMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {


                getDashboardData tumveriler = new getDashboardData
                {
                    DASHBOARDSAYI = DefAracSayi()

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });


                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }
        //select
        //    (select COUNT(ID) from PERSONEL_GUZERGAH where STATE = 1 and DONEMID = 5) ANAGUZERGAH,
        //(select COUNT(ID) from PERSONEL_GUZERGAHDETAY where FKID in (select ID from PERSONEL_GUZERGAH)) GUZERGAHNOKTASI
        public List<GenelAnaliz.DASHBOARDSAYI> DefGuzergah(string DONEMID)
        {

            var dtTable = SQL.GetDataTable($"select " +
                                           $" (select COUNT(ID) from PERSONEL_GUZERGAH where STATE=1 and DONEMID={DONEMID}) ANAGUZERGAH, " +
                                           $" (select COUNT(ID) from PERSONEL_GUZERGAHDETAY where FKID in (select ID from PERSONEL_GUZERGAH)) GUZERGAHNOKTASI ");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelAnaliz.DASHBOARDSAYI> iDefGenel = new List<GenelAnaliz.DASHBOARDSAYI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelAnaliz.DASHBOARDSAYI def = new GenelAnaliz.DASHBOARDSAYI
                    {
                       
                        ANAGUZERGAH = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ANAGUZERGAH"], "0")),
                        GUZERGAHNOKTASI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GUZERGAHNOKTASI"], "0"))

                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getGuzergah(string pUSERID, string pToken, string pDONEMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {


                getDashboardData tumveriler = new getDashboardData
                {
                    DASHBOARDSAYI = DefGuzergah(pDONEMID)

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });


                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }
        public List<GenelAnaliz.DASHBOARDSAYI> DefDashboard(string DONEMID)
        {

            var dtTable = SQL.GetDataTable($"select " +
                                           $" (select COUNT(ID) from PERSONEL_TANIM where  DONEMID={DONEMID} and STATE=1) TOPLAMPERSONELSAYI, " +
                                           $" (select COUNT(ID) ARACSAYISI from DEF_PERSONEL_SERVISARAC where STATE=1) ARACSAYISI, " +
                                           $" (select COUNT(ID) from PERSONEL_GUZERGAH where STATE=1 and DONEMID={DONEMID}) ANAGUZERGAH, " +
                                           $" (select COUNT(ID) from PERSONEL_GUZERGAHDETAY where FKID in (select ID from PERSONEL_GUZERGAH)) GUZERGAHNOKTASI ");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelAnaliz.DASHBOARDSAYI> iDefGenel = new List<GenelAnaliz.DASHBOARDSAYI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelAnaliz.DASHBOARDSAYI def = new GenelAnaliz.DASHBOARDSAYI
                    {
                        TOPLAMPERSONELSAYI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["TOPLAMPERSONELSAYI"], "0")),
                        ARACSAYISI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ARACSAYISI"], "0")),
                        ANAGUZERGAH = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ANAGUZERGAH"], "0")),
                        GUZERGAHNOKTASI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GUZERGAHNOKTASI"], "0"))
 
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getDashbooard(string pUSERID, string pToken,string pDONEMID)
        {
            #region Token Kontrolü

            var vCheckToken = new iTools.token().CheckToken(pToken);
            if (new iTools.token().CheckToken(pToken) != "1")
            {
                return Json(new { success = false, status = 555, statusText = vCheckToken });
            }

            #endregion


            if (string.IsNullOrEmpty(pUSERID))
            {
                return Json(new { data = "", success = false, status = 401, statusText = "Kullanıcı Bilgisi Alınamadı" });
            }

            try
            {


                getDashboardData tumveriler = new getDashboardData
                {
                    PERSONELTURUSAYI = DefPersonelTuruSayi(pDONEMID),
                    DASHBOARDSAYI = DefDashboard(pDONEMID)
                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });


                var vMenu = new WebUtils.ThemeApi().CreateAuthMenu(pUSERID);

                //return Json(new { data = vMenu, success = true, status = 200, statusText = "Yetkili Olduğunuz Menü Listesi" });

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Hata Oluştu => " + e.Message });
            }

        }

        #endregion
        public ActionResult Index()
        {
            return View();
        }
    }
}