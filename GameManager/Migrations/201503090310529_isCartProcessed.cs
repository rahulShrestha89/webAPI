namespace GameManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isCartProcessed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cart", "isProcessed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cart", "isProcessed");
        }
    }
}
