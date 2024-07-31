using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeKasa
    {
        public class DEF_MUHASEBE_KASA
        {
            public int ID { get; set; }

            public string ADI { get; set; }

            public int BANKAID { get; set; }

            public string HESAPADI { get; set; }

            public string IBAN { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

            public string BANKAADI { get; set; }

        }

    }
}