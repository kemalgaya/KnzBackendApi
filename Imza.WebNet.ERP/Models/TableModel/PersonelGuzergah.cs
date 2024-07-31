using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class PersonelGuzergah
    {
        public class PERSONEL_GUZERGAH
        {
            public int ID { get; set; }

            public string ADI { get; set; }
            public string SERVISARAC { get; set; }

            public string SOFOR { get; set; }

            public string HOSTES { get; set; }

            public string DONEM { get; set; }

            public string OKUL { get; set; }
            public int ARACID { get; set; }

            public int SOFORID { get; set; }

            public int HOSTESID { get; set; }

            public string TARIHI { get; set; }

            public string ACIKLAMA { get; set; }

            public int DONEMID { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

            public int OKULID { get; set; }


        }

    }
}