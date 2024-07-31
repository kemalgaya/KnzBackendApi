using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imza.WebNet.Erp.Models.Mobil
{
    public class mobilGiris
    {
        public string USERID { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur.")]
        [DisplayName("Kullanıcı Adı")]
        public string USERNAME { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu Alan Zorunludur.")]
        [DisplayName("Şifresi")]
        public string PASSWORD { get; set; }


        [DisplayName("Adı")]
        public string NAME { get; set; }
        [DisplayName("TOKEN")]
        public string TOKEN { get; set; }
        [DisplayName("Soyadı")]
        public string SURNAME { get; set; }
    }
}