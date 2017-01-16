namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Flags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameInTerms", "Confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameInTerms", "Canceled", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameInTerms", "Pending", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameInTerms", "Pending");
            DropColumn("dbo.GameInTerms", "Canceled");
            DropColumn("dbo.GameInTerms", "Confirmed");
        }
    }
}
