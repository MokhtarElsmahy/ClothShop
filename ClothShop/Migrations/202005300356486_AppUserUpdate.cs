namespace ClothShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUserUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Adsress", c => c.String());
            AddColumn("dbo.AspNetUsers", "PasswordTxt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PasswordTxt");
            DropColumn("dbo.AspNetUsers", "Adsress");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
