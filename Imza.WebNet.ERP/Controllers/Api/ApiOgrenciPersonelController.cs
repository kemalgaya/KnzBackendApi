using System;
using System.Collections.Generic;
using System.Data;
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
    public class ApiOgrenciPersonelController : Controller
    {
        // GET: ApiOgrenciPersonel


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


        #region PERSONEL_TANIM
        public List<PersonelTanim.PERSONEL_TANIM> DefPersonelTanim(string PKID = "", string pList = "", string pCARI = "", string pLOOK = "",
            string fOKUL = "", string fOKULTURU = "", string fBOLUMSINIF = "", string fARAC = "", string fSERVISDONEMI = "", string fSTATE = "",
            string fGRUP1 = "", string fGRUP2 = "", string fGRUP3 = "", string fGRUP4 = "", string fGRUP5 = "", string fGRUP6 = "",string fSEHIRID="",string fSEHIRILCEID="",string fKAYITTARIHIBAS="", string fKAYITTARIHIBIT="")
        {
            StringBuilder sb = new StringBuilder();


            if (pCARI == "1")
            {
                sb.Append($" and PERSONELTURUID!=1");
            }
            else if (pList=="1")
            {
                sb.Append($" and PERSONELTURUID not in (1,6)");
            }
            else if (pList == "0")
            {
                sb.Append($" and PERSONELTURUID in (1)");
            }
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }

            #region Filter

            if (!string.IsNullOrEmpty(fOKUL))
            {
                sb.Append($" AND OKULID in({fOKUL})");
            }
            if (!string.IsNullOrEmpty(fOKULTURU))
            {
                sb.Append($" AND OKULTURUID in({fOKULTURU})");
            }
            if (!string.IsNullOrEmpty(fBOLUMSINIF))
            {
                sb.Append($" AND OGRENCISUBEID in({fBOLUMSINIF})");
            }
            if (!string.IsNullOrEmpty(fARAC))
            {
                sb.Append($" AND SERVISARACID in({fARAC})");
            }
            if (!string.IsNullOrEmpty(fSERVISDONEMI))
            {
                sb.Append($" AND SERVISDONEMID in({fSERVISDONEMI})");
            }

            if (!string.IsNullOrEmpty(fGRUP1))
            {
                sb.Append($" AND GRUP1 in({fGRUP1})");
            }
            if (!string.IsNullOrEmpty(fGRUP2))
            {
                sb.Append($" AND GRUP2 in({fGRUP2})");
            }
            if (!string.IsNullOrEmpty(fGRUP3))
            {
                sb.Append($" AND GRUP3 in({fGRUP3})");
            }
            if (!string.IsNullOrEmpty(fGRUP4))
            {
                sb.Append($" AND GRUP4 in({fGRUP4})");
            }
            if (!string.IsNullOrEmpty(fGRUP5))
            {
                sb.Append($" AND GRUP5 in({fGRUP5})");
            }
            if (!string.IsNullOrEmpty(fGRUP6))
            {
                sb.Append($" AND GRUP6 in({fGRUP6})");
            }
            if (!string.IsNullOrEmpty(fSEHIRID))
            {
                sb.Append($" AND SEHIRID in({fSEHIRID})");
            }
            if (!string.IsNullOrEmpty(fSEHIRILCEID))
            {
                sb.Append($" AND SEHIRILCEID in({fSEHIRILCEID})");
            }

            if (!string.IsNullOrEmpty(fKAYITTARIHIBAS) && !string.IsNullOrEmpty(fKAYITTARIHIBIT))
            {
                sb.Append($" AND GIRISTARIH BETWEEN CONVERT(DATE,'{fKAYITTARIHIBAS}',104) AND CONVERT(DATE,'{fKAYITTARIHIBIT}',104");
            }

            #endregion

            var donem = Utility.Nvl(SQL.ExecuteScalar("select TOP 1 ID from DEF_DONEM where VARSAYILAN=1"));
            if (!string.IsNullOrEmpty(donem))
            {
                sb.Append($" AND DONEMID={donem}");
            }




            var dtTable = SQL.GetDataTable($"select * from viewPERSONEL_TANIM WHERE STATE={fSTATE}  {sb.ToString()}");



            if (dtTable.Rows.Count > 0)
            {

                List<PersonelTanim.PERSONEL_TANIM> iDefGenel = new List<PersonelTanim.PERSONEL_TANIM>();
                if (pLOOK == "1")
                {
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        PersonelTanim.PERSONEL_TANIM def = new PersonelTanim.PERSONEL_TANIM
                        {
                            ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                            ADISOYADI = Utility.Nvl(dtTable.Rows[i]["ADI"]) + " " +
                                        Utility.Nvl(dtTable.Rows[i]["SOYADI"])

                        };

                        iDefGenel.Add(def);
                    }
                }
                else
                {
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
                            KARNESINIF = Utility.Nvl(dtTable.Rows[i]["KARNESINIF"]),
                            KANGRUBU = Utility.Nvl(dtTable.Rows[i]["KANGRUBU"]),
                            SUBE = Utility.Nvl(dtTable.Rows[i]["SUBE"]),
                            VERGIDAIRE = Utility.Nvl(dtTable.Rows[i]["VERGIDAIRE"]),
                            BANKA = Utility.Nvl(dtTable.Rows[i]["BANKA"]),
                            SICILNO = Utility.Nvl(dtTable.Rows[i]["SICILNO"]),
                            KIMLIKNO = Utility.Nvl(dtTable.Rows[i]["KIMLIKNO"]),
                            ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                            SOYADI = Utility.Nvl(dtTable.Rows[i]["SOYADI"]),
                            ADISOYADI = Utility.Nvl(dtTable.Rows[i]["ADI"]) + " " +
                                        Utility.Nvl(dtTable.Rows[i]["SOYADI"]),
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
                            KARNESINIFID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["KARNESINIFID"], "0")),
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
                            SEHIRID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SEHIRID"], "0")),
                            SEHIRILCEID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SEHIRILCEID"], "0")),
                            SEHIRADI = Utility.Nvl(dtTable.Rows[i]["SEHIRADI"]),
                            SEHIRILCEADI = Utility.Nvl(dtTable.Rows[i]["SEHIRILCEADI"]),
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
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getPersonelTanim(string pToken, string PKID = "", string pList = "", string pCARI = "", string pLOOK = "",
            string fOKUL = "", string fOKULTURU = "", string fBOLUMSINIF = "", string fARAC = "", string fSERVISDONEMI = "", string fSTATE = "",
            string fGRUP1 = "", string fGRUP2 = "", string fGRUP3 = "", string fGRUP4 = "", string fGRUP5 = "", string fGRUP6 = "", string fSEHIRID = "", string fSEHIRILCEID = "", string fKAYITTARIHIBAS = "", string fKAYITTARIHIBIT = "")
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

                if (string.IsNullOrEmpty(fSTATE))
                {
                    fSTATE = "1";
                }
                getPersonelTanimData tumveriler = new getPersonelTanimData
                {
                    PERSONEL_TANIM = DefPersonelTanim(PKID, pList, pCARI, pLOOK, fOKUL, fOKULTURU, fBOLUMSINIF, fARAC, fSERVISDONEMI, fSTATE, fGRUP1, fGRUP2, fGRUP3, fGRUP4, fGRUP5, fGRUP6,fSEHIRID,fSEHIRILCEID,fKAYITTARIHIBAS,fKAYITTARIHIBIT),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeletePersonelTanim(string pToken, string PKID)
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


                var dtMuhasebe = SQL.GetDataTable($"select * from MUHASEBE_FATURA where CARIID = {PKID} ");

                if (dtMuhasebe.Rows.Count >= 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Muhasebe İşlemi olduğu için işlem iptal edildi." });
                }
                var dtMuhasebeDetay = SQL.GetDataTable($"select * from MUHASEBE_FATURADETAY where CARIID = {PKID} ");

                if (dtMuhasebeDetay.Rows.Count >= 1)
                {
                    return Json(new { data = "", success = false, status = 999, statusText = "Muhasebe İşlemi olduğu için işlem iptal edildi." });
                }
                var dtable = SQL.GetDataTable($" select * from PERSONEL_TANIM where ID={PKID}");




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

                    var x = SQL.ExecuteNonQuery($"delete from PERSONEL_TANIM where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        SQL.ExecuteNonQuery($"delete from DEF_VELIKULLANICI_OGRENCILERI where  OGRENCIID={PKID}");
                        var dtDosya = SQL.GetDataTable($"select * from PERSONEL_DOSYAEKI where KISIID={PKID}");
                        if (dtDosya.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtDosya.Rows.Count; i++)
                            {
                                new FormUtils().FileDelete(Utility.Nvl(dtDosya.Rows[0]["DOSYAYOLU"]), Server);
                                SQL.ExecuteNonQuery($"delete from PERSONEL_DOSYAEKI where  KISIID={PKID}");
                            }

                        }

                        return Json(new { data = "", success = true, status = 999, statusText = "Başarıyla Öğrenci/Personel/Cari Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Öğrenci/Personel/Cari Kaldırılırken Hata Oluştu" });

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


        #region Aktarım İşlemleri

        public List<GenelTanim.DEF_GENEL> DefGuncelDonem()
        {
            var dtTable = SQL.GetDataTable($"select TOP 1 ID,ADI from dbo.DEF_DONEM where STATE = 1 and VARSAYILAN=1");


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
        public JsonResult getGuncelDonem(string pToken)
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
                    DEF_GENEL = DefGuncelDonem(),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veri Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        public JsonResult DonemAktarimi(string pToken,string pAktarilacakDonemId,string pAktarilacakOgrenciler)
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

                
                var x = Utility.Nvl(SQL.ExecuteScalar(
                    $"UPDATE PERSONEL_TANIM SET DONEMID={pAktarilacakDonemId} WHERE ID in ({pAktarilacakOgrenciler})"));

              
                    return Json(new { data = "", success = true, status = 999, statusText = "Dönem Aktarımı Başarıyla yapıldı." });
               
                
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Dönem Aktarımı yapılırken Hata oluştur... => " + e.Message });
            }
        }
        public JsonResult ServisAracAktarimi(string pToken, string pAktarilacakArac,string pAktarilacakOgrenciler)
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

                
                var x = Utility.Nvl(SQL.ExecuteScalar(
                    $"UPDATE PERSONEL_TANIM SET SERVISARACID={pAktarilacakArac} WHERE ID in ({pAktarilacakOgrenciler})"));


                return Json(new { data = "", success = true, status = 999, statusText = "Servis Araç Aktarımı Başarıyla yapıldı." });


            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Servis Araç Aktarımı yapılırken Hata oluştur... => " + e.Message });
            }
        }
        #endregion

        #endregion

        #region DEF_PERSONEL_SERVISARAC
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
        public JsonResult getServisArac(string pToken, string PKID = "")
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

        public List<GenelTanim.DEF_GENEL> DefServisAracLook()
        {
            var dtTable = SQL.GetDataTable($"select ID,ARACNO,PLAKA from DEF_PERSONEL_SERVISARAC where STATE = 1");


            if (dtTable.Rows.Count > 0)
            {

                List<GenelTanim.DEF_GENEL> iDefGenel = new List<GenelTanim.DEF_GENEL>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    GenelTanim.DEF_GENEL def = new GenelTanim.DEF_GENEL
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),

                        ADI = Utility.Nvl(dtTable.Rows[i]["ARACNO"]) + " - " + Utility.Nvl(dtTable.Rows[i]["PLAKA"])
                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getServisAracLook(string pToken)
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
                    DEF_GENEL = DefServisAracLook(),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Servis Araç Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region DEF_DONEM
        public List<Donem.DEF_DONEM> DefDonem(string PKID = "", string pSTATE = "1", string pGUNCELDONEM = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }


            var dtTable = SQL.GetDataTable($"select * from DEF_DONEM where STATE={pSTATE} {sb.ToString()}");

            if (pGUNCELDONEM == "1")
            {
                dtTable = SQL.GetDataTable($"select top 1 * from DEF_DONEM where VARSAYILAN=1 order by ID desc  ");
            }
            if (dtTable.Rows.Count > 0)
            {

                List<Donem.DEF_DONEM> iDefGenel = new List<Donem.DEF_DONEM>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Donem.DEF_DONEM def = new Donem.DEF_DONEM
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),

                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        VARSAYILAN = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["VARSAYILAN"])),


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getDonem(string pToken, string PKID = "", string pSTATE = "1", string pGUNCELDONEM = "")
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

                getDonemData tumveriler = new getDonemData
                {
                    DEF_DONEM = DefDonem(PKID, pSTATE, pGUNCELDONEM),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Dönem Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region PersonelFileUpload

        //collection["filePERSONELRESIM"] = new FormUtils().FileUpload("PERSONEL_TANIM", "filePERSONELRESIM", Request, Server);
        public List<PersonelDosyaEki.PERSONEL_DOSYAEKI> DefPersonelDosyaEki(string pKISIID)
        {
            var dtTable = SQL.GetDataTable($"select * from PERSONEL_DOSYAEKI where KISIID={pKISIID}");


            if (dtTable.Rows.Count > 0)
            {

                List<PersonelDosyaEki.PERSONEL_DOSYAEKI> iDefGenel = new List<PersonelDosyaEki.PERSONEL_DOSYAEKI>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    PersonelDosyaEki.PERSONEL_DOSYAEKI def = new PersonelDosyaEki.PERSONEL_DOSYAEKI
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),
                        KISIID = Convert.ToInt32(dtTable.Rows[i]["KISIID"]),
                        EVRAKTURUID = Convert.ToInt32(dtTable.Rows[i]["EVRAKTURUID"]),
                        DOSYAYOLU = Utility.Nvl(dtTable.Rows[i]["DOSYAYOLU"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        TARIHI = Utility.Nvl(dtTable.Rows[i]["TARIHI"]),
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

        public JsonResult getPersonelDosyaEki(string pToken, string pKISIID)
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

                getPersonelDosyaEkiData tumveriler = new getPersonelDosyaEkiData
                {
                    PERSONEL_DOSYAEKI = DefPersonelDosyaEki(pKISIID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Dosya Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddFileOgrenciPersonel([FromBody] FormCollection collection, string pToken, string pUSERID, string pPERSONELID)
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
                collection["DOSYAYOLU"] = new FormUtils().FileUpload("PERSONEL_DOSYAEKI", "DOSYAYOLU", Request, Server);
                if (string.IsNullOrEmpty(Utility.Nvl(collection["DOSYAYOLU"])))
                {
                    return Json(new { data = "", success = false, status = 402, statusText = "Dosya Yolu Boş Ya Da Dosya Sunucuya Gönderilemedi." });
                }
                collection["KISIID"] = pPERSONELID;
                //collection["fileKIMLIKTURU"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKIMLIKTURU", Request, Server);
                //collection["fileCALISMAIZNI"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileCALISMAIZNI", Request, Server);
                //collection["fileVCA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileVCA", Request, Server);
                //collection["fileSOZLESME"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileSOZLESME", Request, Server);
                //collection["fileKVK_WKA"] = new FormUtils().FileUpload("PERSONEL_TANIM", "fileKVK_WKA", Request, Server);
                var dtable = SQL.GetDataTable($"select * from PERSONEL_DOSYAEKI where KISIID not in (select ID from PERSONEL_TANIM) ");
                if (dtable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[i]["DOSYAYOLU"]), Server);
                        SQL.ExecuteNonQuery($"delete from PERSONEL_DOSYAEKI where ID={Utility.Nvl(dtable.Rows[i]["ID"])}");
                    }
                }
                var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_DOSYAEKI" };
                var ID = Utility.Nvl(dbStaticUtils.GetSequenceValue("SQE_PERSONEL_DOSYAEKI_ID"));
                var x = dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1"), "1", false, ID), "Yeni kayıt başarıyla tamamlandı.", "");

                if (dbOp.success)
                {
                    return Json(new { data = ID, success = true, status = 999, statusText = "Dosya Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Dosya Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Dosya Kaydedilirken Hata Oluştu => " + e.Message });
            }

        }
        //select DEV_VALUE from APP_PARAMETERS  where MODULID=1 AND DEV_NAME='FTPKLASOR'
        public JsonResult getPersonelDosyaShow(string pToken, string pDosyaAdi)
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

                var GoruntuAdres =
                    Utility.Nvl(SQL.ExecuteScalar(
                        "select DEV_VALUE from APP_PARAMETERS  where MODULID=1 AND DEV_NAME='GORUNTUADRES'"));
                var Klasor = Utility.Nvl(SQL.ExecuteScalar("select DEV_VALUE from APP_PARAMETERS  where MODULID=1 AND DEV_NAME='FTPKLASOR'"));

                return Json(new { data = $"http://{GoruntuAdres}//{Klasor}//{pDosyaAdi}", success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Dosya Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeleteFileOgrenciPersonel(string pToken, string pPERSONELID, string pDOSYAID)
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

                var dtable = SQL.GetDataTable($"select * from PERSONEL_DOSYAEKI where KISIID ={pPERSONELID} and ID={pDOSYAID}");
                if (dtable.Rows.Count >= 0)
                {
                    new FormUtils().FileDelete(Utility.Nvl(dtable.Rows[0]["DOSYAYOLU"]), Server);

                    var x = SQL.ExecuteNonQuery(
                        $"delete from PERSONEL_DOSYAEKI where  KISIID ={pPERSONELID} and ID={pDOSYAID}");
                    return Json(new { data = "", success = true, status = 999, statusText = "Dosya Başarıyla Silindi." });
                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });

                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Dosya Silinirken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region Personel Uyarı

        public List<PersonelUyarilar.PERSONEL_UYARILAR> DefPersonelUyari(string PKID = "", string pOGRENCIID = "", string pUYARITURUID = "", string pUYARIVERILENID = "", string pPERSONELID = "")
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" and ID={PKID}");
            }
            if (!string.IsNullOrEmpty(pOGRENCIID))
            {
                sb.Append($" and OGRENCIID={pOGRENCIID}");
            }
            if (!string.IsNullOrEmpty(pUYARITURUID))
            {
                sb.Append($" and UYARITURUID={pUYARITURUID}");
            }
            if (!string.IsNullOrEmpty(pUYARIVERILENID))
            {
                sb.Append($" and UYARIVERILENID={pUYARIVERILENID}");
            }
            if (!string.IsNullOrEmpty(pPERSONELID))
            {
                sb.Append($" and PERSONELID={pPERSONELID}");
            }
            var dtTable = SQL.GetDataTable($"select * from viewPersonelUyarilar where STATE=1 {sb.ToString()}");

            if (dtTable.Rows.Count > 0)
            {

                List<PersonelUyarilar.PERSONEL_UYARILAR> iDefGenel = new List<PersonelUyarilar.PERSONEL_UYARILAR>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    PersonelUyarilar.PERSONEL_UYARILAR def = new PersonelUyarilar.PERSONEL_UYARILAR
                    {
                        ID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ID"], "0")),
                        UYARITURUID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["UYARITURUID"], "0")),
                        UYARIVERILENID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["UYARIVERILENID"], "0")),
                        TARIH = Utility.Nvl(dtTable.Rows[i]["TARIH"]),
                        OGRENCIID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["OGRENCIID"], "0")),
                        VELIADISOYADI = Utility.Nvl(dtTable.Rows[i]["VELIADISOYADI"]),
                        VELIYAKINLIK = Utility.Nvl(dtTable.Rows[i]["VELIYAKINLIK"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        UYARITURU = Utility.Nvl(dtTable.Rows[i]["UYARITURU"]),
                        UYARIVERILEN = Utility.Nvl(dtTable.Rows[i]["UYARIVERILEN"]),
                        OGRENCI = Utility.Nvl(dtTable.Rows[i]["OGRENCI"]),
                        PERSONEL = Utility.Nvl(dtTable.Rows[i]["PERSONEL"]),
                        EKLEYEN = Utility.Nvl(dtTable.Rows[i]["EKLEYEN"]),
                        PERSONELID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["PERSONELID"], "0")),

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
        public JsonResult getPersonelUyarilar(string pToken, string PKID = "", string pOGRENCIID = "", string pUYARITURUID = "", string pUYARIVERILENID = "", string pPERSONELID = "")
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

                getPersonelUyariData tumveriler = new getPersonelUyariData
                {
                    PERSONEL_UYARILAR = DefPersonelUyari(PKID, pOGRENCIID, pUYARITURUID, pUYARIVERILENID, pPERSONELID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Personel Uyarı Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }


        #endregion

        #region GUZERGAH

        public List<PersonelGuzergah.PERSONEL_GUZERGAH> DefPersonelGuzergah(string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
            var dtTable = SQL.GetDataTable($"select * from viewPERSONEL_GUZERGAH where STATE = 1 {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<PersonelGuzergah.PERSONEL_GUZERGAH> iDefGenel = new List<PersonelGuzergah.PERSONEL_GUZERGAH>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    PersonelGuzergah.PERSONEL_GUZERGAH def = new PersonelGuzergah.PERSONEL_GUZERGAH
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),
                        ADI = Utility.Nvl(dtTable.Rows[i]["ADI"]),
                        SERVISARAC = Utility.Nvl(dtTable.Rows[i]["SERVISARAC"]),
                        SOFOR = Utility.Nvl(dtTable.Rows[i]["SOFOR"]),
                        HOSTES = Utility.Nvl(dtTable.Rows[i]["HOSTES"]),
                        DONEM = Utility.Nvl(dtTable.Rows[i]["DONEM"]),
                        OKUL = Utility.Nvl(dtTable.Rows[i]["OKUL"]),
                        ARACID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["ARACID"], "0")),
                        SOFORID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SOFORID"], "0")),
                        HOSTESID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["HOSTESID"], "0")),
                        TARIHI = Utility.Nvl(dtTable.Rows[i]["TARIHI"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        DONEMID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["DONEMID"], "0")),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0")),
                        OKULID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["OKULID"], "0"))


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getPersonelGuzergah(string pToken, string PKID = "")
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

                getPersonelGuzergahData tumveriler = new getPersonelGuzergahData
                {
                    PERSONEL_GUZERGAH = DefPersonelGuzergah(PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Güzergah Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult getPersonelGuzergahAll(string pToken)
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

                getAllGuzergahData tumveriler = new getAllGuzergahData
                {
                    PERSONEL_GUZERGAH = DefPersonelGuzergah(),
                    PERSONEL_GUZERGAHDETAY = DefPersonelGuzergahDetay(),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Güzergah Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        public JsonResult DeletePersonelGuzergah(string pToken, string PKID)
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


                var dtable = SQL.GetDataTable($"select * from PERSONEL_GUZERGAH where ID = {PKID} ");


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

                    var x = SQL.ExecuteNonQuery($"delete from PERSONEL_GUZERGAH where  ID={PKID}");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (x == "1")
                    {
                        SQL.ExecuteNonQuery($"delete from PERSONEL_GUZERGAHDETAY where  FKID={PKID}");

                        return Json(new { data = "", success = true, status = 999, statusText = "Güzergah Kaldırıldı" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 999, statusText = "Silinirken Hata oluştu" });

                    }




                }
                else
                {

                    return Json(new { data = "", success = false, status = 401, statusText = $"Seçili Güzergah Bulunamadı" });
                }

            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = " Kaydedilirken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region GUZERGAHDETAY
        public List<PersonelGuzergahDetay.PERSONEL_GUZERGAHDETAY> DefPersonelGuzergahDetay(string FKID="", string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
            if (!string.IsNullOrEmpty(FKID))
            {
                sb.Append($" and FKID={FKID}");
            }
            var dtTable = SQL.GetDataTable($"select * from viewPERSONEL_GUZERGAHDETAY where STATE = 1  {sb.ToString()}");


            if (dtTable.Rows.Count > 0)
            {

                List<PersonelGuzergahDetay.PERSONEL_GUZERGAHDETAY> iDefGenel = new List<PersonelGuzergahDetay.PERSONEL_GUZERGAHDETAY>();

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    PersonelGuzergahDetay.PERSONEL_GUZERGAHDETAY def = new PersonelGuzergahDetay.PERSONEL_GUZERGAHDETAY
                    {
                        ID = Convert.ToInt32(dtTable.Rows[i]["ID"]),
                        FKID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["FKID"], "0")),
                        SIRANO = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["SIRANO"], "0")),
                        PERSONELID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["PERSONELID"], "0")),
                        BOLUMSINIFID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["BOLUMSINIFID"], "0")),
                        BOLUMSINIF = Utility.Nvl(dtTable.Rows[i]["BOLUMSINIF"]),
                        PERSONEL = Utility.Nvl(dtTable.Rows[i]["PERSONEL"]),
                        ADRESI = Utility.Nvl(dtTable.Rows[i]["ADRESI"]),
                        TELEFON1 = Utility.Nvl(dtTable.Rows[i]["TELEFON1"]),
                        TELEFON2 = Utility.Nvl(dtTable.Rows[i]["TELEFON2"]),
                        SAAT = Utility.Nvl(dtTable.Rows[i]["SAAT"]),
                        ACIKLAMA = Utility.Nvl(dtTable.Rows[i]["ACIKLAMA"]),
                        INSERT_USERID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["INSERT_USERID"], "0")),
                        INSERT_DATE = Utility.Nvl(dtTable.Rows[i]["INSERT_DATE"]),
                        STATE = Convert.ToBoolean(Utility.Nvl(dtTable.Rows[i]["STATE"])),
                        COMPANYID = Convert.ToInt32(Utility.Nvl(dtTable.Rows[i]["COMPANYID"], "0"))


                    };

                    iDefGenel.Add(def);
                }

                return iDefGenel.ToList();
            }

            return null;
        }
        public JsonResult getPersonelGuzergahDetay(string pToken, string FKID, string PKID = "")
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

                getPersonelGuzergahDetayData tumveriler = new getPersonelGuzergahDetayData
                {
                    PERSONEL_GUZERGAHDETAY = DefPersonelGuzergahDetay(FKID, PKID),

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Güzergah Detay Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }
        #endregion

        #region DEF_SEHIRILCE
        public List<GenelTanim.DEF_GENEL> DefSehirIlce(string FKID, string PKID = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(PKID))
            {
                sb.Append($" AND ID={PKID}");
            }
            var dtTable = SQL.GetDataTable($"select ID,ADI from dbo.DEF_SEHIRILCE where FKID={FKID} {sb.ToString()}");


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
        public JsonResult getSehirIlce(string pToken, string pSEHIRID, string PKID = "")
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
                    DEF_GENEL = DefSehirIlce(pSEHIRID, PKID)

                };

                return Json(new { data = tumveriler, success = true, status = 999, statusText = "Sunucudan Bilgiler Alındı" });
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "İLÇE Listesi Alınırken Hata Oluştu => " + e.Message });
            }
        }

        #endregion

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


        #region TANIMTABLOLARI
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


                #region PersonelTanım

                if (pTableName == "PERSONEL_TANIM")
                {
                    if (!string.IsNullOrEmpty(collection["SICILNO"]))
                    {
                        dTable = SQL.GetDataTable(
                            $"select * from PERSONEL_TANIM where SICILNO='{collection["SICILNO"]}'   ");
                        if (dTable.Rows.Count > 0)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Bu Kart Numarası Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
                        }
                        dTable.Clear();
                    }

                    if (!string.IsNullOrEmpty(collection["KIMLIKNO"]))
                    {
                        dTable = SQL.GetDataTable(
                            $"select * from PERSONEL_TANIM where  KIMLIKNO='{collection["KIMLIKNO"]}' ");
                        if (dTable.Rows.Count > 0)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Bu Kimlik Numarası Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
                        }
                        dTable.Clear();
                    }
                   
                }



                #endregion

                #region ServisArac

                if (pTableName == "DEF_PERSONEL_SERVISARAC")
                {
                    if (!string.IsNullOrEmpty(collection["PLAKA"]))
                    {
                        dTable = SQL.GetDataTable(
                            $"select * from DEF_PERSONEL_SERVISARAC where PLAKA='{collection["PLAKA"]}'");
                        if (dTable.Rows.Count > 0)
                        {
                            return Json(new { data = "", success = false, status = 402, statusText = "Bu Plaka Numaralı Araç Daha Önce Sisteme Kaydedildiği için Kayıt İptal Edildi" });
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
                    if (pTableName == "DEF_DONEM")
                    {
                        SQL.ExecuteNonQuery($"update DEF_DONEM set VARSAYILAN=0 where ID<>{ID}");
                    }


                    return Json(new { data = ID, success = true, status = 999, statusText = "Veriler Sisteme Başarıyla Kaydedildi." });
                }
                else
                {
                    if (dbOp.error)
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu" });
                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 401, statusText = "Lütfen Gerekli Alanları Doldurunuz" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { data = "", success = false, status = 402, statusText = "Veriler Kaydedilirken Hata Oluştu => " + e.Message });
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


                    var vdbOp = new ImzaData.Ops { _TableName = $"{pTableName}" };

                    //var vSonuc = vdbOp.Update(collection, "PERSONEL_TANIM",$" ID = {id}");
                    vdbOp.Result(vdbOp.Update(collection, $"{pTableName}", $" ID = {PKID}"), "Kayıt Bilgileri Başarıyla Güncellendi");

                    //var dbOp = new ImzaData.Ops { _TableName = "PERSONEL_TANIM" };
                    //dbOp.ResultForApi(dbOp.InsertApi(collection, "1", Utility.Nvl(pUSERID, "-1")), "Yeni kayıt başarıyla tamamlandı.", "");

                    if (vdbOp.success)
                    {
                        if (pTableName == "DEF_DONEM")
                        {
                            SQL.ExecuteNonQuery($"update DEF_DONEM set VARSAYILAN=0 where ID<>{PKID}");
                        }
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
                        return Json(new { data = "", success = true, status = 999, statusText = "Veriler Başarıyla Silindi" });

                    }
                    else
                    {
                        return Json(new { data = "", success = false, status = 402, statusText = "Silinirken Hata Oluştu" });

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




        public ActionResult Index()
        {
            return View();
        }
    }
}