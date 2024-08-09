using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Kampanyalar
    {
        public class KNZ_KAMPANYALAR
        {
            public int ID { get; set; }

            public string ADI { get; set; }

            public string OZELLIK { get; set; }

            public string BITISTARIHI { get; set; }

            public int MUSTERIGRUBUID { get; set; }

            public string DOSYAYOLU { get; set; }

            public string DOSYAYOLU2 { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }
    }
}