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
    [Table("ArticleCategories")]
    public class ArticleCategory : MyEntityBase
    {
        [DisplayName("Yazı Kategorisi"),
             Required(ErrorMessage = "{0} alanı gereklidir."),
             StringLength(50, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Title { get; set; }

        public virtual List<Article> Articles { get; set; }

        public ArticleCategory()
        {
            Articles = new List<Article>();
        }

    }
}
