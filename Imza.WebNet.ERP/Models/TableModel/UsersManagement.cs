using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class UsersManagement
    {
        public class AUTH_USERS
        {
            public int USERID { get; set; }

            public string NAME { get; set; }

            public string SURNAME { get; set; }

            public string USERNAME { get; set; }

            public string PASSWORD { get; set; }

            public DateTime? PSWCHANGEDATE { get; set; }

            public int? CODE { get; set; }

            public int INSERT_USERID { get; set; }

            public DateTime? INSERT_DATE { get; set; }

            public int? UPDATE_USERID { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool? STATE { get; set; }

            public string KIMLIKNO { get; set; }

            public string EMAIL { get; set; }

            public int? USERTYPE { get; set; }

        }
        public class AUTH_GROUPS
        {
            public int GROUPID { get; set; }

            public string GROUPNAME { get; set; }

            public string CODE { get; set; }

            public int? INSERT_USERID { get; set; }

            public DateTime? INSERT_DATE { get; set; }

            public float? UPDATE_USERID { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public int? STATE { get; set; }

            public int? COMPANYID { get; set; }

        }
        public class APP_PARAMETER_GROUPS
        {
            public int ID { get; set; }

            public string NAME { get; set; }
            public string MODULENAME { get; set; }

            public string DESCRIPTIONS { get; set; }

            public string CODE { get; set; }

            public int INSERT_USERID { get; set; }

            public DateTime INSERT_DATE { get; set; }

            public float UPDATE_USERID { get; set; }

            public DateTime UPDATE_DATE { get; set; }

            public int STATE { get; set; }

            public int COMPANYID { get; set; }

            public int MODULEID { get; set; }

        }
        public class APP_PARAMETERS
        {
            public float ID { get; set; }

            public string DEV_NAME { get; set; }

            public string DEV_VALUE { get; set; }

            public string ADI { get; set; }

            public int GRUPID { get; set; }

            public int SIRA { get; set; }

            public int TIP { get; set; }

            public int MODULID { get; set; }

            public string ACIKLAMA { get; set; }

            public int INSERT_USERID { get; set; }

            public DateTime? INSERT_DATE { get; set; }

            public float? UPDATE_USERID { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public int STATE { get; set; }

            public int COMPANYID { get; set; }

            public int CODE { get; set; }

        }
        public class AUTH_AUTHORIZE_USERSWEB
        {
            public int ID { get; set; }

            public int USERID { get; set; }

            public int FORMID { get; set; }

            public int AUTH_VIEW { get; set; }

            public int AUTH_ADD { get; set; }

            public int AUTH_UPDATE { get; set; }

            public int AUTH_SAVE { get; set; }

            public int AUTH_DELETE { get; set; }

            public int AUTH_REPORT { get; set; }

            public int INSERT_USERID { get; set; }

            public DateTime INSERT_DATE { get; set; }

            public int UPDATE_USERID { get; set; }

            public DateTime UPDATE_DATE { get; set; }

            public int STATE { get; set; }

            public int COMPANYID { get; set; }

        }
        public class APP_FORMSWEB
        {
            public int ID { get; set; }

           
            
            public string ICONID { get; set; }
            public string NAME { get; set; }

            public string NAMESPACE { get; set; }

            public string MENUFORM { get; set; }

            public int MENUFORMID { get; set; }
            public string AUTH_VIEW { get; set; }
            public string AUTH_ADD { get; set; }
            public string AUTH_UPDATE { get; set; }
            public string AUTH_DELETE { get; set; }
            

            public int INSERT_USERID { get; set; }

            public DateTime? INSERT_DATE { get; set; }

            public float? UPDATE_USERID { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public int STATE { get; set; }

            public int COMPANYID { get; set; }

            public int MODULID { get; set; }

            
            public int GROUPID { get; set; }

            

           

        }

    }
}