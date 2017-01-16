namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoubleForeignKeysSolved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Games", "GameType_Id", "dbo.GameTypes");
            DropIndex("dbo.Games", new[] { "GameType_Id" });
            DropColumn("dbo.Games", "GameTypeId");
            RenameColumn(table: "dbo.Games", name: "GameType_Id", newName: "GameTypeId");
            AlterColumn("dbo.Games", "GameTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Games", "GameTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Games", "GameTypeId");
            AddForeignKey("dbo.Games", "GameTypeId", "dbo.GameTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "GameTypeId", "dbo.GameTypes");
            DropIndex("dbo.Games", new[] { "GameTypeId" });
            AlterColumn("dbo.Games", "GameTypeId", c => c.Int());
            AlterColumn("dbo.Games", "GameTypeId", c => c.Byte(nullable: false));
            RenameColumn(table: "dbo.Games", name: "GameTypeId", newName: "GameType_Id");
            AddColumn("dbo.Games", "GameTypeId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Games", "GameType_Id");
            AddForeignKey("dbo.Games", "GameType_Id", "dbo.GameTypes", "Id");
        }
    }
}
