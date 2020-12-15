using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.Entities
{
    [Table("Likes")]
    public class Liked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MusicaUserId { get; set; }
        public int SharingId { get; set; }
        public virtual MusicaUser MusicaUser { get; set; }
        public virtual Sharing Sharing { get; set; }
    }
}
