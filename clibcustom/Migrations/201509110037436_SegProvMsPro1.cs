namespace clibCustom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SegProvMsPro1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Orden", c => c.Int(nullable: false));
            AddColumn("dbo.Submenus", "Orden", c => c.Int(nullable: false));
            AddColumn("dbo.Menus", "Orden", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Orden");
            DropColumn("dbo.Submenus", "Orden");
            DropColumn("dbo.Items", "Orden");
        }
    }
}
