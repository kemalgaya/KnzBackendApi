using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Imza.WebNet.Erp.Models.TableModel
{
    public class GenelTanim
    {
        public partial class IzinTuru
        {
            public int ID { get; set; }

            [DisplayName("Adı")]
            public string ADI { get; set; }
        }


        public class DEF_GENEL
        {
            public int ID { get; set; }

            [Required(ErrorMessage = "Bu Alan Zorunludur.")]
            [DisplayName("Adı")]
            public string ADI { get; set; }
            
            [DisplayName("LOOKUPEDITID")]
            public string LOOKUPEDITID { get; set; }

        }

    }
}