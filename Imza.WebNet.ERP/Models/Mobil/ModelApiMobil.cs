using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Imza.WebNet.Erp.Models.Mobil
{
    #region IMZA


    public class getAllPersonelLookUpData
    {
        public IList PERSONEL_TANIM { get; set; }

    }

    public class getDashboardData
    {
        public IList PERSONELTURUSAYI { get; set; }
        public IList DASHBOARDSAYI { get; set; }

    }
    public class getAllGuzergahData
    {
        public IList PERSONEL_GUZERGAH { get; set; }
        public IList PERSONEL_GUZERGAHDETAY { get; set; }

    }
    public class getPersonelTanimData
    {
        public IList PERSONEL_TANIM { get; set; }

    }
    public class getServisHakedisData
    {
        public IList DEF_SERVIS_HAKEDIS { get; set; }

    }
    public class getPersonelTanimCariData
    {
        public IList CariHesaplar { get; set; }

    }
    public class getPersonelTanimCariKantinData
    {
        public IList CariHesaplarKantin { get; set; }

    }
    public class getMuhasebeFaturaData
    {
        public IList MUHASEBE_FATURA { get; set; }

    }
    public class getMuhasebeFaturaDetayData
    {
        public IList MUHASEBE_FATURADETAY { get; set; }

    }
    public class getPersonelGuzergahData
    {
        public IList PERSONEL_GUZERGAH { get; set; }
    }
    public class getPersonelGuzergahDetayData
    {
        public IList PERSONEL_GUZERGAHDETAY { get; set; }
    }
    public class getDonemData
    {
        public IList DEF_DONEM { get; set; }
    }
    public class getServisAracData
    {
        public IList DEF_PERSONEL_SERVISARAC { get; set; }
    }
    public class getMuhasebeHizmetUrunData
    {
        public IList DEF_MUHASEBE_HIZMETURUN { get; set; }
    }
    public class getMuhasebeKasaData
    {
        public IList DEF_MUHASEBE_KASA { get; set; }
    }
    public class getMuhasebeGiderGelirData
    {
        public IList DEF_MUHASEBE_GIDERGELIR { get; set; }
    }

    public class getPersonelDosyaEkiData
    {
        public IList PERSONEL_DOSYAEKI { get; set; }
    }
    public class getPersonelUyariData
    {
        public IList PERSONEL_UYARILAR { get; set; }
    }
    public class getVeliKullaniciData
    {
        public IList DEF_VELI_KULLANICI { get; set; }
    }
    public class getDEF_GENELData
    {
        public IList DEF_GENEL { get; set; }
    }
    public class getTableAuthUsersData
    {
        public IList AUTH_USERS { get; set; }

    }
    public class getTableAuthGroupsData
    {
        public IList AUTH_GROUPS { get; set; }

    }
    public class getTableAppParameterGroupsData
    {
        public IList APP_PARAMETER_GROUPS { get; set; }

    }
    public class getTableAppParametersData
    {
        public IList APP_PARAMETERS { get; set; }

    }
    public class getTableAuthAuthorizeUsersWebData
    {
        public IList APP_FORMSWEB { get; set; }

    }

    #endregion

    #region KNZ
    public class getAllSliderData
    {
        public IList KNZ_SLIDER { get; set; }

    }

    public class getAllAboutData
    {
        public IList KNZ_ABOUT { get; set; }

    }


    public class getAllBlogData
    {
        public IList KNZ_BLOG { get; set; }

    }
    public class getAllContactData
    {
        public IList KNZ_CONTACT { get; set; }

    }
    public class getAllCommentData
    {
        public IList KNZ_COMMENT { get; set; }

    }
    public class getAllCustomerData
    {
        public IList KNZ_CUSTOMER { get; set; }

    }
    public class getAllUrunData
    {
        public IList KNZ_URUNLER { get; set; }

    }
    public class getAllUrunGorselData
    {
        public IList KNZ_URUNGORSEL { get; set; }

    }
    public class getAllKampanyalarData
    {
        public IList KNZ_KAMPANYALAR { get; set; }

    }
    public class getAllReferansData
    {
        public IList KNZ_REFERANS { get; set; }

    }
    public class getAllDosyaEkiData
    {
        public IList KNZ_DOSYAEKI { get; set; }

    }
    public class getAllCategoriesData
    {
        public IList CATEGORIES { get; set; }

    }
    public class getAllMessagesData
    {
        public IList MESSAGE { get; set; }

    }
    public class getAllPostsData
    {
        public IList POSTS { get; set; }

    }
    public class getAllProductsData
    {
        public IList PRODUCTS { get; set; }

    }
    public class getAllProductsHomeData
    {
        public IList PRODUCTSHOME { get; set; }

    }
    public class getAllSiparisData
    {
        public IList SIPARIS { get; set; }

    }
    public class getAllSiparisDetayData
    {
        public IList SIPARISDETAY { get; set; }

    }



    #endregion





    public class getAllPersonel
    {

        public int isChecked { get; set; }
        public string ID { get; set; }

        [DisplayName("Adı Soyadı")]
        public string ADISOYADI { get; set; }

        [DisplayName("Doğum Tarihi")]
        public string DOGUMTARIHI { get; set; }

        [DisplayName("Yaşı")]
        public string YAS { get; set; }


        [DisplayName("Uyruk")]
        public string UYRUKADI { get; set; }


        [DisplayName("Kimlik Tür")]
        public string KIMLIKTURUADI { get; set; }


        [DisplayName("Çalışma İzni")]
        public string CALISMAIZNI { get; set; }


        [DisplayName("Çalışma İzni Tarihi")]
        public string CALISMAIZNITARIHI { get; set; }


        [DisplayName("Sosyal Güv No")]
        public string SOSYALGUVENLIKNO { get; set; }

        [DisplayName("Saat Ücreti")]
        public string SAATLIKUCRETADI { get; set; }

        [DisplayName("Proje")]
        public string PROJEADI { get; set; }

        [DisplayName("Şirket Formu")]
        public string SIRKETFORMUADI { get; set; }

        [DisplayName("Sözleşme Tarihi")]
        public string SOZLESMETARIHI { get; set; }

        [DisplayName("KVK/WKA")]
        public string KVK_WKA { get; set; }

        [DisplayName("Firma")]
        public string FIRMAADI { get; set; }

        public bool STATE { get; set; }

        public string USERID { get; set; }


        //public IList PERSONEL_TANIM { get; set; }
        //public IList DEF_RES_MASALAR { get; set; }
        //public IList DEF_RES_MENU_KATEGORI { get; set; }
        //public IList DEF_MUHASEBE_HIZMETURUN { get; set; }
        //public IList RES_ADISYON { get; set; }
        //public IList RES_ADISYONDETAY { get; set; }
    }




    public class APIPERSONEL_DEVAMKONTROL
    {
        public string PROJEID { get; set; }
        public string KULLANICIID { get; set; }
        public string saat { get; set; }
        public string tarih { get; set; }
        public string CALISMASURESI { get; set; }

        //[JsonConverter(typeof(CollectionWithNamedElementsConverter))]
        //public IList PERSONEL_DEVAMKONTROL { get; set; }
        public List<PERSONEL_LIST> PERSONELID { get; set; }
    }

    public class APIPERSONEL_DEVAMKONTROLHAFTALIK
    {
        public string PROJEID { get; set; }
        public string YIL { get; set; }
        public string HAFTANO { get; set; }
        public string KULLANICIID { get; set; }
        public string saat { get; set; }
        public string CALISMASURESI { get; set; }

        //[JsonConverter(typeof(CollectionWithNamedElementsConverter))]
        //public IList PERSONEL_DEVAMKONTROL { get; set; }
        public List<PERSONEL_LIST> PERSONELID { get; set; }
    }

    public class GETALLDEVAMKONTROL
    {
        public string KULLANICIID { get; set; }
        public string saat { get; set; }
        public string tarih { get; set; }
        public string CALISMASURESI { get; set; }
        //[JsonConverter(typeof(CollectionWithNamedElementsConverter))]
        //public IList PERSONEL_DEVAMKONTROL { get; set; }

        public List<PERSONEL_DEVAMKONTROL> PERSONELID { get; set; }
    }


    public class PERSONEL_LIST
    {
        public int ID { get; set; }
    }


    public class PERSONEL_DEVAMKONTROL
    {
        public int ID { get; set; }

        [DisplayName("Personel")]
        public int PERSONELID { get; set; }

        public int SAATUCRETI { get; set; }

        public string CALISMASURESI { get; set; }

        public string MESAITARIHI { get; set; }

        public string DATE_START { get; set; }

        public string DATE_END { get; set; }

        public string USERID { get; set; }
    }
}