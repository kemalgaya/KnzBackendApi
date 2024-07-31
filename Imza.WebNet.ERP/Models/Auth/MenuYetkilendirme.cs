using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Imza.WebNet.Erp.Models.Auth
{
    public class APP_FORMS
    {
        public int ID { get; set; }

        [DisplayName("Menü Adı")]
        public string NAME { get; set; }
        
            [DisplayName("ICON")]
            [AllowHtml] //bu tanım

        public string ICONID { get; set; }
        [DisplayName("Menü Yolu")]
        public string NAMESPACE { get; set; }

        [DisplayName("Üst menü mü?")]
        public bool MENUFORM { get; set; }



        [DisplayName("Üst menü")]
        //public IEnumerable<SelectListItem> MENUFORMID { get; set; }
        public string MENUFORMID { get; set; }

        [DisplayName("Üst menü")]
        //public IEnumerable<SelectListItem> MENUFORMID { get; set; }
        public string MENUFORMADI { get; set; }


        [DisplayName("Modül")]

        public string MODULID { get; set; }

        [DisplayName("Modül")]

        public string MODULADI { get; set; }
        [DisplayName("Sıra")]
        public string ROWNUMBER { get; set; }
        public string ACTION { get; set; }

    }


    public class classGetColumnName
    {



        public string getColumnName(int sortColumn)
        {
            string columnName = "";
            switch (sortColumn)
            {
                case 0:
                {
                    columnName = col0;
                }
                    break;
                case 1:
                    {
                        columnName = col1;
                    }
                    break;
                case 2:
                    {
                        columnName = col2;
                    }
                    break;
                case 3:
                    {
                        columnName = col3;
                    }
                    break;
                case 4:
                    {
                        columnName = col4;
                    }
                    break;
                case 5:
                {
                    columnName = col5;
                }
                    break;
                case 6:
                {
                    columnName = col6;
                }
                    break;

                default:
                {
                    columnName = col0;
                }
                    break;
            }

            return columnName;
        }

        /*

                       { "data": "ID", "orderable": true },
                    { "data": "MODULADI", "orderable": true },
                    { "data": "NAME", "orderable": true },
                    { "data": "NAMESPACE", "orderable": true },
                    { "data": "MENUFORM", "orderable": true },
                    { "data": "MENUFORMADI", "orderable": true },
                    { "data": "ROWNUMBER", "orderable": true },
                    { "data": "ACTION", "orderable": false }

            */

        string col0 = "ID";
         string col1 = "MODULADI";
         string col2 = "NAME";
         string col3 = "NAMESPACE";
         string col4 = "MENUFORM";
        string col5 = "MENUFORMADI";
        string col6 = "ROWNUMBER";

    }
}