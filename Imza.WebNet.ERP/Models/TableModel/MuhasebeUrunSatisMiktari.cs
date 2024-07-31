using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeUrunSatisMiktari
    {
        public class URUNSATIS
        {
            public int ID { get; set; }
            public string ADI { get; set; }
            public string BARKODNO { get; set; }
            public float GECENAYSATISMIKTARI { get; set; }
            public float BUAYSATISMIKTARI { get; set; }
            public float ARTISMIKTARI { get; set; }
            public float TOPLAMSATIS { get; set; }
            public float TOPLAMSATISMIKTARI { get; set; }

        }

        
    }
}