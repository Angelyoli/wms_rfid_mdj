namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_cell_and_modify_storage_modify_task : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_cell", "first_in_first_out", c => c.Boolean(nullable: false));
            AddColumn("dbo.wms_cell", "storage_time", c => c.DateTime(nullable: false));
            AddColumn("dbo.wms_storage", "storage_sequence", c => c.Int(nullable: false));
            AddColumn("dbo.wcs_task", "storage_sequence", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_task", "storage_sequence");
            DropColumn("dbo.wms_storage", "storage_sequence");
            DropColumn("dbo.wms_cell", "storage_time");
            DropColumn("dbo.wms_cell", "first_in_first_out");
        }
    }
}
