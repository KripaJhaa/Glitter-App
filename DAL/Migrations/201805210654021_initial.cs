namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hashtag",
                c => new
                    {
                        HashTagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        UserId = c.String(maxLength: 128),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HashTagId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        EmailId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        CountryOfOrigin = c.String(nullable: false),
                        Image = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EmailId);
            
            CreateTable(
                "dbo.Like",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        TweetId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsLiked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.Tweet", t => t.TweetId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.TweetId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tweet",
                c => new
                    {
                        TweetId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TweetId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLink",
                c => new
                    {
                        UserLinkId = c.Int(nullable: false, identity: true),
                        FolloweeId = c.String(maxLength: 128),
                        FollowerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserLinkId)
                .ForeignKey("dbo.User", t => t.FolloweeId)
                .ForeignKey("dbo.User", t => t.FollowerId)
                .Index(t => t.FolloweeId)
                .Index(t => t.FollowerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLink", "FollowerId", "dbo.User");
            DropForeignKey("dbo.UserLink", "FolloweeId", "dbo.User");
            DropForeignKey("dbo.Like", "UserId", "dbo.User");
            DropForeignKey("dbo.Like", "TweetId", "dbo.Tweet");
            DropForeignKey("dbo.Tweet", "UserId", "dbo.User");
            DropForeignKey("dbo.Hashtag", "UserId", "dbo.User");
            DropIndex("dbo.UserLink", new[] { "FollowerId" });
            DropIndex("dbo.UserLink", new[] { "FolloweeId" });
            DropIndex("dbo.Tweet", new[] { "UserId" });
            DropIndex("dbo.Like", new[] { "UserId" });
            DropIndex("dbo.Like", new[] { "TweetId" });
            DropIndex("dbo.Hashtag", new[] { "UserId" });
            DropTable("dbo.UserLink");
            DropTable("dbo.Tweet");
            DropTable("dbo.Like");
            DropTable("dbo.User");
            DropTable("dbo.Hashtag");
        }
    }
}
