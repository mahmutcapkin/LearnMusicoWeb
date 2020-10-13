using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.Entities
{
    [Table("InstrumentPrices")]
    public class InstrumentPrice : MyEntityBase
    {
        [DisplayName("Enstrüman Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(40, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string InstrumentName { get; set; }

        [DisplayName("İçerik"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(40, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Description { get; set; }

        [DisplayName("Enstrüman Fiyatı"), Required(ErrorMessage = "{0} alanı gereklidir.")]
        public int Price { get; set; }

        [StringLength(150, ErrorMessage = "{0} max {1} karakter içermelidir."), ScaffoldColumn(false)]
        public string ImageFilePath { get; set; }

        [DisplayName("Enstrüman Durumu"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(40, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Status { get; set; }

        [DisplayName("Adres"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(200, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Address { get; set; }

        public int InstrumentCategoryId { get; set; }

        //Owner_ıd
        public virtual MusicaUser Owner { get; set; }

        public virtual InstrumentCategory InstrumentCategory { get; set; }
    }

}
