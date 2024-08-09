using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class DosyaEki
    {
        public class KNZ_DOSYAEKI
        {
            public int ID { get; set; }

            public string ADI { get; set; }

            public int? DOSYATURUID { get; set; }

            public string DOSYAYOLU { get; set; }

            public string ACIKLAMA { get; set; }

            public int  INSERT_USERID { get; set; }

            public string  INSERT_DATE { get; set; }

            public int? COMPANYID { get; set; }

            public bool? STATE { get; set; }

        }

    }
}