namespace Cotillo_ShoppingCart_Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Load_05_12_2016 : DbMigration
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
                        Location = c.String(nullable: false, maxLength: 155),
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
                        PriceIncTax = c.Double(nullable: false),
                        PriceExcTax = c.Double(),
                        Description = c.String(nullable: false, maxLength: 255),
                        Location = c.String(nullable: false, maxLength: 155),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catogories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        PriceIncTax = c.Double(nullable: false),
                        PriceExcTax = c.Double(),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        TotalIncTax = c.Double(nullable: false),
                        TotalExcTax = c.Double(),
                        CreditCardNo = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
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
            
            CreateTable(
                "dbo.ShoppingCartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        PriceIncTax = c.Double(nullable: false),
                        PriceExcTax = c.Double(),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(nullable: false, maxLength: 255),
                        ExternalAccount = c.String(maxLength: 255),
                        Password = c.String(),
                        UserName = c.String(nullable: false, maxLength: 150),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Key = c.String(nullable: false, maxLength: 50),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItem", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderItem", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.ShoppingCartItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ShoppingCartItems", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Catogories");
            DropForeignKey("dbo.Addresses", "StateProvinceId", "dbo.States");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.ShoppingCartItems", new[] { "CustomerId" });
            DropIndex("dbo.ShoppingCartItems", new[] { "ProductId" });
            DropIndex("dbo.Customers", new[] { "UserId" });
            DropIndex("dbo.Customers", new[] { "BillingAddressId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.OrderItem", new[] { "OrderId" });
            DropIndex("dbo.OrderItem", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "StateProvinceId" });
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.ShoppingCartItems");
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItem");
            DropTable("dbo.Products");
            DropTable("dbo.Catogories");
            DropTable("dbo.States");
            DropTable("dbo.Countries");
            DropTable("dbo.Addresses");
        }
    }
}
