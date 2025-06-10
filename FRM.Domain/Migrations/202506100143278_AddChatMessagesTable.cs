namespace FRM.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChatMessagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Text = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Messages");
        }
    }
}
