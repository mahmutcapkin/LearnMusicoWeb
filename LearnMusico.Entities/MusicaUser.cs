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
    [Table("MusicaUsers")]
    public class MusicaUser : MyEntityBase
    {
        [DisplayName("Ad"), StringLength(25, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Name { get; set; }

        [DisplayName("Soyad"), StringLength(50, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Surname { get; set; }

        [DisplayName("Hakkımda"), StringLength(500, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string About { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(25, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Username { get; set; }

        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(50, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(150, ErrorMessage = "{0} max {1} karakter içermelidir.")]
        public string Password { get; set; }

        //profil resminin üretilmesi ekrana gelmesi istenmezse böyle olur kendi profilinden aktif edicek demek bu
        [StringLength(150, ErrorMessage = "{0} max {1} karakter içermelidir."), ScaffoldColumn(false)]//  images/user_12.jgp   falan gelicek şekilde
        public string ProfileImageFilename { get; set; }

        [StringLength(150, ErrorMessage = "{0} max {1} karakter içermelidir."), ScaffoldColumn(false)]//  images/user_12.jgp   falan gelicek şekilde
        public string CV { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        [DisplayName("Is Teacher")]
        public bool IsTeacher { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        public virtual List<Article> Articles { get; set; }
        public virtual List<Sharing> Sharings { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<InstrumentPrice> InstrumentPrices { get; set; }
        public virtual List<SpecialLessonPrice> SpecialLessonPrices { get; set; }
        public virtual List<MessageReplies> MessageReplies { get; set; }
        public virtual List<Liked> Likes { get; set; }
        public virtual List<Instrument> Instruments { get; set; }

        public MusicaUser()
        {
            Articles = new List<Article>();
            Sharings = new List<Sharing>();
            Comments = new List<Comment>();
            Instruments = new List<Instrument>();
            Likes = new List<Liked>();
            MessageReplies = new List<MessageReplies>();
            SpecialLessonPrices = new List<SpecialLessonPrice>();
            InstrumentPrices = new List<InstrumentPrice>();
        }

    }
}
