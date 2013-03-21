namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WCS_Task : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.wms_region", newName: "wcs_region");
            RenameTable(name: "dbo.wms_position", newName: "wcs_position");
            RenameTable(name: "dbo.wms_path_node", newName: "wcs_path_node");
            RenameTable(name: "dbo.wms_path", newName: "wcs_path");
            RenameTable(name: "dbo.wms_cell_position", newName: "wcs_cell_position");
            RenameTable(name: "dbo.wms_srm", newName: "wcs_srm");
            RenameTable(name: "dbo.wms_size", newName: "wcs_size");
            RenameTable(name: "dbo.wms_product_size", newName: "wcs_product_size");
            CreateTable(
                "dbo.wcs_task",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        task_type = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        task_level = c.Int(nullable: false),
                        path_id = c.Int(nullable: false),
                        product_code = c.String(nullable: false, maxLength: 20),
                        product_name = c.String(nullable: false, maxLength: 20),
                        origin_storage_code = c.String(nullable: false, maxLength: 50),
                        target_storage_code = c.String(nullable: false, maxLength: 50),
                        origin_position_id = c.Int(nullable: false),
                        target_position_id = c.Int(nullable: false),
                        current_position_id = c.Int(nullable: false),
                        current_position_state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        tag_state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        quantity = c.Int(nullable: false),
                        task_quantity = c.Int(nullable: false),
                        operate_quantity = c.Int(nullable: false),
                        order_id = c.String(maxLength: 20),
                        order_type = c.String(maxLength: 2, fixedLength: true),
                        allot_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wcs_task");
            RenameTable(name: "dbo.wcs_product_size", newName: "wms_product_size");
            RenameTable(name: "dbo.wcs_size", newName: "wms_size");
            RenameTable(name: "dbo.wcs_srm", newName: "wms_srm");
            RenameTable(name: "dbo.wcs_cell_position", newName: "wms_cell_position");
            RenameTable(name: "dbo.wcs_path", newName: "wms_path");
            RenameTable(name: "dbo.wcs_path_node", newName: "wms_path_node");
            RenameTable(name: "dbo.wcs_position", newName: "wms_position");
            RenameTable(name: "dbo.wcs_region", newName: "wms_region");
        }
    }
}
