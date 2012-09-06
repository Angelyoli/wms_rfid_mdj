namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductWarning : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_product_warning",
                c => new
                    {
                        product_code = c.String(nullable: false, maxLength: 20),
                        min_limited = c.Decimal(nullable: false, precision: 18, scale: 2),
                        max_limited = c.Decimal(nullable: false, precision: 18, scale: 2),
                        assembly_time = c.Decimal(nullable: false, precision: 18, scale: 2),
                        memo = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.product_code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wms_product_warning");
        }
    }
}
