namespace LearnMusico.DataAccessLayer.Migrations
{
    using LearnMusico.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LearnMusico.DataAccessLayer.EntityFramework.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LearnMusico.DataAccessLayer.EntityFramework.DatabaseContext context)
        {
            MusicaUser admin = new MusicaUser()
            {
                Name = "Ahmet",
                Surname = "Kılıç",
                Email = "ahmetkilic@gmail.com",
                About = "merhaba benim adım ahmet LearnMusico da admin olarak işlem yapmaktayım.",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                CV = "review.pdf",
                IsTeacher = false,
                Username = "ahmetkilic",
                ProfileImageFilename = "user_default.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "ahmetkilic"
            };
            MusicaUser standartUser = new MusicaUser()
            {
                Name = "Mustafa",
                Surname = "Kırca",
                Email = "mustafakirca@gmail.com",
                About = "merhaba benim adım mustafa öğrenciyim gitar çalmayı öğrenmek istiyorum",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                CV = "review.pdf",
                IsTeacher = false,
                Username = "mustafakirca",
                ProfileImageFilename = "user_default.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "mustafakirca"
            };
            MusicaUser teachUser = new MusicaUser()
            {
                Name = "Ali",
                Surname = "Akıncı",
                Email = "aliakinci@gmail.com",
                About = "merhaba benim adım Ali bağlama eğitmeniyim 3 yıldır bu işle ilgileniyorum.",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                CV = "review.pdf",
                IsTeacher = true,
                Username = "aliakinci",
                ProfileImageFilename = "user_default.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "aliakinci"
            };
            context.MusicaUsers.Add(admin);
            context.MusicaUsers.Add(standartUser);
            context.MusicaUsers.Add(teachUser);

            for (int a = 0; a < 5; a++)
            {
                MusicaUser user = new MusicaUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    About = FakeData.PlaceData.GetAddress(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{a}",
                    ProfileImageFilename = "user_default.png",
                    Password = "1234",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{a}"
                };
                context.MusicaUsers.Add(user);
            }
            context.SaveChanges();

            List<MusicaUser> userList = context.MusicaUsers.ToList();


            //ADDİNG FAKE ARTİCLE AND CATEGORY 
            for (int f = 0; f < 5; f++)
            {
                ArticleCategory articlecat = new ArticleCategory()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "ahmetkilic"
                };
                context.ArticleCategories.Add(articlecat);
                // adding fake Article...
                for (int k = 0; k < 5; k++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Article article = new Article()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(2, 5)),
                        Description = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, " +
                        "facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque" +
                        "Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, " +
                        "ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat",
                        ArticleCategory = articlecat,
                        ArticleCategoryId = articlecat.Id,
                        SubjectType = "haber",
                        ImageFileName = "haber.jpg",
                        MusicaUser = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    articlecat.Articles.Add(article);
                }
            }


            //Adding fake sharings
            for (int k = 0; k < 5; k++)
            {
                MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                Sharing sharing = new Sharing()
                {
                    Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(1, 5)),
                    Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                    VideoUrlPath = "kalimba.mp4",
                    ImageUrlPath = "keman.jpg",
                    LikeCount = FakeData.NumberData.GetNumber(1, 5),
                    MusicaUser = owner,
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = owner.Username
                };
                context.Sharings.Add(sharing);
                // adding fake comment...
                for (int j = 0; j < 5; j++)
                {
                    MusicaUser owner_comment = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Comment comment = new Comment()
                    {
                        Text = FakeData.TextData.GetSentence(),
                        MusicaUser = owner_comment,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner_comment.Username
                    };
                    sharing.Comments.Add(comment);
                }
                // adding fake likes...

                for (int b = 0; b < sharing.LikeCount; b++)
                {
                    Liked liked = new Liked()
                    {
                        MusicaUser = userList[b],
                    };
                    sharing.Likes.Add(liked);
                }

            }

            // ADDİNG FAKE INSTRUMENT AND INST CATEGORY
            for (int i = 0; i < 5; i++)
            {
                InstrumentCategory instcat = new InstrumentCategory()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "ahmetkilic"
                };
                context.InstrumentCategories.Add(instcat);
                // adding fake instrument...
                for (int m = 0; m < 5; m++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Instrument instrument = new Instrument()
                    {
                        InstrumentName = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(1, 3)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        InstrumentCategory = instcat,
                        ImageFilePath = "gitar.jpg",
                        AudioUrlPath = "dummy-audio.mp3",
                        VideoUrlPath = "kalimba.mp4",
                        MusicaUser = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    instcat.Instruments.Add(instrument);
                }
            }
            context.SaveChanges();

            List<InstrumentCategory> instcatList = context.InstrumentCategories.ToList();
            // adding fake instrumentprice...
            for (int i = 0; i < 5; i++)
            {

                InstrumentCategory cat = instcatList[i];

                for (int k = 0; k < 5; k++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    InstrumentPrice instrumentprice = new InstrumentPrice()
                    {
                        InstrumentName = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(1, 2)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        InstrumentCategory = cat,
                        ImageFilePath = "gitar.jpg",
                        Price = 250,
                        Status = "Yeni gibi sayılır",
                        Address = FakeData.PlaceData.GetAddress(),
                        MusicaUser = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    cat.InstrumentPrices.Add(instrumentprice);
                }

            }


            // adding fake speciallessonprice...

            for (int i = 0; i < 5; i++)
            {

                InstrumentCategory cat = instcatList[i];

                for (int k = 0; k < 5; k++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    SpecialLessonPrice lessonprice = new SpecialLessonPrice()
                    {
                        InstrumentName = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(1, 2)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Price = 300,

                        ImageFilePath = "gitar.jpg",
                        Address = FakeData.PlaceData.GetAddress(),
                        MusicaUser = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    cat.SpecialLessonPrices.Add(lessonprice);
                }

            }

            context.SaveChanges();
        }
    }
}
