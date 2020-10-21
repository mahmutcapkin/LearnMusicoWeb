using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.DataAccessLayer.EntityFramework
{
    public class Initilazier : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
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
                CV="review.pdf",
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
                    About =FakeData.PlaceData.GetAddress(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{a}",
                    ProfileImageFilename = "user.png",
                    Password = "1234",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{a}"
                };
                context.MusicaUsers.Add(user);
            }
            context.SaveChanges();

            List<MusicaUser> userList = context.MusicaUsers.ToList();

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
                for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Instrument instrument = new Instrument()
                    {
                        InstrumentName = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        InstrumentCategory = instcat,
                        ImageFilePath="gitar.jpg",
                        AudioUrlPath= "dummy-audio.mp3",
                        VideoUrlPath="kalimba.mp4",
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    instcat.Instruments.Add(instrument);
                }
            }
            context.SaveChanges();
            


            //Adding fake sharings
            for (int k = 0; k < FakeData.NumberData.GetNumber(5, 6); k++)
            {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    
                    Sharing sharing = new Sharing()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        VideoUrlPath="kalimba.mp4",
                        LikeCount = FakeData.NumberData.GetNumber(1, 5),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    // adding fake comment...
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 6); j++)
                    {
                        MusicaUser owner_comment = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = owner_comment,
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
                            LikedUser = userList[b],
                        };
                        sharing.Likes.Add(liked);
                    }

            }

            //ADDİNG FAKE ARTİCLE AND CATEGORY 
                for (int i = 0; i < 5; i++)
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
                    for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                    {
                        MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Article article = new Article()
                        {
                            Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                            Description = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, " +
                            "similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque" +
                            " nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates " +
                            "repudiandae sint et molestiae non recusandae. Itaque earum rerum " +
                            "hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat At vero eos et accusamus et " +
                            "iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque " +
                            "corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, " +
                            "similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. " +
                            "Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id " +
                            "quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. " +
                            "Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, " +
                            "ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat",
                            ArticleCategory = articlecat,
                            ImageFileName = "gitar.jpg",                       
                            Owner = owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = owner.Username
                        };
                        articlecat.Articles.Add(article);
                    }
                }


            List<InstrumentCategory> catList = context.InstrumentCategories.ToList();
            // adding fake instrumentprice...
            for (int i = 0; i < 6; i++)
            {
               
                InstrumentCategory cat = catList[FakeData.NumberData.GetNumber(0, catList.Count - 1)];
                
                for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    InstrumentPrice instrumentprice = new InstrumentPrice()
                    {
                        InstrumentName = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        InstrumentCategory = cat,
                        ImageFilePath = "gitar.jpg",
                        Price=250,
                        Status="Yeni gibi sayılır",
                        Address= FakeData.PlaceData.GetAddress(),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    cat.InstrumentPrices.Add(instrumentprice);
                }

            }


            // adding fake speciallessonprice...

            for (int i = 0; i < 6; i++)
            {

                InstrumentCategory instcategory = catList[FakeData.NumberData.GetNumber(0, catList.Count - 1)];

                for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                {
                    MusicaUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    SpecialLessonPrice lessonprice = new SpecialLessonPrice()
                    {
                        InstrumentName = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Price = 300,
                        
                        ImageFilePath = "gitar.jpg",
                        Address = FakeData.PlaceData.GetAddress(),
                        Teacher= owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    instcategory.SpecialLessonPrices.Add(lessonprice);
                }

            }






        }


    }
}


