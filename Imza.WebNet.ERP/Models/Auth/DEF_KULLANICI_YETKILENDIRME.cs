using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Imza.WebNet.Erp.Models.Auth
{
    public class DEF_KULLANICI_YETKILENDIRME
    {
        public int ID { get; set; }

        [DisplayName("Kullanıcı")]
        public string KULLANICIID { get; set; }
        [DisplayName("Kullanıcı")]
        public string KULLANICIADI { get; set; }

        [DisplayName("Personel")]
        public string PERSONELID { get; set; }
        [DisplayName("Personel")]
        public string PERSONELADI { get; set; }

    }
}