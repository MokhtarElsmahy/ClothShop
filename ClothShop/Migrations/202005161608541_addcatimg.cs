namespace ClothShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcatimg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CategoryImg", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "CategoryImg");
        }
    }
}
