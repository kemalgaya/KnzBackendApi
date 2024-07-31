using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.Erp.Classes
{
    public class MuhasebeEnums
    {
        public struct GelirGiderTuru
        {
            public static int Gelir = 1;
            public static int Gider = 2;
            public static int GelirGider = 3;
        }
        public struct BorcAlacak
        {
            public static int Borc = 1; // BORÇ
            public static int Alacak = 2; // ALACAK
        }
        public struct PersonelTuruId
        {
            public static int Ogrenci = 1;
            public static int Ogretmen = 2;
            public static int ServisSoforu = 3;
            public static int Hostes = 4;
            public static int Personel = 5;
            public static int Firma = 6;
        }
        public struct GrupKategori
        {
            public static int Kantin = 1;
            public static int Kirtasiye = 2;
            public static int Yemekhane = 3;
            public static int Diger = 4;
        }

        public struct BorcAlacakTuru
        {
            public static int FaturaSatis = 1; // Borçlandırılacak
            public static int FaturaAlis = 2; // Alacaklandırılacak

            public static int Odeme = 1;// Borçlandırılacak
            public static int OdemeTekForm = 1;// Borçlandırılacak
            public static int Borclandirma = 1;// Borçlandırılacak
            public static int ServisAidat = 1;// Borçlandırılacak
            public static int Alacaklandir = 2;// Alacaklandır
            public static int Tahsilat = 2;// Alacaklandır
            public static int Giderler = 2; // Alacaklandırılacak
            public static int TahsilatTekForm = 2;// Alacaklandır
        }

        public struct FormTuru
        {
            public static int FaturaSatis = 1; // Borçlandırılacak
            public static int FaturaAlis = 2; // Alacaklandırılacak
            public static int Odeme = 3;// Borçlandırılacak
            public static int Borclandirma = 4;// Borçlandırılacak
            public static int Tahsilat = 5;// Alacaklandır
            public static int Alacaklandir = 6;// Alacaklandır
            public static int ServisAidat = 7;// Alacaklandır
            public static int Giderler = 8;// Borçlandırılacak
            public static int HizliSatis = 9;// Borçlandırılacak
            public static int KrediYukleme = 10;// Borçlandırılacak
            public static int TahsilatAidat = 11;// Alacaklandır
            public static int TahsilatTekForm = 12;// Alacaklandır
            public static int OdemeTekForm = 13;// Borçlandırılacak


            public static int DepoAktarim = 14;// Borçlandırılacak
        }
    }
}