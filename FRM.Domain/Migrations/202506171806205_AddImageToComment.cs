namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageToComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.CommentEfs", "ImageUrl", c => c.String());
            AddColumn("public.ThreadEfs", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.ThreadEfs", "ImageUrl");
            DropColumn("public.CommentEfs", "ImageUrl");
        }
    }
}
