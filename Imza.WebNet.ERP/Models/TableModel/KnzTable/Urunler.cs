using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Urunler
    {
        public class KNZ_URUNLER
        {
            public int ID { get; set; }

            public string URUNADI { get; set; }

            public string URUNACIKLAMA { get; set; }

            public string EBAT { get; set; }

            public int  MARKAID { get; set; }

            public decimal  FIYAT { get; set; }

            public int  MINSTOK { get; set; }

            public int ORTSTOK { get; set; }

            public int MINSIPARIS { get; set; }

            public string OZELLIK1 { get; set; }

            public string OZELLIK2 { get; set; }

            public string OZELLIK3 { get; set; }

            public string OZELLIK4 { get; set; }

            public string OZELLIK5 { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }
    }
}