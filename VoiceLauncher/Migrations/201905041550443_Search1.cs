namespace VoiceLauncher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Search1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomIntelliSense", "Search", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomIntelliSense", "Search", c => c.String(nullable: false, maxLength: 4000));
        }
    }
}
