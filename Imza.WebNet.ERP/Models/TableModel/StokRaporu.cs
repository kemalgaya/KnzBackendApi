using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class StokRaporu
    {
        public class STOKTAKIBI
        {
            public int ID { get; set; }
            public string ADI { get; set; }
            public string BARKODNO { get; set; }
            public float STOK { get; set; }
            public int COMPANYID { get; set; }
            public int INSERT_USERID { get; set; }
            public DateTime INSERT_DATE { get; set; }
            public int STATE { get; set; }
        }

        public class STOKDURUMDETAILS
        {
            public int SUBEID { get; set; }
            public string SUBEADI { get; set; }
            public float STOK { get; set; }
        }
    }
}