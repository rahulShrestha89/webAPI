namespace GameManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datainit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Game", "Cart_Id", "dbo.Cart");
            DropIndex("dbo.Game", new[] { "Cart_Id" });
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
            
            DropColumn("dbo.Game", "Cart_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "Cart_Id", c => c.Int());
            DropForeignKey("dbo.GameCart", "Cart_Id", "dbo.Cart");
            DropForeignKey("dbo.GameCart", "Game_Id", "dbo.Game");
            DropIndex("dbo.GameCart", new[] { "Cart_Id" });
            DropIndex("dbo.GameCart", new[] { "Game_Id" });
            DropTable("dbo.GameCart");
            CreateIndex("dbo.Game", "Cart_Id");
            AddForeignKey("dbo.Game", "Cart_Id", "dbo.Cart", "Id");
        }
    }
}
