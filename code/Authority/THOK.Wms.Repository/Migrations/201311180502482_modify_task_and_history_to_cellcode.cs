namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_task_and_history_to_cellcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_task", "origin_cell_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task", "target_cell_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task_history", "origin_cell_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task_history", "target_cell_code", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.wcs_task", "origin_storage_code");
            DropColumn("dbo.wcs_task", "target_storage_code");
            DropColumn("dbo.wcs_task_history", "origin_storage_code");
            DropColumn("dbo.wcs_task_history", "target_storage_code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wcs_task_history", "target_storage_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task_history", "origin_storage_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task", "target_storage_code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_task", "origin_storage_code", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.wcs_task_history", "target_cell_code");
            DropColumn("dbo.wcs_task_history", "origin_cell_code");
            DropColumn("dbo.wcs_task", "target_cell_code");
            DropColumn("dbo.wcs_task", "origin_cell_code");
        }
    }
}
