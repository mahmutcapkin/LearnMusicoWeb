using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnMusico.ViewModels
{
    public class LessonPriceViewModel
    {
        public MusicaUser musicaUser { get; set; }
        public List<SpecialLessonPrice> LessonPrices { get; set; }
    }
}