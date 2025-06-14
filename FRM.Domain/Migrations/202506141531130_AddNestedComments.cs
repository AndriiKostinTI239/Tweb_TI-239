namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNestedComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.CommentEfs", "ParentCommentId", c => c.Guid());
            CreateIndex("public.CommentEfs", "ParentCommentId");
            AddForeignKey("public.CommentEfs", "ParentCommentId", "public.CommentEfs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.CommentEfs", "ParentCommentId", "public.CommentEfs");
            DropIndex("public.CommentEfs", new[] { "ParentCommentId" });
            DropColumn("public.CommentEfs", "ParentCommentId");
        }
    }
}
