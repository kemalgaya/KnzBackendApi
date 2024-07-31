using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Imza.WebNet.Erp.Models.Parametreler
{
    public class APP_PARAMETER_GROUPS
    {
        public int ID { get; set; }
        [DisplayName("Parametre Grup Adı")]
        public string NAME { get; set; }

        [DisplayName("Açıklama")]
        public string DESCRIPTIONS { get; set; }

        [DisplayName("Kodu")]
        public string CODE { get; set; }

        [DisplayName("Ekleyen")]
        public string INSERT_USERID { get; set; }

        [DisplayName("Tarih")]
        public DateTime INSERT_DATE { get; set; }

        [DisplayName("Modül")]
        public string MODULID { get; set; }

        [DisplayName("Modül")]
        public string MODULADI { get; set; }
        
    }

}