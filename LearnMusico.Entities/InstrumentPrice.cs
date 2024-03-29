﻿using System;
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
    [Table("InstrumentPrices")]
    public class InstrumentPrice : MyEntityBase
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

        [DisplayName("Enstrüman Fiyatı"), Required(ErrorMessage = "{0} alanı gereklidir."), Range(0, 100000, ErrorMessage = "{0} min {1} değeri almak zorundadır.")]
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
        public int MusicaUserId { get; set; }

        public virtual MusicaUser MusicaUser { get; set; }

        public virtual InstrumentCategory InstrumentCategory { get; set; }
    }

}
