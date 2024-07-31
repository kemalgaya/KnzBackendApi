using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imza.WebTools.Classes;

namespace Imza.WebNet.Erp.Classes
{
    public class FormUtils
    {
        public string boolToString(bool pValue)
        {
            if (pValue)
            {
                return "<span class=\"badge rounded-pill badge-light-success\" text-capitalized=\"\">Aktif</span>";
            }

            return "<span class=\"badge rounded-pill badge-light-secondary\" text-capitalized=\"\">Pasif</span>";
        }
        public List<SelectListItem> SelectItems(string pSql, string pText, string pValue, string pSelected)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            var vTable = SQL.GetDataTable($"{pSql}");

            if (vTable==null)
            {
                return items;
            }

            for (int i = 0; i < vTable.Rows.Count; i++)
            {
                bool vSelected = Utility.Nvl(vTable.Rows[i][pValue]) == pSelected;
                items.Add(new SelectListItem { Text = Utility.Nvl(vTable.Rows[i][pText]), Value = Utility.Nvl(vTable.Rows[i][pValue]), Selected = vSelected });
            }

            return items;
        }


        public List<SelectListItem> SelectItemsStaticData(DataTable pDataTable, string pText, string pValue, string pSelected)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            var vTable = pDataTable;

            if (vTable == null)
            {
                return items;
            }

            for (int i = 0; i < vTable.Rows.Count; i++)
            {
                bool vSelected = Utility.Nvl(vTable.Rows[i][pValue]) == pSelected;
                items.Add(new SelectListItem { Text = Utility.Nvl(vTable.Rows[i][pText]), Value = Utility.Nvl(vTable.Rows[i][pValue]), Selected = vSelected });
            }

            return items;
        }

        public int? CheckValueEdit(string pValue)
        {
            if (pValue=="true" || pValue =="True" || pValue == "TRUE" || pValue == "on")
            {
                return 1;
            }

            else if (pValue == "false" || pValue == "False" || pValue == "FALSE")
            {
                return 0;
            }
            else if(!string.IsNullOrEmpty(pValue))
            {
                return Convert.ToInt32(pValue);
            }

            return null;
        }

        public string getFileName(string pFileName)
        {

            //Use Namespace called :  System.IO  
            string FileName = Path.GetFileNameWithoutExtension(pFileName);

            //To Get File Extension  
            string FileExtension = Path.GetExtension(pFileName);

            //Add Current Date To Attached File Name  
            FileName = dbStaticUtils.GetSequenceValue($"SQE_FILE_ID") + FileExtension;

            return FileName;
        }

        /// <summary>
        /// Request, Server, Request.Form["hfileFATURA"]
        /// </summary>
        /// <param name="pTABLO"></param>
        /// <param name="pDOSYAADI"></param>
        /// <param name="postedFileBase">Request yaz sadece</param>
        /// <param name="pServer">Server yaz sadece</param>
        /// <param name="pEskiDosya">Request.Form["ESKİ ELEMAN ADI"]</param>
        /// <returns></returns>
        public string FileUpload(string pTABLO,string pDOSYAADI,HttpRequestBase postedFileBase, HttpServerUtilityBase pServer, string pEskiDosya="")
        {
            HttpPostedFileBase postedFile = postedFileBase.Files[pDOSYAADI];
            try
            {

                if (!string.IsNullOrEmpty(Utility.Nvl(postedFile.FileName)))
                {
                    string path = pServer.MapPath("~/Resources/");

                    var vDosyaYoluDb = $"{pTABLO}_{pDOSYAADI}_{getFileName(postedFile.FileName)}";
                    var vDosyaYoluEski = path + Path.GetFileName(pEskiDosya);
                    var vDosyaYolu = path + Path.GetFileName(vDosyaYoluDb);
                    postedFile.SaveAs(vDosyaYolu);

                    if (System.IO.File.Exists(vDosyaYolu))
                    {
                        if (File.Exists(vDosyaYoluEski))
                        {
                            File.Delete(vDosyaYoluEski);
                        }
                        return vDosyaYoluDb;
                    }
                }
            }
            catch
            {
                Utility.Nvl(pEskiDosya);
                //return "";
            }

            return pEskiDosya;
        }

        public void FileDelete(string pDosyaYolu, HttpServerUtilityBase pServer)
        {
            if (!string.IsNullOrEmpty(pDosyaYolu))
            {
                string path = pServer.MapPath("~/Resources/");
                 pDosyaYolu = path + Path.GetFileName(pDosyaYolu);

                if (File.Exists(pDosyaYolu))
                {
                    File.Delete(pDosyaYolu);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPKID">PKID Değeri</param>
        /// <param name="pPKNAME">PKID KolonAdı</param>
        /// <param name="pTableName">Tablo Adı</param>
        /// <returns></returns>
        public string DeleteRow(string pPKID,string pPKNAME,string pTableName,string pFKIDNAME="",string pSubTableName="")
        {

            if (!string.IsNullOrEmpty(pPKID))
            {
                var vDelId = Convert.ToInt32(pPKID);

                if (vDelId > 0)
                {
                    try
                    {

                        dbStruct.StDbParamList dbParams = new dbStruct.StDbParamList();

                        var vdbO = new SQLTransaction();

                        dbParams.Add("1", $"delete {pTableName} where {pPKNAME}={vDelId}");

                        if (!string.IsNullOrEmpty(pSubTableName))
                        {
                            dbParams.Add("2", $"delete {pSubTableName} where {pFKIDNAME}={pPKID}");
                        }
                        if (vdbO.ExecuteScalar(dbParams) == "1")
                        {
                            return WebUtils.CssMesaj(WebUtils.MesajTipi.Tamam, "Kayıt başarıyla Silinmiştir.");
                        }
                        else
                        {
                            return WebUtils.CssMesaj(WebUtils.MesajTipi.Hata, $"Kayıt Silinirken Hata Oluştu Transaction Save Problem!");
                        }

                    }
                    catch (Exception hata)
                    {
                        return WebUtils.CssMesaj(WebUtils.MesajTipi.Hata, $"Kayıt Silinirken Hata Oluştu Detay=>{hata}");
                    }
                }
            }

            return "";
        }
    }
}