namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_system_pranmenter : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.wms_system_parameter", newName: "auth_system_parameter");
            AlterColumn("dbo.auth_system_parameter", "system_id", c => c.Guid(nullable: false));
            AddForeignKey("dbo.auth_system_parameter", "system_id", "dbo.auth_system", "system_id");
            CreateIndex("dbo.auth_system_parameter", "system_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.auth_system_parameter", new[] { "system_id" });
            DropForeignKey("dbo.auth_system_parameter", "system_id", "dbo.auth_system");
            AlterColumn("dbo.auth_system_parameter", "system_id", c => c.Guid());
            RenameTable(name: "dbo.auth_system_parameter", newName: "wms_system_parameter");
        }
    }
}
