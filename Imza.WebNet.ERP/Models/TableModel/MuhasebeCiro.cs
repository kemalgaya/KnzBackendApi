using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeCiro
    {
        public class AYLIKCIRO
        {
            public float YUKLEME { get; set; }
            public float HARCAMA { get; set; }
            public string AY { get; set; }
            public string YIL { get; set; }
        }
        public class SUBECIROKARTANALIZ
        {
            public int ID { get; set; }
            public string SUBEADI { get; set; }
            public int KARTSAYISI { get; set; }
            public float TAHSILAT { get; set; }
            public float SATISLAR { get; set; }
            public int HARCAMAYAPILANGUN { get; set; }
            public float ORTCIRO { get; set; }//satıslar/kartsayısı
            public float ORTCIROGUN { get; set; }//gunluk ortalama ciro   satıslar/kartsayısı*gun
            
        }
        public class SUBECIROTARIHKARSILASTIRMA
        {
            public int ID { get; set; }
            public string SUBEADI { get; set; }
            public float TAHSILAT { get; set; }
            public float SATISLAR { get; set; }
            public float TARIHTAHSILAT { get; set; }
            public float TARIHSATISLAR { get; set; }
        }

        public class TOPLAMSATISYUKLEME
        {
            [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
            public decimal TOPLAMSATIS { get; set; }
            [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
            public decimal TOPLAMYUKLEME { get; set; }
            [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
            public decimal KALANBAKIYE { get; set; }
        }
    }
}