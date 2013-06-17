namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product_pointareacodes1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_product", "point_area_codes", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_product", "point_area_codes", c => c.String(maxLength: 50));
        }
    }
}
