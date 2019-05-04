namespace VoiceLauncher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Category = c.String(maxLength: 30),
                        Category_Type = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CustomIntelliSense",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LanguageID = c.Int(nullable: false),
                        Display_Value = c.String(nullable: false, maxLength: 255),
                        SendKeys_Value = c.String(maxLength: 4000),
                        Command_Type = c.String(maxLength: 255),
                        CategoryID = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 255),
                        Search = c.String(nullable: false, maxLength: 4000),
                        ComputerID = c.Int(),
                        DeliveryType = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Computers", t => t.ComputerID)
                .ForeignKey("dbo.Languages", t => t.LanguageID, cascadeDelete: true)
                .Index(t => t.LanguageID)
                .Index(t => t.CategoryID)
                .Index(t => t.ComputerID);
            
            CreateTable(
                "dbo.Computers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ComputerName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Launcher",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CommandLine = c.String(maxLength: 255),
                        CategoryID = c.Int(nullable: false),
                        ComputerID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Computers", t => t.ComputerID)
                .Index(t => t.CategoryID)
                .Index(t => t.ComputerID);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Language = c.String(nullable: false, maxLength: 25),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CurrentWindow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Handle = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GeneralLookups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Item_Value = c.String(nullable: false, maxLength: 255),
                        Category = c.String(nullable: false, maxLength: 255),
                        SortOrder = c.Int(),
                        DisplayValue = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HtmlTags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Tag = c.String(maxLength: 255),
                        Description = c.String(maxLength: 255),
                        ListValue = c.String(maxLength: 255),
                        Include = c.Boolean(nullable: false),
                        SpokenForm = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LauncherMultipleLauncherBridge",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LauncherID = c.Int(nullable: false),
                        MultipleLauncherID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Username = c.String(maxLength: 255),
                        Password = c.String(maxLength: 255),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MousePositions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Command = c.String(nullable: false, maxLength: 255),
                        MouseLeft = c.Int(nullable: false),
                        MouseTop = c.Int(nullable: false),
                        TabPageName = c.String(maxLength: 255),
                        ControlName = c.String(maxLength: 255),
                        Application = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MultipleLauncher",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 70),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PropertyTabPositions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ObjectName = c.String(nullable: false, maxLength: 60),
                        PropertyName = c.String(nullable: false, maxLength: 60),
                        NumberOfTabs = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SavedMousePosition",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NamedLocation = c.String(nullable: false, maxLength: 255),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ValuesToInsert",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ValueToInsert = c.String(nullable: false, maxLength: 255),
                        Lookup = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomIntelliSense", "LanguageID", "dbo.Languages");
            DropForeignKey("dbo.Launcher", "ComputerID", "dbo.Computers");
            DropForeignKey("dbo.Launcher", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.CustomIntelliSense", "ComputerID", "dbo.Computers");
            DropForeignKey("dbo.CustomIntelliSense", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Launcher", new[] { "ComputerID" });
            DropIndex("dbo.Launcher", new[] { "CategoryID" });
            DropIndex("dbo.CustomIntelliSense", new[] { "ComputerID" });
            DropIndex("dbo.CustomIntelliSense", new[] { "CategoryID" });
            DropIndex("dbo.CustomIntelliSense", new[] { "LanguageID" });
            DropTable("dbo.ValuesToInsert");
            DropTable("dbo.SavedMousePosition");
            DropTable("dbo.PropertyTabPositions");
            DropTable("dbo.MultipleLauncher");
            DropTable("dbo.MousePositions");
            DropTable("dbo.Logins");
            DropTable("dbo.LauncherMultipleLauncherBridge");
            DropTable("dbo.HtmlTags");
            DropTable("dbo.GeneralLookups");
            DropTable("dbo.CurrentWindow");
            DropTable("dbo.Languages");
            DropTable("dbo.Launcher");
            DropTable("dbo.Computers");
            DropTable("dbo.CustomIntelliSense");
            DropTable("dbo.Categories");
        }
    }
}
