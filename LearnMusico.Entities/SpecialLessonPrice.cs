using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LearnMusico.Entities
{
    [Table("SpecialLessonPrices")]
    public class SpecialLessonPrice : MyEntityBase
    {
        [DisplayName("Enstrüman Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(40, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string InstrumentName { get; set; }

        [DisplayName("İçerik"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(400, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        [AllowHtml]
        public string Description { get; set; }

        [DisplayName("Özel Ders Fiyatı"), Required]
        public int Price { get; set; }

        [DisplayName("Adres"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(200, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Address { get; set; }

        [StringLength(150), ScaffoldColumn(false)]
        public string ImageFilePath { get; set; }

        public int InstrumentCategoryId { get; set; }

        public virtual MusicaUser Teacher { get; set; }

        public virtual InstrumentCategory InstrumentCategory { get; set; }
    }
}
