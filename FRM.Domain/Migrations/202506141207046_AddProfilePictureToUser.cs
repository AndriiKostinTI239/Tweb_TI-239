namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfilePictureToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.UserEfs", "ProfilePictureUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.UserEfs", "ProfilePictureUrl");
        }
    }
}
