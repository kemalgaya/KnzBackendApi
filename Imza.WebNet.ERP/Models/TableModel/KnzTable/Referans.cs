using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Referans
    {
        public class KNZ_REFERANS
        {
            public int ID { get; set; }

            public string ADI { get; set; }

            public string SOYADI { get; set; }

            public string FIRMA { get; set; }

            public string MAIL { get; set; }

            public string TELEFON { get; set; }

            public string ADRES { get; set; }

            public string DOSYAYOLU { get; set; }

            public string ACIKLAMA { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }

    }
}