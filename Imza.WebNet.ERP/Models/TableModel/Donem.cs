using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class Donem
    {
        public class DEF_DONEM
        {
            public int ID { get; set; }

            public string ADI { get; set; }

            public bool VARSAYILAN { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }

    }
}