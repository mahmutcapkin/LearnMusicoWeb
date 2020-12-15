namespace LearnMusico.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        SubjectType = c.String(nullable: false, maxLength: 60),
                        Description = c.String(nullable: false, maxLength: 4000),
                        ImageFileName = c.String(maxLength: 200),
                        ArticleCategoryId = c.Int(nullable: false),
                        MusicaUserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId, cascadeDelete: true)
                .Index(t => t.ArticleCategoryId)
                .Index(t => t.MusicaUserId);
            
            CreateTable(
                "dbo.MusicaUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Surname = c.String(maxLength: 50),
                        About = c.String(maxLength: 500),
                        Username = c.String(nullable: false, maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 150),
                        ProfileImageFilename = c.String(maxLength: 150),
                        CV = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        IsTeacher = c.Boolean(nullable: false),
                        ActivateGuid = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 300),
                        MusicaUserId = c.Int(nullable: false),
                        SharingId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId)
                .ForeignKey("dbo.Sharings", t => t.SharingId, cascadeDelete: true)
                .Index(t => t.MusicaUserId)
                .Index(t => t.SharingId);
            
            CreateTable(
                "dbo.Sharings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        Description = c.String(nullable: false, maxLength: 500),
                        ImageUrlPath = c.String(maxLength: 150),
                        VideoUrlPath = c.String(maxLength: 150),
                        LikeCount = c.Int(nullable: false),
                        MusicaUserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId, cascadeDelete: true)
                .Index(t => t.MusicaUserId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MusicaUserId = c.Int(nullable: false),
                        SharingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId)
                .ForeignKey("dbo.Sharings", t => t.SharingId, cascadeDelete: true)
                .Index(t => t.MusicaUserId)
                .Index(t => t.SharingId);
            
            CreateTable(
                "dbo.InstrumentPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstrumentName = c.String(nullable: false, maxLength: 40),
                        Description = c.String(nullable: false, maxLength: 400),
                        Price = c.Int(nullable: false),
                        ImageFilePath = c.String(maxLength: 150),
                        Status = c.String(nullable: false, maxLength: 40),
                        Address = c.String(nullable: false, maxLength: 200),
                        InstrumentCategoryId = c.Int(nullable: false),
                        MusicaUserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InstrumentCategories", t => t.InstrumentCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId, cascadeDelete: true)
                .Index(t => t.InstrumentCategoryId)
                .Index(t => t.MusicaUserId);
            
            CreateTable(
                "dbo.InstrumentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Instruments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstrumentName = c.String(nullable: false, maxLength: 40),
                        Description = c.String(nullable: false),
                        ImageFilePath = c.String(maxLength: 150),
                        VideoUrlPath = c.String(maxLength: 250),
                        AudioUrlPath = c.String(maxLength: 250),
                        InstrumentCategoryId = c.Int(nullable: false),
                        MusicaUserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InstrumentCategories", t => t.InstrumentCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId, cascadeDelete: true)
                .Index(t => t.InstrumentCategoryId)
                .Index(t => t.MusicaUserId);
            
            CreateTable(
                "dbo.SpecialLessonPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstrumentName = c.String(nullable: false, maxLength: 40),
                        Description = c.String(nullable: false, maxLength: 400),
                        Price = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 200),
                        ImageFilePath = c.String(maxLength: 150),
                        InstrumentCategoryId = c.Int(nullable: false),
                        MusicaUserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InstrumentCategories", t => t.InstrumentCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId, cascadeDelete: true)
                .Index(t => t.InstrumentCategoryId)
                .Index(t => t.MusicaUserId);
            
            CreateTable(
                "dbo.MessageReplies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false, maxLength: 300),
                        MessagesId = c.Guid(nullable: false),
                        MusicaUserId = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessagesId, cascadeDelete: true)
                .ForeignKey("dbo.MusicaUsers", t => t.MusicaUserId, cascadeDelete: true)
                .Index(t => t.MessagesId)
                .Index(t => t.MusicaUserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 300),
                        ToMusicaUserId = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageReplies", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.MessageReplies", "MessagesId", "dbo.Messages");
            DropForeignKey("dbo.InstrumentPrices", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.SpecialLessonPrices", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.SpecialLessonPrices", "InstrumentCategoryId", "dbo.InstrumentCategories");
            DropForeignKey("dbo.Instruments", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.Instruments", "InstrumentCategoryId", "dbo.InstrumentCategories");
            DropForeignKey("dbo.InstrumentPrices", "InstrumentCategoryId", "dbo.InstrumentCategories");
            DropForeignKey("dbo.Sharings", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.Likes", "SharingId", "dbo.Sharings");
            DropForeignKey("dbo.Likes", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.Comments", "SharingId", "dbo.Sharings");
            DropForeignKey("dbo.Comments", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.Articles", "MusicaUserId", "dbo.MusicaUsers");
            DropForeignKey("dbo.Articles", "ArticleCategoryId", "dbo.ArticleCategories");
            DropIndex("dbo.MessageReplies", new[] { "MusicaUserId" });
            DropIndex("dbo.MessageReplies", new[] { "MessagesId" });
            DropIndex("dbo.SpecialLessonPrices", new[] { "MusicaUserId" });
            DropIndex("dbo.SpecialLessonPrices", new[] { "InstrumentCategoryId" });
            DropIndex("dbo.Instruments", new[] { "MusicaUserId" });
            DropIndex("dbo.Instruments", new[] { "InstrumentCategoryId" });
            DropIndex("dbo.InstrumentPrices", new[] { "MusicaUserId" });
            DropIndex("dbo.InstrumentPrices", new[] { "InstrumentCategoryId" });
            DropIndex("dbo.Likes", new[] { "SharingId" });
            DropIndex("dbo.Likes", new[] { "MusicaUserId" });
            DropIndex("dbo.Sharings", new[] { "MusicaUserId" });
            DropIndex("dbo.Comments", new[] { "SharingId" });
            DropIndex("dbo.Comments", new[] { "MusicaUserId" });
            DropIndex("dbo.Articles", new[] { "MusicaUserId" });
            DropIndex("dbo.Articles", new[] { "ArticleCategoryId" });
            DropTable("dbo.Messages");
            DropTable("dbo.MessageReplies");
            DropTable("dbo.SpecialLessonPrices");
            DropTable("dbo.Instruments");
            DropTable("dbo.InstrumentCategories");
            DropTable("dbo.InstrumentPrices");
            DropTable("dbo.Likes");
            DropTable("dbo.Sharings");
            DropTable("dbo.Comments");
            DropTable("dbo.MusicaUsers");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleCategories");
        }
    }
}
