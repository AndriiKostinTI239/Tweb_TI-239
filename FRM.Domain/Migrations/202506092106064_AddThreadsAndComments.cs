namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThreadsAndComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.CommentEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Content = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ThreadId = c.Guid(nullable: false),
                        AuthorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.UserEfs", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("public.ThreadEfs", t => t.ThreadId, cascadeDelete: true)
                .Index(t => t.ThreadId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "public.ThreadEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        AuthorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.UserEfs", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.CommentEfs", "ThreadId", "public.ThreadEfs");
            DropForeignKey("public.ThreadEfs", "AuthorId", "public.UserEfs");
            DropForeignKey("public.CommentEfs", "AuthorId", "public.UserEfs");
            DropIndex("public.ThreadEfs", new[] { "AuthorId" });
            DropIndex("public.CommentEfs", new[] { "AuthorId" });
            DropIndex("public.CommentEfs", new[] { "ThreadId" });
            DropTable("public.ThreadEfs");
            DropTable("public.CommentEfs");
        }
    }
}
