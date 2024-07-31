using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class About
    {
        public class KNZ_ABOUT
        {
            public int ID { get; set; }

            public string DOSYAYOLUFOTO1 { get; set; }

            public string MISYON { get; set; }

            public string VIZYON { get; set; }

            public string DOSYAYOLUFOTO2 { get; set; }

            public string DOSYAYOLUFOTO3 { get; set; }

            public string ADRESS { get; set; }

            public string ADRESSLINK { get; set; }

            public string PHONE1 { get; set; }

            public string PHONE2 { get; set; }

            public string MAIL { get; set; }

            public string SITELINK { get; set; }

            public string DOSYAYOLU { get; set; }

            public string DOSYAYOLU2 { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }

    }
}