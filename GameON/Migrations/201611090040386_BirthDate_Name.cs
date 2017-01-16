namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BirthDate_Name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));

        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BirthDate");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
