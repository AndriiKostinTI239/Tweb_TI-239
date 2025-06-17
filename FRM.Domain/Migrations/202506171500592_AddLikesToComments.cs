namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLikesToComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.LikeEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CommentId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.CommentEfs", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("public.UserEfs", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.LikeEfs", "UserId", "public.UserEfs");
            DropForeignKey("public.LikeEfs", "CommentId", "public.CommentEfs");
            DropIndex("public.LikeEfs", new[] { "CommentId" });
            DropIndex("public.LikeEfs", new[] { "UserId" });
            DropTable("public.LikeEfs");
        }
    }
}
