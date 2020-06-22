namespace ClothShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiximg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ImageURL", c => c.String());
            DropColumn("dbo.Categories", "CategoryImg");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "CategoryImg", c => c.String());
            DropColumn("dbo.Categories", "ImageURL");
        }
    }
}
