namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_sortorder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_sort_order", "customer_code", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_sort_order", "customer_code", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
