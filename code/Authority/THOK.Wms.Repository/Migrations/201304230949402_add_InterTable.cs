namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_InterTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.inter_bill_master",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        uuid = c.String(maxLength: 64),
                        bill_type = c.String(nullable: false, maxLength: 50),
                        bill_date = c.DateTime(nullable: false),
                        maker_name = c.String(maxLength: 50),
                        operate_date = c.DateTime(),
                        cigarette_type = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        bill_company_code = c.String(nullable: false, maxLength: 8),
                        supplier_code = c.String(nullable: false, maxLength: 20),
                        supplier_type = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        state = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.inter_bill_detail",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        master_id = c.Guid(nullable: false),
                        piece_cigar_code = c.String(nullable: false, maxLength: 13),
                        box_cigar_code = c.String(nullable: false, maxLength: 13),
                        bill_quantity = c.Int(nullable: false),
                        fixed_quantity = c.Int(nullable: false),
                        real_quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.inter_bill_master", t => t.master_id)
                .Index(t => t.master_id);
            
            CreateTable(
                "dbo.inter_navicert",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        master_id = c.Guid(nullable: false),
                        navicert_code = c.String(nullable: false, maxLength: 8),
                        navicert_date = c.DateTime(nullable: false),
                        truck_plate_no = c.String(maxLength: 50),
                        contract_code = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.inter_bill_master", t => t.master_id)
                .ForeignKey("dbo.inter_contract", t => t.contract_code)
                .Index(t => t.master_id)
                .Index(t => t.contract_code);
            
            CreateTable(
                "dbo.inter_contract",
                c => new
                    {
                        contract_code = c.String(nullable: false, maxLength: 50),
                        master_id = c.Guid(nullable: false),
                        supply_side_code = c.String(maxLength: 100),
                        demand_side_code = c.String(maxLength: 50),
                        contract_date = c.DateTime(nullable: false),
                        start_dade = c.DateTime(nullable: false),
                        end_date = c.DateTime(nullable: false),
                        send_place_code = c.String(),
                        send_address = c.String(maxLength: 100),
                        receive_place_code = c.String(maxLength: 100),
                        receive_address = c.String(maxLength: 100),
                        sale_date = c.String(maxLength: 100),
                        state = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.contract_code)
                .ForeignKey("dbo.inter_bill_master", t => t.master_id)
                .Index(t => t.master_id);
            
            CreateTable(
                "dbo.inter_contract_detail",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        contract_code = c.String(nullable: false, maxLength: 50),
                        brand_code = c.String(nullable: false, maxLength: 13),
                        quantity = c.String(nullable: false),
                        price = c.String(),
                        amount = c.Int(nullable: false),
                        tax_amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.inter_contract", t => t.contract_code)
                .Index(t => t.contract_code);
            
            CreateTable(
                "dbo.inter_pallet",
                c => new
                    {
                        pallet_id = c.String(nullable: false, maxLength: 128),
                        wms_uuid = c.String(maxLength: 64),
                        uuid = c.String(maxLength: 64),
                        ticket_no = c.String(maxLength: 100),
                        operate_date = c.DateTime(nullable: false),
                        operate_type = c.String(nullable: false, maxLength: 50),
                        bar_code_type = c.String(maxLength: 2),
                        rfid_ant_code = c.String(maxLength: 20),
                        piece_cigar_code = c.String(maxLength: 13),
                        box_cigar_code = c.String(maxLength: 13),
                        cigarette_name = c.String(maxLength: 50),
                        quantity = c.Decimal(nullable: false, precision: 16, scale: 4),
                        scan_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.pallet_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.inter_contract_detail", new[] { "contract_code" });
            DropIndex("dbo.inter_contract", new[] { "master_id" });
            DropIndex("dbo.inter_navicert", new[] { "contract_code" });
            DropIndex("dbo.inter_navicert", new[] { "master_id" });
            DropIndex("dbo.inter_bill_detail", new[] { "master_id" });
            DropForeignKey("dbo.inter_contract_detail", "contract_code", "dbo.inter_contract");
            DropForeignKey("dbo.inter_contract", "master_id", "dbo.inter_bill_master");
            DropForeignKey("dbo.inter_navicert", "contract_code", "dbo.inter_contract");
            DropForeignKey("dbo.inter_navicert", "master_id", "dbo.inter_bill_master");
            DropForeignKey("dbo.inter_bill_detail", "master_id", "dbo.inter_bill_master");
            DropTable("dbo.inter_pallet");
            DropTable("dbo.inter_contract_detail");
            DropTable("dbo.inter_contract");
            DropTable("dbo.inter_navicert");
            DropTable("dbo.inter_bill_detail");
            DropTable("dbo.inter_bill_master");
        }
    }
}
