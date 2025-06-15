namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewsCountToThread : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.ThreadEfs", "ViewsCount", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("public.ThreadEfs", "ViewsCount");
        }
    }
}
