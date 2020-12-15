using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        [Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(300, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Text { get; set; }

        public int MusicaUserId { get; set; }
        public int SharingId { get; set; }
        public virtual MusicaUser MusicaUser { get; set; }
        public virtual Sharing Sharing { get; set; }

    }
}
