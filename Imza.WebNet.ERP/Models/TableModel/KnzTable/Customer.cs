using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Customer
    {
        public class KNZ_CUSTOMER
        {
            public int ID { get; set; }

            public string NAME { get; set; }

            public string SURNAME { get; set; }

            public string MAIL { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }

    }
}