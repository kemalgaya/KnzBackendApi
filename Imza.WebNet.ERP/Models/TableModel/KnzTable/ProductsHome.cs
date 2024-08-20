using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class ProductsHome
    {

        public class PRODUCTSHOME
        {
            public int HOMEID { get; set; }

            public int HOMECATEGORY { get; set; }

            public int ID { get; set; }

            public string NAME { get; set; }

            public int PRICE { get; set; }

            public string IMGFILE { get; set; }

            public string DESCRIPTION { get; set; }

            public int CATEGORYID { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }


    }
}