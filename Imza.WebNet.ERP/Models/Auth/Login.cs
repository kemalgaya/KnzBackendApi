using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Imza.WebNet.Erp.Models.Auth
{
    public partial class AUTH_USERS
    {
        public int USERID { get; set; }

        [DisplayName("Adı")]
        public string NAME { get; set; }

        [DisplayName("Soyadı")]
        public string SURNAME { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur.")]
        [DisplayName("Kullanıcı Adı")]
        public string USERNAME { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu Alan Zorunludur.")]
        [DisplayName("Şifresi")]
        public string PASSWORD { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Şifre Tekrar")]
        public string WEBSIFRESITEKRAR { get; set; }

        [DisplayName("T.C. Kimlik No")]
        public string KIMLIKNO { get; set; }

        [DisplayName("Email")]
        public string EMAIL { get; set; }

        [DisplayName("Kullanıcı Tipi")]
        public Nullable<int> USERTYPE { get; set; }
    }


}