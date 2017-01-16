namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Terms", "MaxPeople", c => c.Int(nullable: true));
            AlterColumn("dbo.Terms", "Price", c => c.Int(nullable: true));
            AlterColumn("dbo.Terms", "ZIPCode", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
