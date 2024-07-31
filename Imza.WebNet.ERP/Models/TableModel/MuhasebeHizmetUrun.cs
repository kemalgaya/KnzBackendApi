using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeHizmetUrun
    {
        public class DEF_MUHASEBE_HIZMETURUN
        {
            public int ID { get; set; }

            public string KISAKOD { get; set; }

            public string ADI { get; set; }

            public string BIRIMADI { get; set; }

            public string FORMTURUADI { get; set; }

            public int KDVORAN { get; set; }

            public string EKLEYEN { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public float ALISFIYATI { get; set; }

            public float SATISFIYATI { get; set; }

            public string KATEGORIGRUPADI { get; set; }

            public int COMPANYID { get; set; }

            public bool ISMALZEME { get; set; }

            public string BARKODNO { get; set; }

            public float SATISTOPLAM { get; set; }

            public float ALISTOPLAM { get; set; }

            public int ALISMIKTARI { get; set; }

            public int SATISMIKTARI { get; set; }

            public int STOK { get; set; }

            public int STOKUYARISAYISI { get; set; }

            public int BIRIMID { get; set; }

            public int FORMTURUID { get; set; }

            public int KATEGORIGRUPID { get; set; }


        }
    }
}