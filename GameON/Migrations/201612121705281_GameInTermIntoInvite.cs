namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameInTermIntoInvite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invites", "GameInTermId", c => c.Int(nullable: false));
            CreateIndex("dbo.Invites", "GameInTermId");
            AddForeignKey("dbo.Invites", "GameInTermId", "dbo.GameInTerms", "Id", cascadeDelete: true);

        }
        
        public override void Down()
        {
            DropColumn("dbo.Invite", "GameInTermId");
            DropForeignKey("dbo.Invites", "GameInTermId", "dbo.GameInTerms");
            DropIndex("dbo.Invites", new[] { "GameInTermId" });
        }
    }
}
