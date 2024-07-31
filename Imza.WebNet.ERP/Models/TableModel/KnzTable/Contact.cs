using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Contact
    {
        public class KNZ_CONTACT
        {
            public int ID { get; set; }

            public string NAME { get; set; }

            public string SURNAME { get; set; }

            public string MAIL { get; set; }

            public string PHONE { get; set; }

            public string SUBJECT { get; set; }

            public string MESSAGE { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }
    }
}