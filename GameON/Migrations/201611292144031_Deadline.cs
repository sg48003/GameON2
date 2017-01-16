namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deadline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Terms", "Deadline", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Terms", "Taken", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Terms", "Taken", c => c.Boolean());
            DropColumn("dbo.Terms", "Deadline");
        }
    }
}
