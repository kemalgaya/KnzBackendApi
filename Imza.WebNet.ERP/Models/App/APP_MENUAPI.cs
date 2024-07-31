using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.Erp.Models.App
{
    public class getAllMenu
    {
        public IList API_APP_MODULES { get; set; }
        public IList API_APP_FORMS { get; set; }
    }

    public class API_APP_FORMS
    {
        public int ID { get; set; }


        public string NAME { get; set; }

        public string NAMESPACE { get; set; }


        public string ICONID { get; set; }

        public int MODULID { get; set; }



        public int ROWNUMBER { get; set; }



        public bool MENUFORM { get; set; }

        public int MENUFORMID { get; set; }

    }

    public class API_APP_MODULES
    {
        public int MODULID { get; set; }

        public string MODULNAME { get; set; }

        public int ROWNUMBER { get; set; }

    }
}


