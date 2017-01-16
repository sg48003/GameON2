namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Player : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Terms", new[] { "Owner_Id" });
            DropColumn("dbo.Terms", "OwnerId");
            RenameColumn(table: "dbo.Terms", name: "Owner_Id", newName: "OwnerId");
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameInTermId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameInTerms", t => t.GameInTermId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.GameInTermId)
                .Index(t => t.UserId);
            
            AlterColumn("dbo.Terms", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Terms", "OwnerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Players", "GameInTermId", "dbo.GameInTerms");
            DropIndex("dbo.Players", new[] { "UserId" });
            DropIndex("dbo.Players", new[] { "GameInTermId" });
            DropIndex("dbo.Terms", new[] { "OwnerId" });
            AlterColumn("dbo.Terms", "OwnerId", c => c.Int());
            DropTable("dbo.Players");
            RenameColumn(table: "dbo.Terms", name: "OwnerId", newName: "Owner_Id");
            AddColumn("dbo.Terms", "OwnerId", c => c.Int());
            CreateIndex("dbo.Terms", "Owner_Id");
        }
    }
}
