namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product_pointareacodes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_product", "point_area_codes", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_product", "point_area_codes");
        }
    }
}
