using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Slider
    {
        public class KNZ_SLIDER
        {
            public int ID { get; set; }

            public string BASLIK1 { get; set; }

            public string BASLIK2 { get; set; }

            public string BUTONYAZI { get; set; }

            public int SIRANO { get; set; }

            public string DOSYAYOLU { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }
    }
}