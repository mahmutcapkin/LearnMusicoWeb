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
    [Table("InstrumentCategories")]
    public class InstrumentCategory : MyEntityBase
    {
        [DisplayName("Enstrüman Kategorisi"),
             Required(ErrorMessage = "{0} alanı gereklidir."),
             StringLength(50, ErrorMessage = "{0} max {1} karakter içermeli.")]
        public string Title { get; set; }

        public virtual List<Instrument> Instruments { get; set; }
        public virtual List<InstrumentPrice> InstrumentPrices { get; set; }
        public virtual List<SpecialLessonPrice> SpecialLessonPrices { get; set; }

        public InstrumentCategory()
        {
            Instruments = new List<Instrument>();
            InstrumentPrices = new List<InstrumentPrice>();
            SpecialLessonPrices = new List<SpecialLessonPrice>();
        }
    }
}
