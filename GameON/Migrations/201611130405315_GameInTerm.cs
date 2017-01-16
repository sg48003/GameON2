namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameInTerm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameInTerms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.TermId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameInTerms", "TermId", "dbo.Terms");
            DropForeignKey("dbo.GameInTerms", "GameId", "dbo.Games");
            DropIndex("dbo.GameInTerms", new[] { "TermId" });
            DropIndex("dbo.GameInTerms", new[] { "GameId" });
            DropTable("dbo.GameInTerms");
        }
    }
}
