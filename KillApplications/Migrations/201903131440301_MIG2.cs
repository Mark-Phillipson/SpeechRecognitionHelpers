namespace KillApplications.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MIG2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applications", "ProcessName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applications", "ProcessName", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
