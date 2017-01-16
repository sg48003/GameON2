namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteExtraForeignKey : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Games", new[] { "GameType_Id" });
            //DropColumn("dbo.Games", "GameTypeId");
            RenameColumn(table: "dbo.Games", name: "GamyTypeId", newName: "GameTypeId");
            //DropIndex("dbo.Games", "GameType_Id");
            //DropForeignKey("dbo.Games", "GameType_Id", "dbo.GameTypes");
            //DropColumn("dbo.Games", "GameType_Id");
        }
        
        public override void Down()
        {
        }
    }
}
