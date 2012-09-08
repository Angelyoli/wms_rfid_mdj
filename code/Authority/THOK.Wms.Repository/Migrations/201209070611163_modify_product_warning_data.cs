namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product_warning_data : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_product_warning", "min_limited", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.wms_product_warning", "max_limited", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.wms_product_warning", "assembly_time", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_product_warning", "assembly_time", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_product_warning", "max_limited", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_product_warning", "min_limited", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
