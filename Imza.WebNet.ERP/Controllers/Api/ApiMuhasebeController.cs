using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Imza.WebNet.Erp.Classes;
using Imza.WebNet.Erp.Models.Mobil;
using Imza.WebNet.Erp.Models.TableModel;
using Imza.WebNet.ERP.Models.TableModel;
using Imza.WebTools.Classes;

namespace Imza.WebNet.ERP.Controllers.Api
{
    public class ApiMuhasebeController : Controller
    {
        // GET: ApiMuhasebe

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

        #region TANIMLAR

        #region MUHASEBE_HIZMETURUN

        public List<MuhasebeHizmetUrun.DEF_MUHASEBE_HIZMETURUN> DefMuhasebeHizmetUrun(string PKID = "", string pBIRIMID = "", string pKATEGORIGRUPID = "", string pFORMTURUID = "", string pBARKODNO = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }
            if (!string.IsNullOrEmpty(pBIRIMID))
            {
                sb.Append($" and BIRIMID={pBIRIMID}");
            }
            if (!string.IsNullOrEmpty(pKATEGORIGRUPID))
            {
                sb.Append($" and KATEGORIGRUPID={pKATEGORIGRUPID}");
            }
            if (!string.IsNullOrEmpty(pFORMTURUID))
            {
                sb.Append($" and FORMTURUID={pFORMTURUID}");
            }
            if (!string.IsNullOrEmpty(pBARKODNO))
            {
                sb.Append($" and BARKODNO='{pBARKODNO}'");
            }


            var dtTable = SQL.GetDataTable($"select * from viewDEF_MUHASEBE_HIZMETURUN WHERE STATE=1  {sb.ToString()}");

