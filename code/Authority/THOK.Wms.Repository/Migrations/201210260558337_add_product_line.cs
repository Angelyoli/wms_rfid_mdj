namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_product_line : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_product", "is_rounding", c => c.String(maxLength: 1, fixedLength: true));
            AddColumn("dbo.wms_deliver_line", "new_deliver_line_code", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_deliver_line", "new_deliver_line_code");
            DropColumn("dbo.wms_product", "is_rounding");
        }
    }
}
