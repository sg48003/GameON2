namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Game : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        GameTypeId = c.Byte(nullable: false),
                        GameType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameTypes", t => t.GameType_Id )
                .Index(t => t.GameType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "GameType_Id", "dbo.GameTypes");
            DropIndex("dbo.Games", new[] { "GameType_Id" });
            DropTable("dbo.Games");
        }
    }
}
