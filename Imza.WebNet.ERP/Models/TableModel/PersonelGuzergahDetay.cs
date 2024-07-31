using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class PersonelGuzergahDetay
    {
        public class PERSONEL_GUZERGAHDETAY
        {

            public int ID { get; set; }

            public int FKID { get; set; }

            public int SIRANO { get; set; }

            public int PERSONELID { get; set; }

            public int BOLUMSINIFID { get; set; }

            public string BOLUMSINIF { get; set; }

            public string PERSONEL { get; set; }

            public string ADRESI { get; set; }

            public string TELEFON1 { get; set; }

            public string TELEFON2 { get; set; }

            public string SAAT { get; set; }

            public string ACIKLAMA { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }

    }
}