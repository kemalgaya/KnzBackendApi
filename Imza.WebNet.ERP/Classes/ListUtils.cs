using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Imza.WebTools.Classes;

namespace Imza.WebNet.Erp.Classes
{
    public class ListUtils
    {

        public int SortString(string s1, string s2, string sortDirection)
        {
            try
            {
                return sortDirection == "asc" ? s1.CompareTo(s2) : s2.CompareTo(s1);
            }
            catch
            {
                return 0;
            }
        }

        public int SortInteger(string s1, string s2, string sortDirection)
        {
            try
            {
                int i1 = int.Parse(s1);
                int i2 = int.Parse(s2);
                return sortDirection == "asc" ? i1.CompareTo(i2) : i2.CompareTo(i1);
            }
            catch
            {
                return 0;
            }
        }

        public int SortDateTime(string s1, string s2, string sortDirection)
        {

            try
            {
                DateTime d1 = DateTime.Parse(s1);
                DateTime d2 = DateTime.Parse(s2);
                return sortDirection == "asc" ? d1.CompareTo(d2) : d2.CompareTo(d1);
            }
            catch
            {
                return 0;
            }
        }
    }
}