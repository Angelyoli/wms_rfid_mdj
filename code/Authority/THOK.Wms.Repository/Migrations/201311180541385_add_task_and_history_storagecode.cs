namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_task_and_history_storagecode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_task", "origin_storage_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task", "target_storage_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task_history", "origin_storage_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task_history", "target_storage_code", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_task_history", "target_storage_code");
            DropColumn("dbo.wcs_task_history", "origin_storage_code");
            DropColumn("dbo.wcs_task", "target_storage_code");
            DropColumn("dbo.wcs_task", "origin_storage_code");
        }
    }
}
