using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class PersonelUyarilar
    {
        public class PERSONEL_UYARILAR
        {
            public int ID { get; set; }

            public string UYARITURU { get; set; }

            public int UYARITURUID { get; set; }

            public string UYARIVERILEN { get; set; }

            public int UYARIVERILENID { get; set; }

            public string TARIH { get; set; }

            public int OGRENCIID { get; set; }

            public string OGRENCI { get; set; }

            public string VELIADISOYADI { get; set; }

            public string VELIYAKINLIK { get; set; }

            public int PERSONELID { get; set; }

            public string PERSONEL { get; set; }

            public string ACIKLAMA { get; set; }

            public string EKLEYEN { get; set; }

            public float INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }


        }

    }
}