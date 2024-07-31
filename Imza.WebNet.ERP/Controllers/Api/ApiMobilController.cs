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
using Imza.WebNet.ERP.Models.TableModel;
using Imza.WebTools.Classes;


namespace Imza.WebNet.Erp.Controllers.Api
{


    public class ApiMobilController : Controller
    {
        [System.Web.Mvc.HttpPost]
        public JsonResult CheckLogin([FromBody] mobilGiris item)
        {
            try
            {
                var y = DateTime.Now.Year;
                var x = WebTools.Classes.iTools.Cryptation.Crypt(@">DB_IP>MONSTER
>DB_INSTANCE>IMZAMSSQL
>DB_CATALOG>KNZGROUP
>DB_USERNAME>sa
>DB_PASSWPRD>imza+123");
                var z = WebTools.Classes.iTools.Cryptation.DeCrypt(
                    "sBvYplXqGge7NeQcTcq5BrjQ6j86Hxft2yxT0/IMMZ+OzoMvNMa4oqyfcN+UA1VLwcjR3mRpH7MD64Q018qFiy5FYOzaOj9gkXPqr5HGSvrsukW8XopAJXH7bN+50IK5a/npZEBXTf4=",
                    "49120570aisehifa20815287");
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



        /// <summary>
        /// Tanımlar lookupeditler için
        /// </summary>
        /// <param name="pToken"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public JsonResult getSubeLookUpEditData(string pToken)
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

                getAllPersonelLookUpData tumveriler = new getAllPersonelLookUpData
                {
                    PERSONEL_TANIM = DefTable("DEF_SUBELER"),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }


        /// <summary>
        /// INSERT ÖRNEĞİ
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="pToken"></param>
        /// <param name="KULLANICIID"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public JsonResult PersonelEkle([FromBody] FormCollection collection, string pToken, string KULLANICIID)
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

                var dbOp = new ImzaData.Ops { _TableName = "DEF_OKUL" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_DEF_OKUL_ID"));
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(KULLANICIID, "-1"), "0", false, ID), "Yeni kayıt başarıyla tamamlandı.", "");

                if (dbOp.success)
                {
                    return Json(new { data = ID, success = true, status = 999, statusText = "Personel Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = true, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = true, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        public JsonResult YemekhaneMenuMaster([FromBody] FormCollection collection, string pToken, string KULLANICIID)
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

                var dbOp = new ImzaData.Ops { _TableName = "DEF_WEB_YEMEKHANE_MENU" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_DEF_WEB_YEMEKHANE_MENU_ID"));
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(KULLANICIID, "-1"), "0", false, ID), "Yeni kayıt başarıyla tamamlandı.", "");

                if (dbOp.success)
                {
                    return Json(new { data = ID, success = true, status = 999, statusText = "Personel Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = true, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = true, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        public JsonResult YemekhaneMenuMasterDetails([FromBody] FormCollection collection, string pToken, string KULLANICIID)
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

                var dbOp = new ImzaData.Ops { _TableName = "DEF_WEB_YEMEKHANE_MENUDETAILS" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_DEF_WEB_YEMEKHANE_MENUDETAILS_ID"));
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(KULLANICIID, "-1"), "0", false, ID), "Yeni kayıt başarıyla tamamlandı.", "");

                if (dbOp.success)
                {
                    return Json(new { data = "", success = true, status = 999, statusText = "Personel Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = true, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = true, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }

        /// <summary>
        /// UPDATE ÖRNEĞİ
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="pToken"></param>
        /// <param name="KULLANICIID"></param>
        /// <param name="PKID"></param>
        /// <returns></returns>

        [System.Web.Mvc.HttpPost]
        public JsonResult PersonelDuzenle([FromBody] FormCollection collection, string pToken, string KULLANICIID, string PKID)
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


                var dtTable = SQL.GetDataTable($"select * from PERSONEL_TANIM where ID = {PKID} AND STATE=0");

                if (dtTable.Rows.Count == 1)
                {
                    collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));


                    var vdbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "PERSONEL_TANIM", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(KULLANICIID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Personel Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = true, status = 402, statusText = "Personel Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = true, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = true, status = 401, statusText = $"{PKID} ID nolu personel Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }


        #region Get Menü

       
        void TahsilatIsle(string pCariId, string pBankaAdi, string pTutar)
        {
            #region Sisteme İşle
            var vKartIsmi = Request.Form.Get("name");

            #region Master

            //var stParamList = new dbStruct.StDbParamList();
            var dtOgrenci = SQL.GetDataTable($"select * from PERSONEL_TANIM where ID = {pCariId}");

            if (dtOgrenci.Rows.Count != 1)
            {
                return;
            }

            var BorcAlacak = MuhasebeEnums.BorcAlacakTuru.Tahsilat;
            var FormTuruId = MuhasebeEnums.FormTuru.KrediYukleme;
            var DonemId = 1; //new Constants().DonemSec();
            var vCari = pCariId;
            var vKategoriId = 1;
            //var vOkulId = Utility.Nvl(dtOgrenci.Rows[0]["OKULID"]);
            var vKasaId = 2;//Sanal Pos
            var vGelirTuruId = 1;//Servis Ödemesi
            object vODEMETURUID = 6;//Sanal Pos


            var vParamList = new dbStruct.StDbParamList();

            var vId = dbStaticUtils.GetSequenceValue("SQE_MUHASEBE_FATURA_ID");

            //TOPLAMTUTAR
            vParamList.Add("ID", vId);
            vParamList.Add("CARIID", pCariId);
            //vParamList.Add("KATEGORIID", vKategoriId);
            vParamList.Add("FATURAMUHATABI", "SanalPos");
            //vParamList.Add("ISLEMTARIHI", "getdate()", true);
            //vParamList.Add("ODEMEID", vODEMEID);
            vParamList.Add("BORCALACAKID", BorcAlacak);
            vParamList.Add("OKULID", 1);
            vParamList.Add("FORMTURUID", FormTuruId);
            vParamList.Add("ODEMETURUID", vODEMETURUID);
            vParamList.Add("ACIKLAMA", $"{vKartIsmi} kart sahibi, çekilen: {pTutar},{pBankaAdi} Sanal Pos Çekimi Yapıldı.");
            vParamList.Add("KASAHESAPID", vKasaId);
            vParamList.Add("DONEMID", DonemId);
            vParamList.Add("KREDIKARTISAHIBI", vKartIsmi);
            vParamList.Add("TOPLAMTUTAR", Utility.Nvl(pTutar).Replace(",", "."));
            //vParamList.Add("GELIRTURUID", vGelirTuruId);
            vParamList.Add("INSERT_USERID", 1);
            //vParamList.Add("BANKAID", pTable.BANKAID);
            //vParamList.Add("BANKAODEMEIBANNO", pTable.BANKAODEMEIBANNO);
            //vParamList.Add("BANKAODEMEHESAPNO", pTable.BANKAODEMEHESAPNO);
            //vParamList.Add("ISODENDI", pTable.ISODENDI);
            //vParamList.Add("TOPLAMTUTAR", vToplamTutar);

            var vSonuc = SQL.ExecuteTableInsert("MUHASEBE_FATURA", vParamList);





            #endregion

            #region Detail

            var dtKontrol = SQL.GetDataTable($"select ID from MUHASEBE_FATURA where ID {vId}");

            if (dtKontrol.Rows.Count == 1)
            {

                var vDetailId = dbStaticUtils.GetSequenceValue("SQE_MUHASEBE_FATURADETAY_ID");

                var vParamListDetay = new dbStruct.StDbParamList();

                vParamListDetay.Add("ID", vDetailId);
                vParamListDetay.Add("FKID", vId);
                vParamListDetay.Add("CARIID", pCariId);
                //vParamListDetay.Add("ISLEMTARIHI", "getdate()", true);
                //vParamListDetay.Add("VADETARIHI", DateTime.Now.ToString(), true);
                vParamListDetay.Add("MIKTAR", 1);
                vParamListDetay.Add("ACIKLAMA", "Sanal Pos Ödemesi");
                vParamListDetay.Add("FIYAT", Utility.Nvl(pTutar).Replace(",", "."));
                vParamListDetay.Add("ALACAK", Utility.Nvl(pTutar).Replace(",", "."));
                //vParamListDetay.Add("SATISFATURAID", vAidatDetayId);
                vParamListDetay.Add("BORCALACAKID", BorcAlacak);
                vParamListDetay.Add("INSERT_USERID", 1);
                vParamListDetay.Add("FORMTURUID", FormTuruId);
                vParamListDetay.Add("OKULID", 1);
                vParamListDetay.Add("ODEMETURUID", vODEMETURUID);
                //vParamListDetay.Add("BIRIMID", 1);
                //vParamListDetay.Add("ALISFATURAID", pTable.ALISFATURAID);
                //vParamListDetay.Add("ISODENDI", 1);
                //vParamListDetay.Add("TAHSILATID", pTable.TAHSILATID);
                //vParamListDetay.Add("ALACAK", pTable.ALACAK);
                //vParamListDetay.Add("ODEMEID", pTable.ODEMEID);
                //vParamListDetay.Add("AYID", DateTime.Now.Month);

                SQL.ExecuteTableInsert("MUHASEBE_FATURADETAY", vParamListDetay);
            }



            #endregion

            #endregion
        }
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



        public List<API_APP_MODULES> APP_MODULES()
        {
            var dtTable = SQL.GetDataTable($"SELECT MODULID,MODULNAME,ROWNUMBER FROM dbo.APP_MODULESWEB where STATE = 1");


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




        public List<API_APP_FORMS> APP_FORMS()
        {
            var dtTable = SQL.GetDataTable($"select ID,NAME,NAMESPACE,ICONID,MODULID,ROWNUMBER,ISNULL(MENUFORM,0) MENUFORM,MENUFORMID from APP_FORMSWEB where STATE = 1");
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
                        ICONID = Utility.Nvl(dtTable.Rows[i]["ICONID"]),
                        MODULID = Convert.ToInt32(dtTable.Rows[i]["MODULID"]),
                        ROWNUMBER = Convert.ToInt32(dtTable.Rows[i]["ROWNUMBER"]),
                        MENUFORM = Convert.ToBoolean(dtTable.Rows[i]["MENUFORM"]),
                        MENUFORMID = Convert.ToInt32(dtTable.Rows[i]["MENUFORMID"])
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
                    API_APP_FORMS = APP_FORMS(),
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

    }
}