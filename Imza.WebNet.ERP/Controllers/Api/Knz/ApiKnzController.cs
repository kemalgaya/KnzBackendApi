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
using Imza.WebNet.Erp.Models.App;
using Imza.WebNet.Erp.Models.Mobil;
using Imza.WebNet.Erp.Models.TableModel;
using Imza.WebNet.ERP.Models.TableModel.KnzTable;
using Imza.WebTools.Classes;


namespace Imza.WebNet.ERP.Controllers.Api.Knz
{
    public class ApiKnzController : Controller
    {
        #region TokenIslem


        [System.Web.Mvc.HttpPost]
        public JsonResult CheckLogin([FromBody] mobilGiris item)
        {
            try
            {

                var vToken = _SQL.CheckLogin(item.USERNAME, item.PASSWORD);

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
        #endregion

        #region TANIMTABLOLARI
        public List<GenelTanim.DEF_GENEL> DefTable(string pTable, string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
            var dtTable = SQL.GetDataTable($"select ID,ADI from dbo.{pTable} where STATE = 1 {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelTanim.DEF_GENEL> iDefGenel = new List<GenelTanim.DEF_GENEL>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelTanim.DEF_GENEL def = new GenelTanim.DEF_GENEL
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),

                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getTanimTable(string pToken, string pTableName)
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

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}"),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddTanimTable([FromBody] FormCollection collection, string pToken, string pUSERID, string pTableName)
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

                var dbOp = new ImzaData.Ops { _TableName = $"{pTableName}" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_{pTableName}_ID"));
                #region checkData

                var dTable = new DataTable();




                #region HizmetUrun

                if (pTableName == "DEF_MUHASEBE_HIZMETURUN")
                {
                    if (!string.IsNullOrEmpty(collection["BARKODNO"]))
                    {
                        dTable = SQL.GetDataTable(
                            $"select * from DEF_MUHASEBE_HIZMETURUN where BARKODNO='{collection["BARKODNO"]}'");
                        if (dTable.Rows.Count > 0)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Bu Barkod Numarası Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
                        }
                    }


                    if (string.IsNullOrEmpty(collection["BARKODNO"]))
                    {
                        collection["BARKODNO"] = ID;
                    }

                    dTable.Clear();
                }

                #endregion

                #region CariKarti

                if (pTableName == "PERSONEL_TANIM")
                {
                    if (!string.IsNullOrEmpty(collection["VERGINO"]))
                    {
                        dTable = SQL.GetDataTable(
                            $"select * from PERSONEL_TANIM where VERGINO='{collection["VERGINO"]}'");
                        if (dTable.Rows.Count > 0)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Bu Vergi Numarası Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
                        }


                        dTable.Clear();
                    }

                }

                #endregion


                #endregion




                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EditTanimTable([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID, string pTableName)
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
                var dtTable = SQL.GetDataTable($"select * from {pTableName} {sb.ToString()}");
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
                    #region checkData

                    var dTable = new DataTable();




                    #region HizmetUrun

                    if (pTableName == "DEF_MUHASEBE_HIZMETURUN")
                    {
                        if (!string.IsNullOrEmpty(collection["BARKODNO"]))
                        {
                            dTable = SQL.GetDataTable(
                                $"select * from DEF_MUHASEBE_HIZMETURUN where BARKODNO='{collection["BARKODNO"]}'");
                            if (dTable.Rows.Count > 0)
                            {
                                return Json(new { data = "", success = false, status = 402, statusText = "Bu Barkod Numarası Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
                            }
                        }



                        dTable.Clear();
                    }

                    #endregion

                    #region CariKarti

                    if (pTableName == "PERSONEL_TANIM")
                    {
                        if (!string.IsNullOrEmpty(collection["VERGINO"]))
                        {
                            dTable = SQL.GetDataTable(
                                $"select * from PERSONEL_TANIM where VERGINO='{collection["VERGINO"]}'");
                            if (dTable.Rows.Count > 0)
                            {
                                return Json(new { data = "", success = false, status = 402, statusText = "Bu Vergi Numarası Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
                            }


                            dTable.Clear();
                        }

                    }

                    #endregion


                    #endregion

                    var vdbOp = new ImzaData.Ops { _TableName = $"{pTableName}" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"{pTableName}", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Veri Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DeleteTanimTable(string pToken, string pUSERID, string PKID, string pTableName)
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

                if (pTableName == "def_PERSONEL_TURU")
                {
                    if (PKID == "1" || PKID == "5" || PKID == "6")
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Personel Türü Tablosundan ÖĞRENCİ || PERSONEL || FİRMA silinme işlemi yapılamaz" });
                    }
                }
                var dtTable = SQL.GetDataTable($"select * from {pTableName} where ID = {PKID} ");

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
                    var x = SQL.ExecuteNonQuery($"delete from {pTableName} where ID={PKID}");
                    //var checkGroup = SQL.ExecuteNonQuery($"delete from APP_PARAMETERS where GRUPID={PKID}");
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

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data Grubu bulunmadığı için silme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

        #region SLIDER

        public List<Slider.KNZ_SLIDER> DefSlider(string PKID = "",string pSIRANO="",string pORDERBY="")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

            if (!string.IsNullOrEmpty(pSIRANO))
            {
                sb.Append($" AND SIRANO={pSIRANO}");
            }

            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY) && pORDERBY=="1")
            {
                ob.Append($" ORDER BY SIRANO DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_SLIDER where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<Slider.KNZ_SLIDER> iDefGenel = new List<Slider.KNZ_SLIDER>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Slider.KNZ_SLIDER def = new Slider.KNZ_SLIDER
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"],"0")),
                        BASLIK1 = Utility.Nvl(dtTable.Rows[i]["BASLIK1"]),
                        BASLIK2 = Utility.Nvl(dtTable.Rows[i]["BASLIK2"]),
                        BUTONYAZI = Utility.Nvl(dtTable.Rows[i]["BUTONYAZI"]),
                        DOSYAYOLU = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU"]),
                        SIRANO = Convert.ToInt32(dtTable.Rows[i]["SIRANO"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getSlider(string PKID = "", string pSIRANO = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllSliderData tumveriler = new getAllSliderData
                {
                    KNZ_SLIDER = DefSlider(PKID,pSIRANO,pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddSlider([FromBody] FormCollection collection, string pToken, string pUSERID )
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
                collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_SLIDER", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_SLIDER" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_SLIDER_ID"));
                
                 
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateSlider([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID )
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_SLIDER {sb.ToString()}");
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
                    collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_SLIDER", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_SLIDER" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_SLIDER", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        
                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteSlider(string pToken, string PKID)
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


               
                var dtable = SQL.GetDataTable($" select * from KNZ_SLIDER where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_SLIDER where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    { 
                        if (!string.IsNullOrEmpty(dosyaYol))
                        {
                            new FormUtils().FileDelete(dosyaYol, Server);
                        }

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Slider Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Slider Kaldırılırken Hata Oluştu" });

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


        #region About

        public List<About.KNZ_ABOUT> DefAbout(string PKID = "" )
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

           

            var dtTable = SQL.GetDataTable($"select * from KNZ_ABOUT where STATE = 1 {sb.ToString()} ");


            if (dtTable.Rows.Count > 0)
            {

                List<About.KNZ_ABOUT> iDefGenel = new List<About.KNZ_ABOUT>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    About.KNZ_ABOUT def = new About.KNZ_ABOUT
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        DOSYAYOLUFOTO1 = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLUFOTO1"]),
                        DOSYAYOLUFOTO2 = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLUFOTO2"]),
                        DOSYAYOLUFOTO3 = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLUFOTO3"]),
                        MISYON = Utility.Nvl(dtTable.Rows[i]["MISYON"]),
                        VIZYON = Utility.Nvl(dtTable.Rows[i]["VIZYON"]),
                        ADRESS = Utility.Nvl(dtTable.Rows[i]["ADRESS"]),
                        ADRESSLINK = Utility.Nvl(dtTable.Rows[i]["ADRESSLINK"]),
                        PHONE1 = Utility.Nvl(dtTable.Rows[i]["PHONE1"]),
                        PHONE2 = Utility.Nvl(dtTable.Rows[i]["PHONE2"]),
                        MAIL = Utility.Nvl(dtTable.Rows[i]["MAIL"]),
                        SITELINK = Utility.Nvl(dtTable.Rows[i]["SITELINK"]),
                        DOSYAYOLU = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU"]),
                        DOSYAYOLU2 = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU2"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getAbout(string PKID = "" )
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllAboutData tumveriler = new getAllAboutData
                {
                    KNZ_ABOUT = DefAbout(PKID ),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddAbout([FromBody] FormCollection collection, string pToken, string pUSERID)
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
                collection["DOSYAYOLUFOTO1"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLUFOTO1", Request, Server);
                collection["DOSYAYOLUFOTO2"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLUFOTO2", Request, Server);
                collection["DOSYAYOLUFOTO3"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLUFOTO3", Request, Server);
                collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLU", Request, Server);
                collection["DOSYAYOLU2"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLU2", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_ABOUT" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_ABOUT_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateAbout([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_ABOUT {sb.ToString()}");
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
                    collection["DOSYAYOLUFOTO1"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLUFOTO1", Request, Server);
                    collection["DOSYAYOLUFOTO2"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLUFOTO2", Request, Server);
                    collection["DOSYAYOLUFOTO3"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLUFOTO3", Request, Server);
                    collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLU", Request, Server);
                    collection["DOSYAYOLU2"] = new FormUtils().FileUpload("KNZ_ABOUT", "DOSYAYOLU2", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_ABOUT" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_ABOUT", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteAbout(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_ABOUT where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));
                     

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_ABOUT where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")

                    {
                        if (!string.IsNullOrEmpty(Utility.Nvl(dtable.Rows[0]["DOSYAYOLUFOTO1"])))
                        {
                            new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[0]["DOSYAYOLUFOTO1"]), Server);
                        }
                        if (!string.IsNullOrEmpty(Utility.Nvl(dtable.Rows[0]["DOSYAYOLUFOTO2"])))
                        {
                            new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[0]["DOSYAYOLUFOTO2"]), Server);
                        }
                        if (!string.IsNullOrEmpty(Utility.Nvl(dtable.Rows[0]["DOSYAYOLUFOTO3"])))
                        {
                            new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[0]["DOSYAYOLUFOTO3"]), Server);
                        }
                        if (!string.IsNullOrEmpty(Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"])))
                        {
                            new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]), Server);
                        }
                        if (!string.IsNullOrEmpty(Utility.Nvl(dtable.Rows[0]["DOSYAYOLU2"])))
                        {
                            new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[0]["DOSYAYOLU2"]), Server);
                        }

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla About Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "About Kaldırılırken Hata Oluştu" });

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


        #region Blog

        public List<Blog.KNZ_BLOG> DefBlog(string PKID = "" , string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
             
            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_BLOG where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<Blog.KNZ_BLOG> iDefGenel = new List<Blog.KNZ_BLOG>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Blog.KNZ_BLOG def = new Blog.KNZ_BLOG
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        BASLIK = Utility.Nvl(dtTable.Rows[i]["BASLIK"]),
                        ICERIK = Utility.Nvl(dtTable.Rows[i]["ICERIK"]),
                        DOSYAYOLU = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getBlog(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllBlogData tumveriler = new getAllBlogData
                {
                    KNZ_BLOG = DefBlog(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddBlog([FromBody] FormCollection collection, string pToken, string pUSERID)
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
                collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_BLOG" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_BLOG_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateBlog([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_BLOG {sb.ToString()}");
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
                    collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_BLOG" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_BLOG", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteBlog(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_BLOG where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_BLOG where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        if (!string.IsNullOrEmpty(dosyaYol))
                        {
                            new FormUtils().FileDelete(dosyaYol, Server);
                        }

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Blog Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Blog Kaldırılırken Hata Oluştu" });

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


        #region Contact

        public List<Contact.KNZ_CONTACT> DefContact(string PKID = "" , string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
             
            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_CONTACT where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<Contact.KNZ_CONTACT> iDefGenel = new List<Contact.KNZ_CONTACT>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Contact.KNZ_CONTACT def = new Contact.KNZ_CONTACT
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        SURNAME = Utility.Nvl(dtTable.Rows[i]["SURNAME"]),
                        MAIL = Utility.Nvl(dtTable.Rows[i]["MAIL"]),
                        PHONE = Utility.Nvl(dtTable.Rows[i]["PHONE"]),
                        SUBJECT = Utility.Nvl(dtTable.Rows[i]["SUBJECT"]),
                        MESSAGE = Utility.Nvl(dtTable.Rows[i]["MESSAGE"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getContact(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllContactData tumveriler = new getAllContactData
                {
                    KNZ_CONTACT = DefContact(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddContact([FromBody] FormCollection collection, string pToken, string pUSERID)
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
                //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_CONTACT" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_CONTACT_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateContact([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_CONTACT {sb.ToString()}");
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
                    //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_CONTACT" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_CONTACT", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteContact(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_CONTACT where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                   // string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_CONTACT where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        //if (!string.IsNullOrEmpty(dosyaYol))
                        //{
                        //    new FormUtils().FileDelete(dosyaYol, Server);
                        //}

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Contact Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Contact Kaldırılırken Hata Oluştu" });

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


        #region Comment

        public List<Comment.KNZ_COMMENT> DefComment(string PKID = "", string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_COMMENT where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<Comment.KNZ_COMMENT> iDefGenel = new List<Comment.KNZ_COMMENT>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Comment.KNZ_COMMENT def = new Comment.KNZ_COMMENT
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        SURNAME = Utility.Nvl(dtTable.Rows[i]["SURNAME"]),
                        COMMENTTEXT = Utility.Nvl(dtTable.Rows[i]["COMMENTTEXT"]), 

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getComment(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllCommentData tumveriler = new getAllCommentData
                {
                    KNZ_COMMENT = DefComment(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddComment([FromBody] FormCollection collection, string pToken, string pUSERID)
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
                //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_COMMENT" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_COMMENT_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateComment([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_COMMENT {sb.ToString()}");
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
                    //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_COMMENT" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_COMMENT", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteComment(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_COMMENT where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    // string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_COMMENT where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        //if (!string.IsNullOrEmpty(dosyaYol))
                        //{
                        //    new FormUtils().FileDelete(dosyaYol, Server);
                        //}

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Contact Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Comment Kaldırılırken Hata Oluştu" });

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


        #region Customer

        public List<Customer.KNZ_CUSTOMER> DefCustomer(string PKID = "", string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_CUSTOMER where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<Customer.KNZ_CUSTOMER> iDefGenel = new List<Customer.KNZ_CUSTOMER>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Customer.KNZ_CUSTOMER def = new Customer.KNZ_CUSTOMER
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        SURNAME = Utility.Nvl(dtTable.Rows[i]["SURNAME"]),
                        MAIL = Utility.Nvl(dtTable.Rows[i]["MAIL"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getCustomer(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllCustomerData tumveriler = new getAllCustomerData
                {
                    KNZ_CUSTOMER = DefCustomer(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddCustomer([FromBody] FormCollection collection, string pToken, string pUSERID)
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
                //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_CUSTOMER" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_CUSTOMER_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateCustomer([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_CUSTOMER {sb.ToString()}");
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
                    //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_CUSTOMER" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_CUSTOMER", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteCustomer(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_CUSTOMER where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    // string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_CUSTOMER where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        //if (!string.IsNullOrEmpty(dosyaYol))
                        //{
                        //    new FormUtils().FileDelete(dosyaYol, Server);
                        //}

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Customer Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Customer Kaldırılırken Hata Oluştu" });

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


        #region Urunler

        public List<Urunler.KNZ_URUNLER> DefUrunler(string PKID = "", string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_URUNLER where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<Urunler.KNZ_URUNLER> iDefGenel = new List<Urunler.KNZ_URUNLER>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Urunler.KNZ_URUNLER def = new Urunler.KNZ_URUNLER
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        URUNADI = Utility.Nvl(dtTable.Rows[i]["URUNADI"]),
                        URUNACIKLAMA = Utility.Nvl(dtTable.Rows[i]["URUNACIKLAMA"]),
                        EBAT = Utility.Nvl(dtTable.Rows[i]["EBAT"]),
                        MARKAID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MARKAID"], "0")),
                        FIYAT = Convert.ToDecimal(Utility.Nvl(dtTable.Rows[i]["FIYAT"], "0")),
                        MINSTOK = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MINSTOK"], "0")),
                        ORTSTOK = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ORTSTOK"], "0")),
                        MINSIPARIS = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MINSIPARIS"], "0")),
                        OZELLIK1 = Utility.Nvl(dtTable.Rows[i]["OZELLIK1"]),
                        OZELLIK2 = Utility.Nvl(dtTable.Rows[i]["OZELLIK2"]),
                        OZELLIK3 = Utility.Nvl(dtTable.Rows[i]["OZELLIK3"]),
                        OZELLIK4 = Utility.Nvl(dtTable.Rows[i]["OZELLIK4"]),
                        OZELLIK5 = Utility.Nvl(dtTable.Rows[i]["OZELLIK5"]),




                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getUrunler(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllUrunData tumveriler = new getAllUrunData
                {
                    KNZ_URUNLER = DefUrunler(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddUrunler([FromBody] FormCollection collection, string pToken, string pUSERID)
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
                //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_URUNLER" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_URUNLER_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateUrunler([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_URUNLER {sb.ToString()}");
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
                    //collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_BLOG", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_URUNLER" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_URUNLER", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteUrunler(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_URUNLER where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    // string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_URUNLER where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        //if (!string.IsNullOrEmpty(dosyaYol))
                        //{
                        //    new FormUtils().FileDelete(dosyaYol, Server);
                        //}
                        SQL.ExecuteNonQuery($"delete from KNZ_URUNGORSEL where  URUNID={PKID}");

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Ürün Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Ürün Kaldırılırken Hata Oluştu" });

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


        #region UrunGorsel

        public List<UrunGorsel.KNZ_URUNGORSEL> DefUrunGorsel(string PKID = "", string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_URUNGORSEL where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<UrunGorsel.KNZ_URUNGORSEL> iDefGenel = new List<UrunGorsel.KNZ_URUNGORSEL>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UrunGorsel.KNZ_URUNGORSEL def = new UrunGorsel.KNZ_URUNGORSEL
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        URUNID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["URUNID"], "0")),
                        DOSYAYOLU = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU"]),
                         
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getUrunGorsel(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllUrunGorselData tumveriler = new getAllUrunGorselData
                {
                    KNZ_URUNGORSEL = DefUrunGorsel(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddUrunGorsel([FromBody] FormCollection collection, string pToken, string pUSERID)
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

                collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_URUNGORSEL", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_URUNGORSEL" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_URUNGORSEL_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateUrunGorsel([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_URUNLER {sb.ToString()}");
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
                    collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_URUNGORSEL", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_URUNGORSEL" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_URUNGORSEL", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteUrunGorsel(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_URUNGORSEL where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_URUNGORSEL where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        if (!string.IsNullOrEmpty(dosyaYol))
                        {
                            new FormUtils().FileDelete(dosyaYol, Server);
                        }
                        //SQL.ExecuteNonQuery($"delete from KNZ_URUNGORSEL where  URUNID={PKID}");

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Ürün Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Ürün Görselleri Kaldırılırken Hata Oluştu" });

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


        #region Kampanyalar

        public List<UrunGorsel.KNZ_URUNGORSEL> DefUrunGorsel(string PKID = "", string pORDERBY = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }

            StringBuilder ob = new StringBuilder();
            if (!string.IsNullOrEmpty(pORDERBY))
            {
                if (pORDERBY == "1")
                    ob.Append($" ORDER BY INSERT_DATE DESC");
            }

            var dtTable = SQL.GetDataTable($"select * from KNZ_URUNGORSEL where STATE = 1 {sb.ToString()} {ob.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<UrunGorsel.KNZ_URUNGORSEL> iDefGenel = new List<UrunGorsel.KNZ_URUNGORSEL>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    UrunGorsel.KNZ_URUNGORSEL def = new UrunGorsel.KNZ_URUNGORSEL
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        URUNID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["URUNID"], "0")),
                        DOSYAYOLU = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"], "0"))
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getUrunGorsel(string PKID = "", string pORDERBY = "")
        {
            try
            {
                #region Token Kontrolü

                //var vCheckToken = new iTools.token().CheckToken(pToken);
                //if (new iTools.token().CheckToken(pToken) != "1")
                //{
                //    return Json(new { success = false, status = 555, statusText = vCheckToken });
                //}

                #endregion

                getAllUrunGorselData tumveriler = new getAllUrunGorselData
                {
                    KNZ_URUNGORSEL = DefUrunGorsel(PKID, pORDERBY),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult AddUrunGorsel([FromBody] FormCollection collection, string pToken, string pUSERID)
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

                collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_URUNGORSEL", "DOSYAYOLU", Request, Server);

                var dbOp = new ImzaData.Ops { _TableName = $"KNZ_URUNGORSEL" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue($"SQE_KNZ_URUNGORSEL_ID"));


                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veri Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult UpdateUrunGorsel([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from KNZ_URUNLER {sb.ToString()}");
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
                    collection["DOSYAYOLU"] = new FormUtils().FileUpload("KNZ_URUNGORSEL", "DOSYAYOLU", Request, Server);


                    var vdbOp = new ImzaData.Ops { _TableName = $"KNZ_URUNGORSEL" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"KNZ_URUNGORSEL", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {

                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu Data bulunamadığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteUrunGorsel(string pToken, string PKID)
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



                var dtable = SQL.GetDataTable($" select * from KNZ_URUNGORSEL where ID={PKID}");




                if (dtable.Rows.Count > 0)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));

                    string dosyaYol = Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]);

                    var x = SQL.ExecuteNonQuery($"delete from KNZ_URUNGORSEL where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        if (!string.IsNullOrEmpty(dosyaYol))
                        {
                            new FormUtils().FileDelete(dosyaYol, Server);
                        }
                        //SQL.ExecuteNonQuery($"delete from KNZ_URUNGORSEL where  URUNID={PKID}");

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Ürün Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Ürün Görselleri Kaldırılırken Hata Oluştu" });

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



        public List<GenelTanim.DEF_GENEL> DefTable(string pTable)
        {
            var dtTable = SQL.GetDataTable($"select ID,ADI from dbo.{pTable} where STATE = 1");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelTanim.DEF_GENEL> iDefGenel = new List<GenelTanim.DEF_GENEL>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelTanim.DEF_GENEL def = new GenelTanim.DEF_GENEL
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),

                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }

        
        // GET: ApiKnz
        public ActionResult Index()
        {
            return View();
        }
    }
}