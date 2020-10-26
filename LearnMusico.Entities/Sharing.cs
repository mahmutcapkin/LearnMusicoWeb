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
    [Table("Sharings")]
    public class Sharing : MyEntityBase
    {
        [DisplayName("Paylaşım Başlığı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(60, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Title { get; set; }

        [DisplayName("Paylaşım İçeriği"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(500, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Description { get; set; }

        [StringLength(150), ScaffoldColumn(false)]
        public string ImageUrlPath { get; set; }

        [StringLength(150), ScaffoldColumn(false)]
        public string VideoUrlPath { get; set; }


        [DisplayName("Beğeni Sayısı")]
        public int LikeCount { get; set; }

        public virtual MusicaUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Sharing()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
