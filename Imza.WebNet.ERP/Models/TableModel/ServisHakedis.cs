using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class ServisHakedis
    {
        public class DEF_SERVIS_HAKEDIS
        {
            public bool SEC { get; set; }

            public int ID { get; set; }

            public int CARIHESAPID { get; set; }

            public string CARIHESAP { get; set; }

            public int SERVISARACID { get; set; }

            public string PLAKA { get; set; }

            public int ISLEMADEDI { get; set; }

            public float ISLEMADETUCRETI { get; set; }

            public float HAKEDISTUTARI { get; set; }

            public string TARIH { get; set; }

            public string ACIKLAMA { get; set; }

            public string EKLEYEN { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }

    }
}