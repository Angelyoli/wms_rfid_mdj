namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BusinessSystemsDailyBalance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_business_systems_daily_balance",
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
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .Index(t => t.warehouse_code)
                .Index(t => t.product_code)
                .Index(t => t.unit_code);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_business_systems_daily_balance", new[] { "unit_code" });
            DropIndex("dbo.wms_business_systems_daily_balance", new[] { "product_code" });
            DropIndex("dbo.wms_business_systems_daily_balance", new[] { "warehouse_code" });
            DropForeignKey("dbo.wms_business_systems_daily_balance", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_business_systems_daily_balance", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_business_systems_daily_balance", "warehouse_code", "dbo.wms_warehouse");
            DropTable("dbo.wms_business_systems_daily_balance");
        }
    }
}
