namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsBannedToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.UserEfs", "IsBanned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.UserEfs", "IsBanned");
        }
    }
}
