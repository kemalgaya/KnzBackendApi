using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class MuhasebeFatura
    {
        public class MUHASEBE_FATURA
        {
            public int ID { get; set; }

            public string CARI { get; set; }

            public string GELIRTURU { get; set; }

            public string GIDERTURU { get; set; }

            public string PERSONELTURU { get; set; }

            public string PERSONELTURUID { get; set; }

            public int GELIRTURUID { get; set; }

            public int GIDERTURUID { get; set; }

            public string KATEGORI { get; set; }

            public int FATURAADEDI { get; set; }

            public float BORC { get; set; }

            public float ALACAK { get; set; }

            public int CARIID { get; set; }

            public int KATEGORIID { get; set; }

            public string FATURAMUHATABI { get; set; }

            public string ISLEMTARIHI { get; set; }

            public string FATURASERINO { get; set; }

            public string FATURANO { get; set; }

            public string FATURATARIHI { get; set; }

            public string VADETARIHI { get; set; }

            public int BANKAID { get; set; }

            public string BANKAODEMEIBANNO { get; set; }

            public string BANKAODEMEHESAPNO { get; set; }

            public bool ISODENDI { get; set; }

            public int ODEMEID { get; set; }

            public int BORCALACAKID { get; set; }

            public string EKLEYEN { get; set; }

            public int INSERT_USERID { get; set; }

            public string INSERT_DATE { get; set; }

            public bool STATE { get; set; }

            public int COMPANYID { get; set; }

            public string FORMTURU { get; set; }

            public int FORMTURUID { get; set; }

            public string ODEMETURU { get; set; }

            public int ODEMETURUID { get; set; }

            public string KREDIKARTI { get; set; }

            public string KREDIKARTISAHIBI { get; set; }

            public string KREDIKARTIBANKA { get; set; }

            public string KREDIKARTITARIH { get; set; }

            public string BANKAADI { get; set; }

            public string BANKAGONDEREN { get; set; }

            public string BANKAHESAPNO { get; set; }

            public string BANKATARIH { get; set; }

            public string OKUL { get; set; }

            public int OKULID { get; set; }

            public string SERVISARAC { get; set; }
            public int SERVISARACID { get; set; }

            public string ACIKLAMA { get; set; }

            public int DONEMID { get; set; }
            public int ILISKILICARIID { get; set; }

        }

    }
}