            if (dtTable.Rows.Count > 0)
            {

                List<MuhasebeHizmetUrun.DEF_MUHASEBE_HIZMETURUN> iDefGenel = new List<MuhasebeHizmetUrun.DEF_MUHASEBE_HIZMETURUN>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    MuhasebeHizmetUrun.DEF_MUHASEBE_HIZMETURUN def = new MuhasebeHizmetUrun.DEF_MUHASEBE_HIZMETURUN
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        KISAKOD = Utility.Nvl(dtTable.Rows[i]["KISAKOD"]),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                        BIRIMADI = Utility.Nvl(dtTable.Rows[i]["BIRIMADI"]),
                        FORMTURUADI = Utility.Nvl(dtTable.Rows[i]["FORMTURUADI"]),
                        KDVORAN = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["KDVORAN"], "0")),

                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        ALISFIYATI = float.Parse(Utility.Nvl(dtTable.Rows[i]["ALISFIYATI"], "0")),
                        SATISFIYATI = float.Parse(Utility.Nvl(dtTable.Rows[i]["SATISFIYATI"], "0")),
                        KATEGORIGRUPADI = Utility.Nvl(dtTable.Rows[i]["KATEGORIGRUPADI"]),
                        ISMALZEME = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["ISMALZEME"])),
                        BARKODNO = Utility.Nvl(dtTable.Rows[i]["BARKODNO"]),
                        SATISTOPLAM = float.Parse(Utility.Nvl(dtTable.Rows[i]["SATISTOPLAM"], "0")),
                        ALISTOPLAM = float.Parse(Utility.Nvl(dtTable.Rows[i]["ALISTOPLAM"], "0")),
                        ALISMIKTARI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ALISMIKTARI"], "0")),
                        SATISMIKTARI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SATISMIKTARI"], "0")),
                        STOK = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["STOK"], "0")),
                        STOKUYARISAYISI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["STOKUYARISAYISI"], "0")),
                        BIRIMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BIRIMID"], "0")),
                        FORMTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["FORMTURUID"], "0")),
                        KATEGORIGRUPID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["KATEGORIGRUPID"], "0"))

                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getMuhasebeHizmetUrun(string pToken, string PKID = "", string pBIRIMID = "", string pKATEGORIGRUPID = "", string pFORMTURUID = "", string pBARKODNO = "")
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

                getMuhasebeHizmetUrunData tumveriler = new getMuhasebeHizmetUrunData
                {
                    DEF_MUHASEBE_HIZMETURUN = DefMuhasebeHizmetUrun(PKID, pBIRIMID, pKATEGORIGRUPID, pFORMTURUID, pBARKODNO),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Stok Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult StokBirlestirme(string pToken, string pAKTARILACAKURUNID, string pSTOKID)
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


                var dTable = SQL.GetDataTable($"select * from DEF_MUHASEBE_HIZMETURUN where ID={pAKTARILACAKURUNID}");
                if (dTable.Rows.Count < 1)
                {
                    return Json(new { data = "", success = true, status = 999, statusText = "Aktarılacak Ürün Bulunamadı" });
                }
                var dTable2 = SQL.GetDataTable($"select * from DEF_MUHASEBE_HIZMETURUN where ID={pSTOKID}");
                if (dTable2.Rows.Count < 1)
                {
                    return Json(new { data = "", success = true, status = 999, statusText = "Ana Ürün Bulunamadı" });
                }

                var x = SQL.ExecuteNonQuery($"update MUHASEBE_FATURADETAY set URUNHIZMETID={pSTOKID} where URUNHIZMETID={pAKTARILACAKURUNID}");
                if (x != "-1")
                {
                    return Json(new { data = "", success = true, status = 999, statusText = "Stok Aktarımı Tamamlandı" });
                }
                else
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Hata Oluştu" });
                }


            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Stok Aktarımı Alınırken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region Kasa/Hesap Bilgileri
        public List<MuhasebeKasa.DEF_MUHASEBE_KASA> DefMuhasebeKasa(string PKID = "", string pBANKAID = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }
            if (!string.IsNullOrEmpty(pBANKAID))
            {
                sb.Append($" and BANKAID={pBANKAID}");
            }

            var dtTable = SQL.GetDataTable($"select * from viewDEF_MUHASEBE_KASA WHERE STATE=1  {sb.ToString()}");

            if (dtTable.Rows.Count > 0)
            {

                List<MuhasebeKasa.DEF_MUHASEBE_KASA> iDefGenel = new List<MuhasebeKasa.DEF_MUHASEBE_KASA>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    MuhasebeKasa.DEF_MUHASEBE_KASA def = new MuhasebeKasa.DEF_MUHASEBE_KASA
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                        BANKAID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BANKAID"], "0")),

                        HESAPADI = Utility.Nvl(dtTable.Rows[i]["HESAPADI"]),
                        IBAN = Utility.Nvl(dtTable.Rows[i]["IBAN"]),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        BANKAADI = Utility.Nvl(dtTable.Rows[i]["BANKAADI"])

                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getMuhasebeKasa(string pToken, string PKID = "", string pBANKAID = "")
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

                getMuhasebeKasaData tumveriler = new getMuhasebeKasaData
                {
                    DEF_MUHASEBE_KASA = DefMuhasebeKasa(PKID, pBANKAID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Kasa Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }


        #endregion

        #region MuhasebeGiderGelir

        public List<MuhasebeGiderGelir.DEF_MUHASEBE_GIDERGELIR> DefMuhasebeGiderGelir(string PKID = "", string pTURUID = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }
            if (!string.IsNullOrEmpty(pTURUID))
            {
                sb.Append($" and TURUID={pTURUID}");
            }

            var dtTable = SQL.GetDataTable($"select * from viewDEF_MUHASEBE_GIDERGELIR WHERE STATE=1  {sb.ToString()}");

            if (dtTable.Rows.Count > 0)
            {

                List<MuhasebeGiderGelir.DEF_MUHASEBE_GIDERGELIR> iDefGenel = new List<MuhasebeGiderGelir.DEF_MUHASEBE_GIDERGELIR>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    MuhasebeGiderGelir.DEF_MUHASEBE_GIDERGELIR def = new MuhasebeGiderGelir.DEF_MUHASEBE_GIDERGELIR
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                        TURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["TURUID"], "0")),
                        TURU = Utility.Nvl(dtTable.Rows[i]["TURU"]),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"]))


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getMuhasebeGiderGelir(string pToken, string PKID = "", string pTURUID = "")
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

                getMuhasebeGiderGelirData tumveriler = new getMuhasebeGiderGelirData
                {
                    DEF_MUHASEBE_GIDERGELIR = DefMuhasebeGiderGelir(PKID, pTURUID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Gider Gelir Türü Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

        #region Kullanıcı Okul Engelleme

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

        public List<GenelTanim.DEF_GENEL> DefKullaniciOKul(string pUSERID, string pENGELSTATE)//state 1 ise engelli olanlar 0 ise izin verilenler
        {
            var dtTable = new DataTable();

            if (pENGELSTATE == "1")
            {
                dtTable = SQL.GetDataTable($"select ID,ADI from DEF_OKUL where ID  in(select OKULID from DEF_ENGELLENECEK_KULLANICILAROKUL where KULLANICIID={pUSERID})");
            }
            else
            {
                dtTable = SQL.GetDataTable($"select ID,ADI from DEF_OKUL where ID not in(select OKULID from DEF_ENGELLENECEK_KULLANICILAROKUL where KULLANICIID={pUSERID})");
            }



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
        public JsonResult getKullaniciOkul(string pToken, string pUSERID, string pENGELSTATE)
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
                    DEF_GENEL = DefKullaniciOKul(pUSERID, pENGELSTATE),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Okul Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult KullaniciOkulEngellleme(string pToken, string pUSERID, string pEKLENECEKKULLANICI,
            string pOKULID)
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


                var dtUser = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {pEKLENECEKKULLANICI} ");

                if (dtUser.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Kullanıcı bulunamadığı için işlem iptal edildi." });
                }
                var dtOkul = SQL.GetDataTable($"select * from DEF_OKUL where ID = {pOKULID} ");

                if (dtOkul.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "İlgili OKUL bulunamadığı için işlem iptal edildi." });
                }
                var dtable = SQL.GetDataTable($" select * from DEF_ENGELLENECEK_KULLANICILAROKUL where KULLANICIID={pEKLENECEKKULLANICI} and OKULID={pOKULID}");
                if (dtable.Rows.Count > 0)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Okul zaten engellenmiş" });
                }




                if (dtOkul.Rows.Count == 1 && dtUser.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var SQE_ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_DEF_ENGELLENECEK_KULLANICILAROKUL_ID"));
                    var a = $"INSERT INTO DEF_ENGELLENECEK_KULLANICILAROKUL VALUES({SQE_ID},{pOKULID},{pEKLENECEKKULLANICI}," +
                            $"{pUSERID},CONVERT(DATETIME,'{DateTime.Now}',104),1,1)";

                    var x = SQL.ExecuteNonQuery($"INSERT INTO DEF_ENGELLENECEK_KULLANICILAROKUL VALUES({SQE_ID},{pOKULID},{pEKLENECEKKULLANICI}," +
                        $"{pUSERID},CONVERT(DATETIME,'{DateTime.Now}',104),1,1)");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Okul Başarıyla Engellendi " });

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

        public JsonResult KullaniciOkulEngelllemeKaldir(string pToken, string pUSERID, string pEKLENECEKKULLANICI,
            string pOKULID)
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


                var dtUser = SQL.GetDataTable($"select * from AUTH_USERS where USERID = {pEKLENECEKKULLANICI} ");

                if (dtUser.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Kullanıcı bulunamadığı için işlem iptal edildi." });
                }
                var dtOkul = SQL.GetDataTable($"select * from DEF_OKUL where ID = {pOKULID} ");

                if (dtOkul.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "İlgili OKUL bulunamadığı için işlem iptal edildi." });
                }

                var dtable = SQL.GetDataTable($" select * from DEF_ENGELLENECEK_KULLANICILAROKUL where KULLANICIID={pEKLENECEKKULLANICI} and OKULID={pOKULID}");




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

                    var x = SQL.ExecuteNonQuery($"delete from DEF_ENGELLENECEK_KULLANICILAROKUL where  KULLANICIID={pEKLENECEKKULLANICI} and OKULID={pOKULID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Engelleme Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Engel Kaldırılırken Hata Oluştu" });

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

        #region VeliKullanici

        public List<VeliKullanici.DEF_VELIKULLANICI> DefVeliKullanici(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where ID={PKID}");
            }
            else
            {
                sb.Append($" where STATE=1");
            }
            var dtTable = SQL.GetDataTable($"select * from DEF_VELIKULLANICI {sb.ToString()}");//original where state =1

            CultureInfo cultures = new CultureInfo("en-US");
            if (dtTable.Rows.Count > 0)
            {

                List<VeliKullanici.DEF_VELIKULLANICI> iDefGenel = new List<VeliKullanici.DEF_VELIKULLANICI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    VeliKullanici.DEF_VELIKULLANICI def = new VeliKullanici.DEF_VELIKULLANICI
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),

                        NAME = Utility.Nvl(dtTable.Rows[i]["NAME"]),
                        SURNAME = Utility.Nvl(dtTable.Rows[i]["SURNAME"], ""),
                        KIMLIKNO = Utility.Nvl(dtTable.Rows[i]["KIMLIKNO"], ""),
                        USERNAME = Utility.Nvl(dtTable.Rows[i]["USERNAME"], ""),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"], ""),
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
        public JsonResult getVeliKullanici(string pToken, string PKID = "")
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



                getVeliKullaniciData tumveriler = new getVeliKullaniciData
                {
                    DEF_VELI_KULLANICI = DefVeliKullanici(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veli Listesi Alınırken Hata Oluştu => " + e.Message });
            }

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddVeliKullanici([FromBody] FormCollection collection, string pToken, string pUSERID)
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

                var dbOp = new ImzaData.Ops { _TableName = "DEF_VELIKULLANICI" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_DEF_VELIKULLANICI_ID"));
                var temp = Utility.Nvl(collection["PASSWORD"]);
                if (!string.IsNullOrEmpty(temp))
                {
                    temp = WebTools.Classes.ImzaBase.ImzaCrypt.Cryptation.Crypt(temp);
                    collection["PASSWORD"] = temp;
                }
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID, "ID"), "Yeni kayıt başarıyla tamamlandı.");
                // var editState = SQL.ExecuteScalar($"update AUTH_USERS SET STATE=1 where ID={ID}");
                if (dbOp.success)
                {



                    return Json(new { data = ID, success = true, status = 999, statusText = "Veli Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veli Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veli Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        [System.Web.Mvc.HttpPost]
        public JsonResult EditVeliKullanici([FromBody] FormCollection collection, string pToken, string pUSERID, string PKID)
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
                var dtTable = SQL.GetDataTable($"select * from DEF_VELIKULLANICI {sb.ToString()}");
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


                    var vdbOp = new ImzaData.Ops { _TableName = "DEF_VELIKULLANICI" };
                    var temp = Utility.Nvl(collection["PASSWORD"]);
                    if (!string.IsNullOrEmpty(temp))
                    {
                        temp = WebTools.Classes.ImzaBase.ImzaCrypt.Cryptation.Crypt(temp);
                        collection["PASSWORD"] = temp;
                    }

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, "DEF_VELIKULLANICI", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Veli Bilgisi Başarıyla Güncellendi." });
                    }
                    else
                    {
                        if (vdbOp.error)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Veli Kaydedilirken Hata Oluştu" });
                        }
                        else
                        {
                            return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                        }
                    }


                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"{PKID} ID nolu DATA Onaylandığı için düzenleme yapılamaz." });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DeleteVeliKullanici(string pToken, string pVELIID)
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


                var dtUser = SQL.GetDataTable($"select * from DEF_VELIKULLANICI where ID = {pVELIID} ");

                if (dtUser.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Kullanıcı bulunamadığı için işlem iptal edildi." });
                }


                var dtable = SQL.GetDataTable($" select * from DEF_VELIKULLANICI where ID={pVELIID}");




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

                    var x = SQL.ExecuteNonQuery($"delete from DEF_VELIKULLANICI where  ID={pVELIID} ");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        SQL.ExecuteNonQuery($"delete from DEF_VELIKULLANICI_OGRENCILERI where  VELIID={pVELIID} ");

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla VELİ Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "VELİ Kaldırılırken Hata Oluştu" });

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

        #region Ekli-ekli olmayan ogrenci listeleme
        public List<PersonelTanim.PERSONEL_TANIM> DefPersonelTanim(string pVELIID, string pLISTESTATE)//state 1 ise ekli olanlar değilse ekli olmayanlar
        {
            StringBuilder sb = new StringBuilder();

            var dtTable = new DataTable();
            if (pLISTESTATE == "1")
            {
                dtTable = SQL.GetDataTable($"select * from viewPERSONEL_TANIM WHERE PERSONELTURUID=1 and STATE=1 AND ID IN(SELECT OGRENCIID FROM DEF_VELIKULLANICI_OGRENCILERI WHERE VELIID={pVELIID}) ");
            }
            else
            {
                dtTable = SQL.GetDataTable($"select * from viewPERSONEL_TANIM WHERE PERSONELTURUID=1 and STATE=1 AND ID NOT IN(SELECT OGRENCIID FROM DEF_VELIKULLANICI_OGRENCILERI WHERE VELIID={pVELIID}) ");
            }


            if (dtTable.Rows.Count > 0)
            {

                List<PersonelTanim.PERSONEL_TANIM> iDefGenel = new List<PersonelTanim.PERSONEL_TANIM>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    PersonelTanim.PERSONEL_TANIM def = new PersonelTanim.PERSONEL_TANIM
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        SERVISARACPLAKA = Utility.Nvl(dtTable.Rows[i]["SERVISARACPLAKA"]),
                        OKUL = Utility.Nvl(dtTable.Rows[i]["OKUL"]),
                        OKULTURU = Utility.Nvl(dtTable.Rows[i]["OKULTURU"]),
                        PERSONELTURU = Utility.Nvl(dtTable.Rows[i]["PERSONELTURU"]),
                        GRUP1ADI = Utility.Nvl(dtTable.Rows[i]["GRUP1ADI"]),
                        GRUP2ADI = Utility.Nvl(dtTable.Rows[i]["GRUP2ADI"]),
                        GRUP3ADI = Utility.Nvl(dtTable.Rows[i]["GRUP3ADI"]),
                        GRUP4ADI = Utility.Nvl(dtTable.Rows[i]["GRUP4ADI"]),
                        GRUP5ADI = Utility.Nvl(dtTable.Rows[i]["GRUP5ADI"]),
                        GRUP6ADI = Utility.Nvl(dtTable.Rows[i]["GRUP6ADI"]),
                        DONEM = Utility.Nvl(dtTable.Rows[i]["DONEM"]),
                        ANNEMESLEK = Utility.Nvl(dtTable.Rows[i]["ANNEMESLEK"]),
                        BABAMESLEK = Utility.Nvl(dtTable.Rows[i]["BABAMESLEK"]),
                        KURUM = Utility.Nvl(dtTable.Rows[i]["KURUM"]),
                        SERVISDONEMI = Utility.Nvl(dtTable.Rows[i]["SERVISDONEMI"]),
                        KANGRUBU = Utility.Nvl(dtTable.Rows[i]["KANGRUBU"]),
                        SUBE = Utility.Nvl(dtTable.Rows[i]["SUBE"]),
                        VERGIDAIRE = Utility.Nvl(dtTable.Rows[i]["VERGIDAIRE"]),
                        BANKA = Utility.Nvl(dtTable.Rows[i]["BANKA"]),
                        SICILNO = Utility.Nvl(dtTable.Rows[i]["SICILNO"]),
                        KIMLIKNO = Utility.Nvl(dtTable.Rows[i]["KIMLIKNO"]),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                        SOYADI = Utility.Nvl(dtTable.Rows[i]["SOYADI"]),
                        ADISOYADI = Utility.Nvl(dtTable.Rows[i]["ADI"]) + " " + Utility.Nvl(dtTable.Rows[i]["SOYADI"]),
                        DOGUMTARIH = Utility.Nvl(dtTable.Rows[i]["DOGUMTARIH"]),
                        DOGUMYERI = Utility.Nvl(dtTable.Rows[i]["DOGUMYERI"]),
                        TELEFON1 = Utility.Nvl(dtTable.Rows[i]["TELEFON1"]),
                        TELEFON2 = Utility.Nvl(dtTable.Rows[i]["TELEFON2"]),
                        ANNEADISOYADI = Utility.Nvl(dtTable.Rows[i]["ANNEADISOYADI"]),
                        BABAADISOYADI = Utility.Nvl(dtTable.Rows[i]["BABAADISOYADI"]),
                        ANNETELEFON1 = Utility.Nvl(dtTable.Rows[i]["ANNETELEFON1"]),
                        BABATELEFON1 = Utility.Nvl(dtTable.Rows[i]["BABATELEFON1"]),
                        ANNETELEFON2 = Utility.Nvl(dtTable.Rows[i]["ANNETELEFON2"]),
                        BABATELEFON2 = Utility.Nvl(dtTable.Rows[i]["BABATELEFON2"]),
                        ANNEMESLEKID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ANNEMESLEKID"], "0")),
                        BABAMESLEKID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BABAMESLEKID"], "0")),
                        GIRISTARIH = Utility.Nvl(dtTable.Rows[i]["GIRISTARIH"]),
                        CIKISTARIH = Utility.Nvl(dtTable.Rows[i]["CIKISTARIH"]),
                        CIKISTARIHUYARI = Utility.Nvl(dtTable.Rows[i]["CIKISTARIHUYARI"]),
                        SERVISARACID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SERVISARACID"], "0")),
                        OKULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["OKULID"], "0")),
                        OKULTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["OKULTURUID"], "0")),
                        KURUMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["KURUMID"], "0")),
                        SERVISDONEMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SERVISDONEMID"], "0")),
                        KANGRUBUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["KANGRUBUID"], "0")),
                        OGRENCISUBEID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["OGRENCISUBEID"], "0")),
                        PERSONELTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["PERSONELTURUID"], "0")),
                        IL = Utility.Nvl(dtTable.Rows[i]["IL"]),
                        ILCE = Utility.Nvl(dtTable.Rows[i]["ILCE"]),
                        ADRES = Utility.Nvl(dtTable.Rows[i]["ADRES"]),
                        ADRESCADDE = Utility.Nvl(dtTable.Rows[i]["ADRESCADDE"]),
                        ADRESSOKAK = Utility.Nvl(dtTable.Rows[i]["ADRESSOKAK"]),
                        ADRESMAHALLE = Utility.Nvl(dtTable.Rows[i]["ADRESMAHALLE"]),
                        GRUP1 = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP1"], "0")),
                        GRUP2 = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP2"], "0")),
                        GRUP3 = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP3"], "0")),
                        GRUP4 = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP4"], "0")),
                        GRUP5 = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP5"], "0")),
                        GRUP6 = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP6"], "0")),
                        BILGI = Utility.Nvl(dtTable.Rows[i]["BILGI"]),
                        DONEMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["DONEMID"], "0")),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        VERGINO = Utility.Nvl(dtTable.Rows[i]["VERGINO"]),
                        VERGIDAIREID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["VERGIDAIREID"], "0")),
                        BANKAID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BANKAID"], "0")),
                        HESAPNO = Utility.Nvl(dtTable.Rows[i]["HESAPNO"]),
                        IBAN = Utility.Nvl(dtTable.Rows[i]["IBAN"]),
                        FAX = Utility.Nvl(dtTable.Rows[i]["FAX"]),
                        EMAIL = Utility.Nvl(dtTable.Rows[i]["EMAIL"]),
                        WEBSITESI = Utility.Nvl(dtTable.Rows[i]["WEBSITESI"]),
                        YETKILIADISOYADI = Utility.Nvl(dtTable.Rows[i]["YETKILIADISOYADI"]),
                        YETKILIUNVANI = Utility.Nvl(dtTable.Rows[i]["YETKILIUNVANI"]),
                        YETKILITELEFON = Utility.Nvl(dtTable.Rows[i]["YETKILITELEFON"]),
                        SIRANO = Utility.Nvl(dtTable.Rows[i]["SIRANO"]),
                        SAAT = Utility.Nvl(dtTable.Rows[i]["SAAT"]),
                        OGRETMENADI = Utility.Nvl(dtTable.Rows[i]["OGRETMENADI"]),
                        OGRETMENTELEFONU = Utility.Nvl(dtTable.Rows[i]["OGRETMENTELEFONU"]),
                        OKULNO = Utility.Nvl(dtTable.Rows[i]["OKULNO"]),
                        ANNEMAIL = Utility.Nvl(dtTable.Rows[i]["ANNEMAIL"]),
                        BABAMAIL = Utility.Nvl(dtTable.Rows[i]["BABAMAIL"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getVeliKullaniciOgrenci(string pToken, string pVELIID, string pLISTESTATE)
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

                getPersonelTanimData tumveriler = new getPersonelTanimData
                {
                    PERSONEL_TANIM = DefPersonelTanim(pVELIID, pLISTESTATE),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Öğrenci Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region Veli Ogrenci Ekleme/Kaldırma

        public JsonResult VeliOgrenciEkleme(string pToken, string pUSERID, string pOGRENCIID,
           string pVELIID)
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


                var dtVeli = SQL.GetDataTable($"select * from DEF_VELIKULLANICI where ID = {pVELIID} ");

                if (dtVeli.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "VELİ bulunamadığı için işlem iptal edildi." });
                }
                var dtOGRENCI = SQL.GetDataTable($"select * from PERSONEL_TANIM where ID = {pOGRENCIID} ");

                if (dtOGRENCI.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "İlgili Öğrenci bulunamadığı için işlem iptal edildi." });
                }
                var dtable = SQL.GetDataTable($" select * from DEF_VELIKULLANICI_OGRENCILERI where VELIID={pVELIID} and OGRENCIID={pOGRENCIID}");
                if (dtable.Rows.Count > 0)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Öğrenci zaten veliye tanımlı" });
                }




                if (dtVeli.Rows.Count == 1 && dtOGRENCI.Rows.Count == 1)
                {
                    //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server, Utility.Nvl(dtTable.Rows[0]["filePERSONELRESIM"]));
                    //collection["fileSOSYALGUVENLIK"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOSYALGUVENLIK", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOSYALGUVENLIK"]));
                    //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKIMLIKTURU"]));
                    //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileCALISMAIZNI"]));
                    //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileVCA"]));
                    //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileSOZLESME"]));
                    //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server, Utility.Nvl(dtTable.Rows[0]["fileKVK_WKA"]));




                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    var SQE_ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_DEF_VELIKULLANICI_OGRENCILERI_ID"));
                    var a = $"INSERT INTO DEF_VELIKULLANICI_OGRENCILERI VALUES({SQE_ID},{pVELIID},{pOGRENCIID}," +
                            $"{pUSERID},CONVERT(DATETIME,'{DateTime.Now}',104),1,1)";

                    var x = SQL.ExecuteNonQuery($"INSERT INTO DEF_VELIKULLANICI_OGRENCILERI VALUES({SQE_ID},{pVELIID},{pOGRENCIID}," +
                        $"{pUSERID},CONVERT(DATETIME,'{DateTime.Now}',104),1,1)");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Öğrenci Başarıyla Eklendi " });

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

        public JsonResult VeliOgrenciKaldirma(string pToken, string pUSERID, string pOGRENCIID,
            string pVELIID)
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


                var dtVeli = SQL.GetDataTable($"select * from DEF_VELIKULLANICI where ID = {pVELIID} ");

                if (dtVeli.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "VELİ bulunamadığı için işlem iptal edildi." });
                }
                var dtOGRENCI = SQL.GetDataTable($"select * from PERSONEL_TANIM where ID = {pOGRENCIID} ");

                if (dtOGRENCI.Rows.Count != 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "İlgili Öğrenci bulunamadığı için işlem iptal edildi." });
                }
                var dtable = SQL.GetDataTable($" select * from DEF_VELIKULLANICI_OGRENCILERI where VELIID={pVELIID} and OGRENCIID={pOGRENCIID}");




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

                    var x = SQL.ExecuteNonQuery($"delete from DEF_VELIKULLANICI_OGRENCILERI where  VELIID={pVELIID} and OGRENCIID={pOGRENCIID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Öğrenci Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Öğrenci Kaldırılırken Hata Oluştu" });

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

        #endregion

        #endregion

        #region Cari Hesaplar
        public List<PersonelTanimCari.CariHesaplar> DefCariHesaplar(string PKID = "", string pSTATE = "", string pOGRENCISTATE = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }

            if (pOGRENCISTATE == "1")
            {
                sb.Append(" and PERSONELTURUID!=6");
            }
            else if (pOGRENCISTATE == "0")
            {
                sb.Append(" and PERSONELTURUID!=1");
            }

            var dtTable = SQL.GetDataTable($"select * from viewCariHesaplar  where STATE = {pSTATE} {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<PersonelTanimCari.CariHesaplar> iDefGenel = new List<PersonelTanimCari.CariHesaplar>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    PersonelTanimCari.CariHesaplar def = new PersonelTanimCari.CariHesaplar
                    {
                        SEC = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["SEC"], "0")),
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),

                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                        VERGINO = Utility.Nvl(dtTable.Rows[i]["VERGINO"]),
                        VERGIDAIREID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["VERGIDAIREID"], "0")),
                        VADESIGECMISODEMEVAR = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["VADESIGECMISODEMEVAR"], "0")),
                        BANKA = Utility.Nvl(dtTable.Rows[i]["BANKA"]),
                        VERGIDAIRE = Utility.Nvl(dtTable.Rows[i]["VERGIDAIRE"]),
                        BORC = float.Parse(Utility.Nvl(dtTable.Rows[i]["BORC"], "0")),
                        ALACAK = float.Parse(Utility.Nvl(dtTable.Rows[i]["ALACAK"], "0")),
                        BAKIYETUTAR = float.Parse(Utility.Nvl(dtTable.Rows[i]["BAKIYETUTAR"], "0")),
                        PERSONELMAAS = float.Parse(Utility.Nvl(dtTable.Rows[i]["PERSONELMAAS"], "0")),
                        BAKIYEDURUMADI = Utility.Nvl(dtTable.Rows[i]["BAKIYEDURUMADI"]),
                        BAKIYEDURUMUBIRLESIK = Utility.Nvl(dtTable.Rows[i]["BAKIYEDURUMUBIRLESIK"]),
                        BANKAID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BANKAID"], "0")),
                        HESAPNO = Utility.Nvl(dtTable.Rows[i]["HESAPNO"]),
                        IBAN = Utility.Nvl(dtTable.Rows[i]["IBAN"]),
                        FAX = Utility.Nvl(dtTable.Rows[i]["FAX"]),
                        EMAIL = Utility.Nvl(dtTable.Rows[i]["EMAIL"]),
                        WEBSITESI = Utility.Nvl(dtTable.Rows[i]["WEBSITESI"]),
                        YETKILIADISOYADI = Utility.Nvl(dtTable.Rows[i]["YETKILIADISOYADI"]),
                        YETKILIUNVANI = Utility.Nvl(dtTable.Rows[i]["YETKILIUNVANI"]),
                        YETKILITELEFON = Utility.Nvl(dtTable.Rows[i]["YETKILITELEFON"]),
                        IL = Utility.Nvl(dtTable.Rows[i]["IL"]),
                        ILCE = Utility.Nvl(dtTable.Rows[i]["ILCE"]),
                        ADRES = Utility.Nvl(dtTable.Rows[i]["ADRES"]),
                        TELEFON1 = Utility.Nvl(dtTable.Rows[i]["TELEFON1"]),
                        GRUP1 = Utility.Nvl(dtTable.Rows[i]["GRUP1"]),
                        GRUP2 = Utility.Nvl(dtTable.Rows[i]["GRUP2"]),
                        GRUP3 = Utility.Nvl(dtTable.Rows[i]["GRUP3"]),
                        GRUP4 = Utility.Nvl(dtTable.Rows[i]["GRUP4"]),
                        GRUP5 = Utility.Nvl(dtTable.Rows[i]["GRUP5"]),
                        GRUP6 = Utility.Nvl(dtTable.Rows[i]["GRUP6"]),
                        GRUP1ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP1ID"], "0")),
                        GRUP2ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP2ID"], "0")),
                        GRUP3ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP3ID"], "0")),
                        GRUP4ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP4ID"], "0")),
                        GRUP5ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP5ID"], "0")),
                        GRUP6ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GRUP6ID"], "0")),
                        PERSONELTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["PERSONELTURUID"], "0")),
                        PERSONELTURU = Utility.Nvl(dtTable.Rows[i]["PERSONELTURU"]),
                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
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

        public JsonResult getCariHesaplar(string pToken, string PKID = "", string pSTATE = "1", string pOGRENCISTATE = "")
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

                getPersonelTanimCariData tumveriler = new getPersonelTanimCariData
                {
                    CariHesaplar = DefCariHesaplar(PKID, pSTATE, pOGRENCISTATE),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Cari Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

        #region Faturalar Gelir/Gider İşlemleri

        #region Muhasebe Fatura

        public List<MuhasebeFatura.MUHASEBE_FATURA> DefMuhasebeFatura(string pFATURATURU = "", string pFORMTURU = "", string PKID = "", string pBORCALAKID = "",
            string fCARIHESAPTURU = "", string fCARIHESAP = "", string fKATEGORI = "", string fOKUL = "", string fSERVISARAC = "", string fVADETARIHBAS = "",
            string fVADETARIHBIT = "", string fFATURATARIHBAS = "", string fFATURATARIHBIT = "", string fISLEMTARIHBAS = "", string fISLEMTARIHBIT = "")//fatura türü 1 ise satış faturası 2 ise alış faturası
        {

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }

            if (pFATURATURU == "1")
            {
                sb.Append($" and BORCALACAKID=1 and FORMTURUID=1");
            }

            if (pFATURATURU == "2")
            {
                sb.Append($" and BORCALACAKID=2 and FORMTURUID=2");

            }

            if (!string.IsNullOrEmpty(pFORMTURU))
            {
                sb.Append($" and FORMTURUID IN({pFORMTURU})");
            }

            if (!string.IsNullOrEmpty(pBORCALAKID))
            {
                sb.Append($" and BORCALACAKID IN({pBORCALAKID})");

            }
            #region Filter


            if (!string.IsNullOrEmpty(fCARIHESAP))
            {
                sb.Append($" and CARIID in ({fCARIHESAP})");
            }
            if (!string.IsNullOrEmpty(fKATEGORI))
            {
                sb.Append($" and KATEGORIID in ({fKATEGORI})");
            }
            if (!string.IsNullOrEmpty(fOKUL))
            {
                sb.Append($" and OKULID in ({fOKUL})");
            }
            if (!string.IsNullOrEmpty(fSERVISARAC))
            {
                sb.Append($" and SERVISARACID in ({fSERVISARAC})");
            }
            if (!string.IsNullOrEmpty(fCARIHESAPTURU))
            {
                sb.Append($" and PERSONELTURUID in ({fCARIHESAPTURU})");
            }

            if (!string.IsNullOrEmpty(fVADETARIHBAS) && !string.IsNullOrEmpty(fVADETARIHBIT))
            {
                sb.Append($" AND VADETARIHI BETWEEN CONVERT(DATE,'{fVADETARIHBAS}',104) AND CONVERT(DATE,'{fVADETARIHBIT}',104)");
            }
            if (!string.IsNullOrEmpty(fFATURATARIHBAS) && !string.IsNullOrEmpty(fFATURATARIHBIT))
            {
                sb.Append($" AND FATURATARIHI BETWEEN CONVERT(DATE,'{fFATURATARIHBAS}',104) AND CONVERT(DATE,'{fFATURATARIHBIT}',104)");
            }
            if (!string.IsNullOrEmpty(fISLEMTARIHBAS) && !string.IsNullOrEmpty(fISLEMTARIHBIT))
            {
                sb.Append($" AND ISLEMTARIHI BETWEEN CONVERT(DATE,'{fISLEMTARIHBAS}',104) AND CONVERT(DATE,'{fISLEMTARIHBIT}',104)");
            }

            #endregion




            var donem = Utility.Nvl(SQL.ExecuteScalar("select TOP 1 ID from DEF_DONEM where VARSAYILAN=1"));
            if (!string.IsNullOrEmpty(donem))
            {
                sb.Append($" AND DONEMID={donem}");
            }
            var dtTable = SQL.GetDataTable($"select * from viewMuhasebe where STATE = 1 {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<MuhasebeFatura.MUHASEBE_FATURA> iDefGenel = new List<MuhasebeFatura.MUHASEBE_FATURA>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    MuhasebeFatura.MUHASEBE_FATURA def = new MuhasebeFatura.MUHASEBE_FATURA
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),

                        CARI = Utility.Nvl(dtTable.Rows[i]["CARI"]),
                        GELIRTURU = Utility.Nvl(dtTable.Rows[i]["GELIRTURU"]),
                        GIDERTURU = Utility.Nvl(dtTable.Rows[i]["GIDERTURU"]),
                        PERSONELTURU = Utility.Nvl(dtTable.Rows[i]["PERSONELTURU"]),
                        PERSONELTURUID = Utility.Nvl(dtTable.Rows[i]["PERSONELTURUID"]),

                        GELIRTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GELIRTURUID"], "0")),
                        GIDERTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["GIDERTURUID"], "0")),
                        KATEGORI = Utility.Nvl(dtTable.Rows[i]["KATEGORI"]),
                        FATURAADEDI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["FATURAADEDI"], "0")),
                        BORC = float.Parse(Utility.Nvl(dtTable.Rows[i]["BORC"], "0")),
                        ALACAK = float.Parse(Utility.Nvl(dtTable.Rows[i]["ALACAK"], "0")),
                        CARIID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["CARIID"], "0")),
                        KATEGORIID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["KATEGORIID"], "0")),
                        FATURAMUHATABI = Utility.Nvl(dtTable.Rows[i]["FATURAMUHATABI"]),
                        ISLEMTARIHI = Utility.Nvl(dtTable.Rows[i]["ISLEMTARIHI"]),
                        FATURASERINO = Utility.Nvl(dtTable.Rows[i]["FATURASERINO"]),
                        FATURANO = Utility.Nvl(dtTable.Rows[i]["FATURANO"]),
                        FATURATARIHI = Utility.Nvl(dtTable.Rows[i]["FATURATARIHI"]),
                        VADETARIHI = Utility.Nvl(dtTable.Rows[i]["VADETARIHI"]),
                        BANKAID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BANKAID"], "0")),
                        BANKAODEMEIBANNO = Utility.Nvl(dtTable.Rows[i]["BANKAODEMEIBANNO"]),
                        BANKAODEMEHESAPNO = Utility.Nvl(dtTable.Rows[i]["BANKAODEMEHESAPNO"]),
                        ISODENDI = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["ISODENDI"], "0")),
                        ODEMEID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ODEMEID"], "0")),
                        BORCALACAKID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BORCALACAKID"], "0")),
                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
                        FORMTURU = Utility.Nvl(dtTable.Rows[i]["FORMTURU"]),
                        FORMTURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["FORMTURUID"], "0")),
                        ODEMETURU = Utility.Nvl(dtTable.Rows[i]["ODEMETURU"]),
                        ODEMETURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ODEMETURUID"], "0")),
                        KREDIKARTI = Utility.Nvl(dtTable.Rows[i]["KREDIKARTI"]),
                        KREDIKARTISAHIBI = Utility.Nvl(dtTable.Rows[i]["KREDIKARTISAHIBI"]),
                        KREDIKARTIBANKA = Utility.Nvl(dtTable.Rows[i]["KREDIKARTIBANKA"]),
                        KREDIKARTITARIH = Utility.Nvl(dtTable.Rows[i]["KREDIKARTITARIH"]),
                        BANKAADI = Utility.Nvl(dtTable.Rows[i]["BANKAADI"]),
                        BANKAGONDEREN = Utility.Nvl(dtTable.Rows[i]["BANKAGONDEREN"]),
                        BANKAHESAPNO = Utility.Nvl(dtTable.Rows[i]["BANKAHESAPNO"]),
                        BANKATARIH = Utility.Nvl(dtTable.Rows[i]["BANKATARIH"]),
                        OKUL = Utility.Nvl(dtTable.Rows[i]["OKUL"]),
                        OKULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["OKULID"], "0")),
                        SERVISARAC = Utility.Nvl(dtTable.Rows[i]["SERVISARAC"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        SERVISARACID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SERVISARACID"], "0")),
                        DONEMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["DONEMID"], "0")),
                        ILISKILICARIID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ILISKILICARIID"], "0")),
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
        public JsonResult getMuhasebeFatura(string pToken, string pFATURATURU = "", string pFORMTURU = "", string PKID = "", string pBORCALACAKID = "",
            string fCARIHESAPTURU = "", string fCARIHESAP = "", string fKATEGORI = "", string fOKUL = "", string fSERVISARAC = "", string fVADETARIHBAS = "",
            string fVADETARIHBIT = "", string fFATURATARIHBAS = "", string fFATURATARIHBIT = "", string fISLEMTARIHBAS = "", string fISLEMTARIHBIT = "")
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

                getMuhasebeFaturaData tumveriler = new getMuhasebeFaturaData
                {
                    MUHASEBE_FATURA = DefMuhasebeFatura(pFATURATURU, pFORMTURU, PKID, pBORCALACAKID, fCARIHESAPTURU, fCARIHESAP, fKATEGORI, fOKUL, fSERVISARAC, fVADETARIHBAS, fVADETARIHBIT, fFATURATARIHBAS, fFATURATARIHBIT, fISLEMTARIHBAS, fISLEMTARIHBIT),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Fatura Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DeleteMuhasebeFatura(string pToken, string PKID)
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


                var dtMuhasebe = SQL.GetDataTable($"select * from MUHASEBE_FATURA where ID = {PKID} ");

                if (dtMuhasebe.Rows.Count < 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Muhasebe İşlemi bulunamadığı için işlem iptal edildi." });
                }

                var dtable = SQL.GetDataTable($" select * from MUHASEBE_FATURA where ID={PKID}");




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

                    var x = SQL.ExecuteNonQuery($"delete from MUHASEBE_FATURA where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        SQL.ExecuteNonQuery($"delete from MUHASEBE_FATURADETAY where  FKID={PKID}");


                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Fatura Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Fatura Kaldırılırken Hata Oluştu" });

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

        #region Muhasebe Fatura Detay

        public List<MuhasebeFaturaDetay.MUHASEBE_FATURADETAY> DefMuhasebeFaturaDetay(string pFKID, string PKID = "")
        {

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }


            var dtTable = SQL.GetDataTable($"select * from viewMUHASEBE_FATURADETAY where STATE = 1 and FKID={pFKID} {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<MuhasebeFaturaDetay.MUHASEBE_FATURADETAY> iDefGenel = new List<MuhasebeFaturaDetay.MUHASEBE_FATURADETAY>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    MuhasebeFaturaDetay.MUHASEBE_FATURADETAY def = new MuhasebeFaturaDetay.MUHASEBE_FATURADETAY
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        FKID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["FKID"], "0")),
                        CARIID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["CARIID"], "0")),
                        KDVORAN = float.Parse(Utility.Nvl(dtTable.Rows[i]["KDVORAN"], "0")),
                        KDVTUTAR = float.Parse(Utility.Nvl(dtTable.Rows[i]["KDVTUTAR"], "0")),
                        ISKONTOORAN = float.Parse(Utility.Nvl(dtTable.Rows[i]["ISKONTOORAN"], "0")),
                        ISKONTOTUTAR = float.Parse(Utility.Nvl(dtTable.Rows[i]["ISKONTOTUTAR"], "0")),
                        URUNADI = Utility.Nvl(dtTable.Rows[i]["URUNADI"]),
                        URUNHIZMETID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["URUNHIZMETID"], "0")),
                        MIKTAR = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MIKTAR"], "0")),
                        VERGISIZTOPLAM = float.Parse(Utility.Nvl(dtTable.Rows[i]["VERGISIZTOPLAM"], "0")),
                        FIYAT = float.Parse(Utility.Nvl(dtTable.Rows[i]["FIYAT"], "0")),
                        TOPLAMTUTAR = float.Parse(Utility.Nvl(dtTable.Rows[i]["TOPLAMTUTAR"], "0")),
                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
                        BORC = float.Parse(Utility.Nvl(dtTable.Rows[i]["BORC"], "0")),
                        ALACAK = float.Parse(Utility.Nvl(dtTable.Rows[i]["ALACAK"], "0")),
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
        public JsonResult getMuhasebeFaturaDetay(string pToken, string pFKID, string PKID = "")
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

                getMuhasebeFaturaDetayData tumveriler = new getMuhasebeFaturaDetayData
                {
                    MUHASEBE_FATURADETAY = DefMuhasebeFaturaDetay(pFKID, PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Fatura Detay Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        #endregion


        #endregion

        #region AraçPuantaj

        public List<ServisArac.DEF_PERSONEL_SERVISARAC> DefServisArac(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" where ID={PKID}");
            }
            var dtTable = SQL.GetDataTable($"select * from viewSERVISARAC  {sb.ToString()}");

            if (dtTable.Rows.Count > 0)
            {

                List<ServisArac.DEF_PERSONEL_SERVISARAC> iDefGenel = new List<ServisArac.DEF_PERSONEL_SERVISARAC>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    ServisArac.DEF_PERSONEL_SERVISARAC def = new ServisArac.DEF_PERSONEL_SERVISARAC
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        MARKA = Utility.Nvl(dtTable.Rows[i]["MARKA"]),
                        MODEL = Utility.Nvl(dtTable.Rows[i]["MODEL"]),
                        SOFOR = Utility.Nvl(dtTable.Rows[i]["SOFOR"]),
                        HOSTES = Utility.Nvl(dtTable.Rows[i]["HOSTES"]),
                        ARACTIPIADI = Utility.Nvl(dtTable.Rows[i]["ARACTIPIADI"]),
                        CARIHESAP = Utility.Nvl(dtTable.Rows[i]["CARIHESAP"]),
                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
                        ARACNO = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ARACNO"], "0")),
                        MARKAID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MARKAID"], "0")),
                        MODELID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["MODELID"], "0")),
                        SOFORID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SOFORID"], "0")),
                        HOSTESID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["HOSTESID"], "0")),
                        PLAKA = Utility.Nvl(dtTable.Rows[i]["PLAKA"]),
                        GRUP1 = Utility.Nvl(dtTable.Rows[i]["GRUP1"]),
                        GRUP2 = Utility.Nvl(dtTable.Rows[i]["GRUP2"]),
                        GRUP3 = Utility.Nvl(dtTable.Rows[i]["GRUP3"]),
                        GRUP4 = Utility.Nvl(dtTable.Rows[i]["GRUP4"]),
                        GRUP5 = Utility.Nvl(dtTable.Rows[i]["GRUP5"]),
                        GRUP6 = Utility.Nvl(dtTable.Rows[i]["GRUP6"]),
                        GUNCELKM = Utility.Nvl(dtTable.Rows[i]["GUNCELKM"]),
                        TRAFIGECIKISTARIHI = Utility.Nvl(dtTable.Rows[i]["TRAFIGECIKISTARIHI"]),
                        ARACTIPI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ARACTIPI"], "0")),
                        YAKITCINSI = Utility.Nvl(dtTable.Rows[i]["YAKITCINSI"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        SERVISBASLAMATARIH = Utility.Nvl(dtTable.Rows[i]["SERVISBASLAMATARIH"]),
                        SERVISBITISTARIH = Utility.Nvl(dtTable.Rows[i]["SERVISBITISTARIH"]),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        GUZERGAHADI = Utility.Nvl(dtTable.Rows[i]["GUZERGAHADI"]),
                        CARIHESAPID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["CARIHESAPID"], "0")),
                        PUANTAJ_SERVIS_ISLEMUCRETI = Convert.ToDecimal(Utility.Nvl(dtTable.Rows[i]["PUANTAJ_SERVIS_ISLEMUCRETI"], "0"))

                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getServisAracHakedisOlusturma(string pToken, string PKID = "")
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

                getServisAracData tumveriler = new getServisAracData
                {
                    DEF_PERSONEL_SERVISARAC = DefServisArac(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Servis Araç Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }


        public List<ServisHakedis.DEF_SERVIS_HAKEDIS> DefServisHakedis(string pCARIHESAPID, string PKID = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
            var dtTable = SQL.GetDataTable($"select * from viewDEF_SERVIS_HAKEDIS WHERE CARIHESAPID={pCARIHESAPID}  {sb.ToString()}");

            if (dtTable.Rows.Count > 0)
            {

                List<ServisHakedis.DEF_SERVIS_HAKEDIS> iDefGenel = new List<ServisHakedis.DEF_SERVIS_HAKEDIS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    ServisHakedis.DEF_SERVIS_HAKEDIS def = new ServisHakedis.DEF_SERVIS_HAKEDIS
                    {
                        SEC = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["SEC"], "0")),
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        CARIHESAPID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["CARIHESAPID"], "0")),
                        CARIHESAP = Utility.Nvl(dtTable.Rows[i]["CARIHESAP"]),
                        SERVISARACID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SERVISARACID"], "0")),
                        PLAKA = Utility.Nvl(dtTable.Rows[i]["PLAKA"]),
                        ISLEMADEDI = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ISLEMADEDI"], "0")),
                        ISLEMADETUCRETI = float.Parse(Utility.Nvl(dtTable.Rows[i]["ISLEMADETUCRETI"], "0")),
                        HAKEDISTUTARI = float.Parse(Utility.Nvl(dtTable.Rows[i]["HAKEDISTUTARI"], "0")),
                        TARIH = Utility.Nvl(dtTable.Rows[i]["TARIH"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"]))


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getServisHakedis(string pToken, string pCARIHESAPID, string PKID = "")
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

                getServisHakedisData tumveriler = new getServisHakedisData
                {
                    DEF_SERVIS_HAKEDIS = DefServisHakedis(pCARIHESAPID, PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Servis Araç Hakediş Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public List<ServisHakedis.DEF_SERVIS_HAKEDIS> DefServisAracPuantaj(string pCARIHESAPID, string PKID = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID in ({PKID})");
            }
            var dtTable = SQL.GetDataTable($"select SUM(HAKEDISTUTARI) HAKEDISTUTARI from viewDEF_SERVIS_HAKEDIS WHERE CARIHESAPID={pCARIHESAPID}  {sb.ToString()}");
            var urunHizmetId = Convert.ToInt32(Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='ARACPUANTAJ'")));
            if (urunHizmetId < 1)
            {
                return null;
            }
            if (dtTable.Rows.Count > 0)
            {

                List<ServisHakedis.DEF_SERVIS_HAKEDIS> iDefGenel = new List<ServisHakedis.DEF_SERVIS_HAKEDIS>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    ServisHakedis.DEF_SERVIS_HAKEDIS def = new ServisHakedis.DEF_SERVIS_HAKEDIS
                    {

                        ID = urunHizmetId,

                        HAKEDISTUTARI = float.Parse(Utility.Nvl(dtTable.Rows[i]["HAKEDISTUTARI"], "0")),



                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }

        public JsonResult getServisHakedisFaturaDetay(string pToken, string pCARIHESAPID, string PKID = "")
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

                getServisHakedisData tumveriler = new getServisHakedisData
                {
                    DEF_SERVIS_HAKEDIS = DefServisAracPuantaj(pCARIHESAPID, PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Servis Araç Hakediş Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public List<GenelTanim.DEF_GENEL> DefFaturaMuhatabi(string pUSERID)
        {
            var dtTable = SQL.GetDataTable($"select CONCAT(NAME,' ',SURNAME) ADI,* from AUTH_USERS where USERID={pUSERID}");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelTanim.DEF_GENEL> iDefGenel = new List<GenelTanim.DEF_GENEL>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelTanim.DEF_GENEL def = new GenelTanim.DEF_GENEL
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["USERID"]),

                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getFaturaMuhatabi(string pToken, string PKID)
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
                    DEF_GENEL = DefFaturaMuhatabi(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Fatura Muhatabı Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getServisAracPuantajKategori(string pToken)
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

                var pTableName = "DEF_MUHASEBE_FATURAKATEGORI";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='SERVISARACFATURAKATEGORI'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getServisPuantajUrunHizmet(string pToken)
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

                var pTableName = "DEF_MUHASEBE_HIZMETURUN";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='ARACPUANTAJ'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        //select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='ARACPUANTAJ'

        //public JsonResult getServisUcreti(string pToken, string PKID = "")
        //{
        //    try
        //    {
        //        #region Token Kontrolü

        //        var vCheckToken = new iTools.token().CheckToken(pToken);
        //        if (new iTools.token().CheckToken(pToken) != "1")
        //        {
        //            return Json(new { success = false, status = 555, statusText = vCheckToken });
        //        }

        //        #endregion

        //        getServisHakedisData tumveriler = new getServisHakedisData
        //        {
        //            DEF_SERVIS_HAKEDIS = DefServisHakedis(PKID),

        //        };

        //        return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { data = "", success = false, status = 402, statusText = "Servis Araç Hakediş Listesi Alınırken Hata Oluştu => " + e.Message });
        //    }
        //}


        #endregion

        #region PERSONELPUANTAJ

        public JsonResult getPersonelPuantajKategori(string pToken)
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

                var pTableName = "DEF_MUHASEBE_FATURAKATEGORI";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='PERSONELFATURAKATEGORI'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getPersonelPuantajDigerGiderKategori(string pToken)
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

                var pTableName = "DEF_MUHASEBE_FATURAKATEGORI";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='PERSONELDIGERGIDERKATEGORI'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getPersonelPuantajUrunHizmet(string pToken)
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

                var pTableName = "DEF_MUHASEBE_HIZMETURUN";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='PERSONELPUANTAJ'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getPersonelPuantajDigerGiderUrunHizmet(string pToken)
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

                var pTableName = "DEF_MUHASEBE_HIZMETURUN";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='PERSONELDIGERGIDERURUN'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getPersonelDigerGiderCariId(string pToken)
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

                var pTableName = "PERSONEL_TANIM";
                var PKID = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=3 AND DEV_NAME='PERSONELDIGERGIDERCARIID'"));

                getDEF_GENELData tumveriler = new getDEF_GENELData
                {
                    DEF_GENEL = DefTable($"{pTableName}", PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        #endregion





        public ActionResult Index()
        {
            return View();
        }
    }
}