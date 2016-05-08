namespace Cotillo_ShoppingCart_Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableUpdate_050516 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address1 = c.String(maxLength: 255),
                        Address2 = c.String(maxLength: 255),
                        AddressType = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        StateProvinceId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.States", t => t.StateProvinceId)
                .Index(t => t.CountryId)
                .Index(t => t.StateProvinceId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        CountryId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Catogories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        FileName = c.String(nullable: false, maxLength: 100),
                        Barcode = c.String(nullable: false, maxLength: 20),
                        ExpiresOn = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catogories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 255),
                        Password = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 255),
                        BillingAddressId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.BillingAddressId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.BillingAddressId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Catogories");
            DropForeignKey("dbo.Addresses", "StateProvinceId", "dbo.States");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropIndex("dbo.Customers", new[] { "UserId" });
            DropIndex("dbo.Customers", new[] { "BillingAddressId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "StateProvinceId" });
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Users");
            DropTable("dbo.Products");
            DropTable("dbo.Catogories");
            DropTable("dbo.States");
            DropTable("dbo.Countries");
            DropTable("dbo.Addresses");
        }
    }
}
