using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeSaatlikCiro
    {
        public class CIRO_SAATLIK_ANALIZ
        {
            public int ID { get; set; }
            public string ADI { get; set; }
            public string LOKASYON { get; set; }
            public int INSERT_USERID { get; set; }
            public DateTime INSERT_DATE { get; set; }
            public int STATE { get; set; }
            public int COMPANYID { get; set; }

            public float MESAITOPLAM { get; set; }
            public float S08000830 { get; set; }
            public float S08300900 { get; set; }
            public float S09000930 { get; set; }
            public float S09301000 { get; set; }
            public float S10001030 { get; set; }
            public float S10301100 { get; set; }
            public float S11001130 { get; set; }
            public float S11301200 { get; set; }
            public float S12001230 { get; set; }
            public float S12301300 { get; set; }
            public float S13001330 { get; set; }
            public float S13301400 { get; set; }
            public float S14001430 { get; set; }
            public float S14301500 { get; set; }
            public float S15001530 { get; set; }
            public float S15301600 { get; set; }
            public float S16001630 { get; set; }
            public float S16301700 { get; set; }
            public float S17001730 { get; set; }
            public float S17301800 { get; set; }

        }
    }
}