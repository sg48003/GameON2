namespace GameON.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [BirthDate]) VALUES (N'64c9738a-0a8b-4853-b4bb-caf5d2c4ae18', N'sebastian.glad@fer.hr', 0, N'ALdA6rSEzLqoA+LhRqF3g/PFzT4hLuUyK8UkRO+JHTDlQO8efB3QTZ3mKcnkJagSPA==', N'57d104ad-6df8-488a-aa8d-d458d440abd2', NULL, 0, 0, NULL, 1, 0, N'admin', N'Sebastian Glad', N'1994-09-30 00:00:00')

            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1646895b-5b3f-47ff-a3c6-c3b5645d69f3', N'Admin')
            
            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'64c9738a-0a8b-4853-b4bb-caf5d2c4ae18', N'1646895b-5b3f-47ff-a3c6-c3b5645d69f3')

            ");
        }
        
        public override void Down()
        {
        }
    }
}
