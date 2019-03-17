namespace KillApplications.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MIG3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "Kill", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "Kill");
        }
    }
}
