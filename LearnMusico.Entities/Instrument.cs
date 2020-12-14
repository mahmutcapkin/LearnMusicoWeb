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
    [Table("Instruments")]
    public class Instrument : MyEntityBase
    {
        [DisplayName("Enstrüman Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(40, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string InstrumentName { get; set; }

        [DisplayName("Açıklama"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(20000, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        [AllowHtml]
        public string Description { get; set; }

        [StringLength(150), ScaffoldColumn(false)]
        public string ImageFilePath { get; set; }

        [StringLength(250), ScaffoldColumn(false)]
        public string VideoUrlPath { get; set; }

        [StringLength(250), ScaffoldColumn(false)]
        public string AudioUrlPath { get; set; }

        public int InstrumentCategoryId { get; set; }


        public virtual InstrumentCategory InstrumentCategory { get; set; }

        public virtual MusicaUser Owner { get; set; }

    }
}
