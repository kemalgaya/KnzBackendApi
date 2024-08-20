using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel.KnzTable
{
    public class Post
    {
        public class POSTS
        {
            public int ID { get; set; }

            public int USERID { get; set; }

            public string KULLANICI { get; set; }
            public string TITLE { get; set; }

            public string BODY { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

        }

    }
}