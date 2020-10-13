using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.Entities
{
    [Table("Messages")]
    public class Messages
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(300, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        public int ToMusicaUserId { get; set; }

        public virtual List<MessageReplies> MessageReplies { get; set; }

        public Messages()
        {
            this.MessageReplies = new List<MessageReplies>();
        }
    }
}
