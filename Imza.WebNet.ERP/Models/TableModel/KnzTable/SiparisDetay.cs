using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class SiparisDetay
    {
        public class SIPARISDETAY
        {
            public int ID { get; set; }

            public int SIPARISID { get; set; }

            public int PRODUCTID { get; set; }

            public int COUNT { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }
    }
}