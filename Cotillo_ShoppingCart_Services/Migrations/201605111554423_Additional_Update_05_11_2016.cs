namespace Cotillo_ShoppingCart_Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Additional_Update_05_11_2016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Catogories", "Location", c => c.String(nullable: false, maxLength: 155));
            AddColumn("dbo.Products", "Location", c => c.String(nullable: false, maxLength: 155));
            AddColumn("dbo.Users", "DisplayName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Users", "DisplayName");
            DropColumn("dbo.Products", "Location");
            DropColumn("dbo.Catogories", "Location");
        }
    }
}
