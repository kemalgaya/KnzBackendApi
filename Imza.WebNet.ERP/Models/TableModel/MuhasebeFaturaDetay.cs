using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeFaturaDetay
    {
        public class MUHASEBE_FATURADETAY
        {
            public int ID { get; set; }

            public int FKID { get; set; }

            public int CARIID { get; set; }

            public float KDVORAN { get; set; }

            public float KDVTUTAR { get; set; }

            public float ISKONTOORAN { get; set; }

            public float ISKONTOTUTAR { get; set; }

            public string URUNADI { get; set; }
            public int URUNHIZMETID { get; set; }

            public int MIKTAR { get; set; }

            public float VERGISIZTOPLAM { get; set; }

            public float FIYAT { get; set; }

            public float TOPLAMTUTAR { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

            public string EKLEYEN { get; set; }

            public float BORC { get; set; }

            public float ALACAK { get; set; }

        }


    }
}