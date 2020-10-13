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
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Oluşturma Tarihi"), Required, ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Güncelleme Tarihi"), Required, ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Güncelleyen"), Required, StringLength(30), ScaffoldColumn(false)]
        public string ModifiedUsername { get; set; }
    }
}
