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
    [Table("MessageReplies")]
    public class MessageReplies
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(300, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Text { get; set; }

        public Guid MessageId { get; set; }

        public int MusicaUserId { get; set; }

        [DisplayName("Oluşturma Tarihi")]
        public DateTime AddedDate { get; set; }

        [DisplayName("Güncelleme Tarihi")]
        public DateTime? ModifiedDate { get; set; }

        public virtual MusicaUser MusicaUser { get; set; }
        public virtual Messages Messages { get; set; }
    }
}
