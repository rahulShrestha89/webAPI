namespace GameManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _initial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        CartId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 25),
                        Password = c.String(),
                        ApiKey = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameCart",
                c => new
                    {
                        Game_Id = c.Int(nullable: false),
                        Cart_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Game_Id, t.Cart_Id })
                .ForeignKey("dbo.Game", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cart", t => t.Cart_Id, cascadeDelete: true)
                .Index(t => t.Game_Id)
                .Index(t => t.Cart_Id);
            
            CreateTable(
                "dbo.GenreGame",
                c => new
                    {
                        Genre_Id = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_Id, t.Game_Id })
                .ForeignKey("dbo.Genre", t => t.Genre_Id, cascadeDelete: true)
                .ForeignKey("dbo.Game", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Genre_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.TagGame",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Game_Id })
                .ForeignKey("dbo.Tag", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Game", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagGame", "Game_Id", "dbo.Game");
            DropForeignKey("dbo.TagGame", "Tag_Id", "dbo.Tag");
            DropForeignKey("dbo.GenreGame", "Game_Id", "dbo.Game");
            DropForeignKey("dbo.GenreGame", "Genre_Id", "dbo.Genre");
            DropForeignKey("dbo.GameCart", "Cart_Id", "dbo.Cart");
            DropForeignKey("dbo.GameCart", "Game_Id", "dbo.Game");
            DropIndex("dbo.TagGame", new[] { "Game_Id" });
            DropIndex("dbo.TagGame", new[] { "Tag_Id" });
            DropIndex("dbo.GenreGame", new[] { "Game_Id" });
            DropIndex("dbo.GenreGame", new[] { "Genre_Id" });
            DropIndex("dbo.GameCart", new[] { "Cart_Id" });
            DropIndex("dbo.GameCart", new[] { "Game_Id" });
            DropTable("dbo.TagGame");
            DropTable("dbo.GenreGame");
            DropTable("dbo.GameCart");
            DropTable("dbo.User");
            DropTable("dbo.Sale");
            DropTable("dbo.Tag");
            DropTable("dbo.Genre");
            DropTable("dbo.Game");
            DropTable("dbo.Cart");
        }
    }
}
