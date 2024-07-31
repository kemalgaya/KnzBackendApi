using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class VeliKullanici
    {
        public class DEF_VELIKULLANICI
        {
            public int ID { get; set; }

            public string NAME { get; set; }

            public string SURNAME { get; set; }

            public string KIMLIKNO { get; set; }

            public string EMAIL { get; set; }

            public string USERNAME { get; set; }

            public string PASSWORD { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }

    }
}