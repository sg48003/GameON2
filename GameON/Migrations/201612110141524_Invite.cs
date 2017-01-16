namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invites",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserSendId = c.String(nullable: false, maxLength: 128),
                    UserReceiveId = c.String(nullable: false, maxLength: 128),
                    Accepted = c.Boolean(nullable: false),
                    Rejected = c.Boolean(nullable: false),
                    Pending = c.Boolean(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserSendId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserReceiveId);
               
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", "UserSendId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Invites", "UserReceiveId", "dbo.AspNetUsers");
            DropIndex("dbo.Invites", new[] { "UserSendId" });
            DropIndex("dbo.Invites", new[] { "UserReceiveId" });
            DropTable("dbo.Invites");
        }
    }
}
