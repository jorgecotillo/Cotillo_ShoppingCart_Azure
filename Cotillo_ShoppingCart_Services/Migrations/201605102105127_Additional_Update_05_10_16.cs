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
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "PriceExcTax");
            DropColumn("dbo.Products", "PriceIncTax");
        }
    }
}
