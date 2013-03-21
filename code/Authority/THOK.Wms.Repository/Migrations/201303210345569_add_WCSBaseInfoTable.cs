namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WCSBaseInfoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_position",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        position_name = c.String(nullable: false, maxLength: 50),
                        position_type = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        region_id = c.Int(nullable: false),
                        srmname = c.String(nullable: false, maxLength: 50),
                        travel_pos = c.Int(nullable: false),
                        lift_pos = c.Int(nullable: false),
                        extension = c.Int(nullable: false),
                        description = c.String(),
                        has_goods = c.Boolean(nullable: false),
                        able_stock_out = c.Boolean(nullable: false),
                        able_stock_in_pallet = c.Boolean(nullable: false),
                        tag_address = c.String(nullable: false),
                        current_task_id = c.Int(nullable: false),
                        current_operate_quantity = c.Int(nullable: false),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_region", t => t.region_id)
                .Index(t => t.region_id);
            
            CreateTable(
                "dbo.wms_path_node",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        path_id = c.Int(nullable: false),
                        position_id = c.Int(nullable: false),
                        path_node_order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_path", t => t.path_id)
                .ForeignKey("dbo.wms_position", t => t.position_id)
                .Index(t => t.path_id)
                .Index(t => t.position_id);
            
            CreateTable(
                "dbo.wms_path",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        path_name = c.String(nullable: false, maxLength: 50),
                        origin_region_id = c.Int(nullable: false),
                        target_region_id = c.Int(nullable: false),
                        description = c.String(),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_region", t => t.origin_region_id)
                .ForeignKey("dbo.wms_region", t => t.target_region_id)
                .Index(t => t.origin_region_id)
                .Index(t => t.target_region_id);
            
            CreateTable(
                "dbo.wms_cell_position",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        stock_in_position_id = c.Int(nullable: false),
                        stock_out_position_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_position", t => t.stock_in_position_id)
                .ForeignKey("dbo.wms_position", t => t.stock_out_position_id)
                .Index(t => t.stock_in_position_id)
                .Index(t => t.stock_out_position_id);
            
            CreateTable(
                "dbo.wms_srm",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        srmname = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.wms_size",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        size_name = c.String(nullable: false, maxLength: 50),
                        size_no = c.Int(nullable: false),
                        length = c.Int(nullable: false),
                        width = c.Int(nullable: false),
                        height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.wms_product_size",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_code = c.String(nullable: false, maxLength: 20),
                        size_no = c.Int(nullable: false),
                        area_no = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_cell_position", new[] { "stock_out_position_id" });
            DropIndex("dbo.wms_cell_position", new[] { "stock_in_position_id" });
            DropIndex("dbo.wms_path", new[] { "target_region_id" });
            DropIndex("dbo.wms_path", new[] { "origin_region_id" });
            DropIndex("dbo.wms_path_node", new[] { "position_id" });
            DropIndex("dbo.wms_path_node", new[] { "path_id" });
            DropIndex("dbo.wms_position", new[] { "region_id" });
            DropForeignKey("dbo.wms_cell_position", "stock_out_position_id", "dbo.wms_position");
            DropForeignKey("dbo.wms_cell_position", "stock_in_position_id", "dbo.wms_position");
            DropForeignKey("dbo.wms_path", "target_region_id", "dbo.wms_region");
            DropForeignKey("dbo.wms_path", "origin_region_id", "dbo.wms_region");
            DropForeignKey("dbo.wms_path_node", "position_id", "dbo.wms_position");
            DropForeignKey("dbo.wms_path_node", "path_id", "dbo.wms_path");
            DropForeignKey("dbo.wms_position", "region_id", "dbo.wms_region");
            DropTable("dbo.wms_product_size");
            DropTable("dbo.wms_size");
            DropTable("dbo.wms_srm");
            DropTable("dbo.wms_cell_position");
            DropTable("dbo.wms_path");
            DropTable("dbo.wms_path_node");
            DropTable("dbo.wms_position");
        }
    }
}
