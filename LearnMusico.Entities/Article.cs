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
    [Table("Articles")]
    public class Article : MyEntityBase
    {
        [DisplayName("Yazı Başlığı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(60, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Title { get; set; }

        [DisplayName("Yazı Türü"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(60, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string SubjectType { get; set; }

        [DisplayName("İçerik"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(4000, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Description { get; set; }

        [StringLength(200, ErrorMessage = "{0} max {1} karakter içermelidir."), ScaffoldColumn(false)]
        public string ImageFileName { get; set; }

        public int ArticleCategoryId { get; set; }

        public virtual ArticleCategory ArticleCategory { get; set; }

        public virtual MusicaUser Owner { get; set; }
    }

}
