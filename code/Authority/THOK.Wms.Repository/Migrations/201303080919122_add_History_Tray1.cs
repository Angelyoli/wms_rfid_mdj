namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_History_Tray1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_tray_info",
                c => new
                    {
                        TaryID = c.Guid(nullable: false),
                        TaryRfid = c.String(nullable: false, maxLength: 100),
                        ProductCode = c.String(nullable: false, maxLength: 20),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.TaryID);
            
            CreateTable(
                "dbo.wms_in_bill_master_history",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(),
                        description = c.String(maxLength: 100),
                        lock_tag = c.String(maxLength: 50),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                        row_version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        target_cell_code = c.String(),
                    })
                .PrimaryKey(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_in_bill_detail_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bill_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        allot_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_in_bill_master_history", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_in_bill_allot_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        in_bill_detail_id = c.Int(nullable: false),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        allot_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        operate_person_id = c.Guid(),
                        _operator = c.String(name: "operator", maxLength: 20),
                        start_time = c.DateTime(),
                        finish_time = c.DateTime(),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_in_bill_master_history", t => t.bill_no)
                .ForeignKey("dbo.wms_in_bill_detail_history", t => t.in_bill_detail_id)
                .Index(t => t.bill_no)
                .Index(t => t.in_bill_detail_id);
            
            CreateTable(
                "dbo.wms_out_bill_master_history",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        origin = c.String(nullable: false, maxLength: 1),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(),
                        description = c.String(maxLength: 100),
                        move_bill_master_bill_no = c.String(),
                        lock_tag = c.String(maxLength: 50),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                        row_version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        target_cell_code = c.String(),
                    })
                .PrimaryKey(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_out_bill_detail_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bill_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        allot_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_out_bill_master_history", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_out_bill_allot_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        pallet_tag = c.Int(),
                        product_code = c.String(nullable: false, maxLength: 20),
                        out_bill_detail_id = c.Int(nullable: false),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        allot_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        operate_person_id = c.Guid(),
                        _operator = c.String(name: "operator", maxLength: 20),
                        can_real_operate = c.String(maxLength: 1),
                        start_time = c.DateTime(),
                        finish_time = c.DateTime(),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_out_bill_master_history", t => t.bill_no)
                .ForeignKey("dbo.wms_out_bill_detail_history", t => t.out_bill_detail_id)
                .Index(t => t.bill_no)
                .Index(t => t.out_bill_detail_id);
            
            CreateTable(
                "dbo.wms_move_bill_master_history",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        origin = c.String(nullable: false, maxLength: 1),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                        lock_tag = c.String(maxLength: 50),
                        row_version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_move_bill_detail_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        pallet_tag = c.Int(),
                        product_code = c.String(nullable: false, maxLength: 20),
                        out_cell_code = c.String(nullable: false, maxLength: 20),
                        out_storage_code = c.String(nullable: false, maxLength: 50),
                        in_cell_code = c.String(nullable: false, maxLength: 20),
                        in_storage_code = c.String(nullable: false, maxLength: 50),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        real_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        operate_person_id = c.Guid(),
                        _operator = c.String(name: "operator", maxLength: 20),
                        can_real_operate = c.String(maxLength: 1),
                        start_time = c.DateTime(),
                        finish_time = c.DateTime(),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_move_bill_master_history", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_check_bill_master_history",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_check_bill_detail_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        real_product_code = c.String(nullable: false, maxLength: 20),
                        real_unit_code = c.String(nullable: false, maxLength: 20),
                        real_quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        operate_person_id = c.Guid(),
                        _operator = c.String(name: "operator", maxLength: 20),
                        start_time = c.DateTime(),
                        finish_time = c.DateTime(),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_check_bill_master_history", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_profit_loss_bill_master_history",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 1),
                        check_bill_no = c.String(maxLength: 20),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                        lock_tag = c.String(maxLength: 50),
                        row_version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_profit_loss_bill_detail_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_profit_loss_bill_master_history", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_daily_balance_history",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        settle_date = c.DateTime(nullable: false),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        beginning = c.Decimal(nullable: false, precision: 18, scale: 2),
                        entry_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        delivery_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        profit_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        loss_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ending = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_profit_loss_bill_detail_history", new[] { "bill_no" });
            DropIndex("dbo.wms_check_bill_detail_history", new[] { "bill_no" });
            DropIndex("dbo.wms_move_bill_detail_history", new[] { "bill_no" });
            DropIndex("dbo.wms_out_bill_allot_history", new[] { "out_bill_detail_id" });
            DropIndex("dbo.wms_out_bill_allot_history", new[] { "bill_no" });
            DropIndex("dbo.wms_out_bill_detail_history", new[] { "bill_no" });
            DropIndex("dbo.wms_in_bill_allot_history", new[] { "in_bill_detail_id" });
            DropIndex("dbo.wms_in_bill_allot_history", new[] { "bill_no" });
            DropIndex("dbo.wms_in_bill_detail_history", new[] { "bill_no" });
            DropForeignKey("dbo.wms_profit_loss_bill_detail_history", "bill_no", "dbo.wms_profit_loss_bill_master_history");
            DropForeignKey("dbo.wms_check_bill_detail_history", "bill_no", "dbo.wms_check_bill_master_history");
            DropForeignKey("dbo.wms_move_bill_detail_history", "bill_no", "dbo.wms_move_bill_master_history");
            DropForeignKey("dbo.wms_out_bill_allot_history", "out_bill_detail_id", "dbo.wms_out_bill_detail_history");
            DropForeignKey("dbo.wms_out_bill_allot_history", "bill_no", "dbo.wms_out_bill_master_history");
            DropForeignKey("dbo.wms_out_bill_detail_history", "bill_no", "dbo.wms_out_bill_master_history");
            DropForeignKey("dbo.wms_in_bill_allot_history", "in_bill_detail_id", "dbo.wms_in_bill_detail_history");
            DropForeignKey("dbo.wms_in_bill_allot_history", "bill_no", "dbo.wms_in_bill_master_history");
            DropForeignKey("dbo.wms_in_bill_detail_history", "bill_no", "dbo.wms_in_bill_master_history");
            DropTable("dbo.wms_daily_balance_history");
            DropTable("dbo.wms_profit_loss_bill_detail_history");
            DropTable("dbo.wms_profit_loss_bill_master_history");
            DropTable("dbo.wms_check_bill_detail_history");
            DropTable("dbo.wms_check_bill_master_history");
            DropTable("dbo.wms_move_bill_detail_history");
            DropTable("dbo.wms_move_bill_master_history");
            DropTable("dbo.wms_out_bill_allot_history");
            DropTable("dbo.wms_out_bill_detail_history");
            DropTable("dbo.wms_out_bill_master_history");
            DropTable("dbo.wms_in_bill_allot_history");
            DropTable("dbo.wms_in_bill_detail_history");
            DropTable("dbo.wms_in_bill_master_history");
            DropTable("dbo.wms_tray_info");
        }
    }
}
