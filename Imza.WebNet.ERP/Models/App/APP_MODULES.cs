using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imza.WebNet.Erp.Models.App
{



    public class APP_MODULES
    {
        [Required(ErrorMessage = "Bu Alan Zorunludur.")]
        [DisplayName("Modül ID")]
        public int MODULID { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur.")]
        [DisplayName("Modül Adı")]
        public string MODULNAME { get; set; }

        public string ICONID { get; set; }

        public string CODE { get; set; }

        public float INSERT_USERID { get; set; }

        public string INSERT_DATE { get; set; }

        public bool STATE { get; set; }

        public float COMPANYID { get; set; }

        public float ROWNUMBER { get; set; }
        public string ACTION { get; set; }

    }

}