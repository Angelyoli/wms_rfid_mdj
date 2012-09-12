namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_in_out_detail_unit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.wms_in_bill_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_out_bill_detail", "unit_code", "dbo.wms_unit");
            DropIndex("dbo.wms_in_bill_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_out_bill_detail", new[] { "unit_code" });
            AddForeignKey("dbo.wms_in_bill_detail", "unit_code", "dbo.wms_unit", "unit_code");
            AddForeignKey("dbo.wms_out_bill_detail", "unit_code", "dbo.wms_unit", "unit_code");
            CreateIndex("dbo.wms_in_bill_detail", "unit_code");
            CreateIndex("dbo.wms_out_bill_detail", "unit_code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_in_bill_detail", new[] { "unit_code" });
            DropForeignKey("dbo.wms_out_bill_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_in_bill_detail", "unit_code", "dbo.wms_unit");
            CreateIndex("dbo.wms_out_bill_detail", "unit_code");
            CreateIndex("dbo.wms_in_bill_detail", "unit_code");
            AddForeignKey("dbo.wms_out_bill_detail", "unit_code", "dbo.wms_unit", "unit_code", cascadeDelete: true);
            AddForeignKey("dbo.wms_in_bill_detail", "unit_code", "dbo.wms_unit", "unit_code", cascadeDelete: true);
        }
    }
}
