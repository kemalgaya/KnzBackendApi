using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class PersonelDosyaEki
    {
        public class PERSONEL_DOSYAEKI
        {
            public int ID { get; set; }

            public int KISIID { get; set; }

            public int EVRAKTURUID { get; set; }

            public bool DURUMU { get; set; }

            public string TARIHI { get; set; }

            public string DOSYAYOLU { get; set; }

            public string ACIKLAMA { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }

    }
}