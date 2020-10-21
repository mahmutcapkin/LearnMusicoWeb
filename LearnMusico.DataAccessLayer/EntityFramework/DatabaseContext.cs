using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentPrice> InstrumentPrices { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public DbSet<MessageReplies> MessageReplies { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<MusicaUser> MusicaUsers { get; set; }
        public DbSet<Sharing> Sharings { get; set; }
        public DbSet<SpecialLessonPrice> SpecialLessonPrices { get; set; }
        public DbSet<InstrumentCategory> InstrumentCategories { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }


        public DatabaseContext()
        {
            Database.SetInitializer(new Initilazier());
        }
    }
}
