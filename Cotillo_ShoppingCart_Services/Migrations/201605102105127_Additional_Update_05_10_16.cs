namespace Cotillo_ShoppingCart_Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Additional_Update_05_10_16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PriceIncTax", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "PriceExcTax", c => c.Double());
            AddColumn("dbo.Products", "Description", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "ExternalAccount", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ExternalAccount");
            DropColumn("dbo.Products", "Description");
            DropColumn("dbo.Products", "PriceExcTax");
            DropColumn("dbo.Products", "PriceIncTax");
        }
    }
}
