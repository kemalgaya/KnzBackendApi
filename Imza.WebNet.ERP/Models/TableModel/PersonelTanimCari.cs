using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imza.WebNet.ERP.Models.TableModel
{
    public class PersonelTanimCari
    {
        public class CariHesaplar
        {
            public bool SEC { get; set; }

            public int ID { get; set; }

            public string ADI { get; set; }

            public string VERGINO { get; set; }

            public int VERGIDAIREID { get; set; }

            public bool VADESIGECMISODEMEVAR { get; set; }

            public string BANKA { get; set; }

            public string VERGIDAIRE { get; set; }

            public float BORC { get; set; }

            public float ALACAK { get; set; }

            public float BAKIYETUTAR { get; set; }
            public float PERSONELMAAS { get; set; }

            public string BAKIYEDURUMADI { get; set; }

            public string BAKIYEDURUMUBIRLESIK { get; set; }

            public int BANKAID { get; set; }

            public string HESAPNO { get; set; }

            public string IBAN { get; set; }

            public string FAX { get; set; }

            public string EMAIL { get; set; }

            public string WEBSITESI { get; set; }

            public string YETKILIADISOYADI { get; set; }

            public string YETKILIUNVANI { get; set; }

            public string YETKILITELEFON { get; set; }

            public string IL { get; set; }

            public string ILCE { get; set; }

            public string ADRES { get; set; }

            public string TELEFON1 { get; set; }

            public string GRUP1 { get; set; }

            public string GRUP2 { get; set; }

            public string GRUP3 { get; set; }

            public string GRUP4 { get; set; }

            public string GRUP5 { get; set; }

            public string GRUP6 { get; set; }
            public int GRUP1ID { get; set; }
            public int GRUP2ID { get; set; }
            public int GRUP3ID { get; set; }
            public int GRUP4ID { get; set; }
            public int GRUP5ID { get; set; }
            public int GRUP6ID { get; set; }

            public int PERSONELTURUID { get; set; }

            public string PERSONELTURU { get; set; }

            public string EKLEYEN { get; set; }

            public string INSERT_DATE { get; set; }

            public int COMPANYID { get; set; }

            public bool STATE { get; set; }

        }

    }
}