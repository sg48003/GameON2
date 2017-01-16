namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Term : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Taken = c.Boolean(nullable: false),
                        Price = c.Int(nullable: false),
                        City = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Place = c.String(),
                        ZIPCode = c.Int(nullable: false),
                        MinPeople = c.Int(nullable: false),
                        MaxPeople = c.Int(nullable: false),
                        OwnerId = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Terms", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Terms", new[] { "Owner_Id" });
            DropTable("dbo.Terms");
        }
    }
}
