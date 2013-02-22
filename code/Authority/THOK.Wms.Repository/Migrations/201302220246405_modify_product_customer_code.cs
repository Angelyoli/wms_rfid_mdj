namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product_customer_code : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_product", "custom_code", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_product", "custom_code", c => c.String(maxLength: 20));
        }
    }
}
