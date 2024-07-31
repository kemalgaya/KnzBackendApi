using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class GenelAnaliz
    {
        public class KART_ANALIZ
        {
            public int YUKLEMEYAPILANKARTSAYISI { get; set; }
            public int VELITANIMLIOGRENCISAYISI { get; set; }
            public int HARCAMAYAPILANKARTSAYISI { get; set; }
            public int AKTIFKARTSAYISI { get; set; }
            public int KARTSAYISI { get; set; }
            public float KAYITLIPARAYUKLEMEYEN { get; set; }
            public float PARAYUKLUHARCAMAYAPMAYAN { get; set; }
            public float HARCAMAYAPMAYANKARTBAKIYE { get; set; }
            public float HARCANMAYANTOPLAMBAKIYE { get; set; }

        }
        public class KART_ANALIZ_SUBELI
        {
            public int ID { get; set; }
            public string SUBEADI { get; set; }
            public int YUKLEMEYAPILANKARTSAYISI { get; set; }
            public int HARCAMAYAPILANKARTSAYISI { get; set; }
            public int VELITANIMLIOGRENCISAYISI { get; set; }
            public float KAYITLIPARAYUKLEMEYEN { get; set; }
            public float PARAYUKLUHARCAMAYAPMAYAN { get; set; }

        }

        public class CIRO_ANALIZ
        {

            public float KREDIKARTI { get; set; }
            public float KARTLISATIS { get; set; }
            public float PERSONELKARTICIRO { get; set; }
            public float OGRENCIKARTCIRO { get; set; }
            public float TOPLAMCIRO { get; set; }
            public float KREDIKARTIYUZDE { get; set; }
            public float KARTLISATISYUZDE { get; set; }

        }

        public class CIRO_ANALIZ_SUBELI
        {
            public int ID { get; set; }
            public string SUBEADI { get; set; }
            public float KARTLISATIS { get; set; }
            public float KREDIKARTISATIS { get; set; }
            public float TOPLAMSATIS { get; set; }
            public float YUKLEME { get; set; }
            public float PERSONELKARTISATIS { get; set; }
            public float OGRENCIKARTISATIS { get; set; }
            public float KREDIKARTIYUZDE { get; set; }
            public float KARTLISATISYUZDE { get; set; }
        }
        public class CIRO_ANALIZ_DUNBUGUN
        {
            public int ID { get; set; }
            public string SUBEADI { get; set; }

            public float GUNUNSATISI { get; set; }
            public float DUNUNSATISI { get; set; }
            public string COLOR { get; set; }

        }
        public class PERSONELTURUSAYI
        {
            public int ID { get; set; }
            public int SAYISI { get; set; }
            public string ADI { get; set; }

           

        }
        public class DASHBOARDSAYI
        {
            public int TOPLAMPERSONELSAYI { get; set; }
            public int ARACSAYISI { get; set; }
            public int ANAGUZERGAH { get; set; }
            public int GUZERGAHNOKTASI { get; set; }
           



        }

    }
}