namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Terms", "Taken", c => c.Boolean());
            AlterColumn("dbo.Terms", "Price", c => c.Int());
            AlterColumn("dbo.Terms", "ZIPCode", c => c.Int());
            AlterColumn("dbo.Terms", "MaxPeople", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Terms", "MaxPeople", c => c.Int(nullable: false));
            AlterColumn("dbo.Terms", "ZIPCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Terms", "Price", c => c.Int(nullable: false));
            AlterColumn("dbo.Terms", "Taken", c => c.Boolean(nullable: false));
        }
    }
}
