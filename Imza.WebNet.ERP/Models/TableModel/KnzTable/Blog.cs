using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Blog
    {
        public class KNZ_BLOG
        {
            public int ID { get; set; }

            public string BASLIK { get; set; }

            public string ICERIK { get; set; }

            public string DOSYAYOLU { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }
    }
}