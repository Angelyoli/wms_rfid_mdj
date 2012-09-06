namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_orderdate_fixed_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_sort_order_dispatch", "order_date", c => c.String(nullable: false, maxLength: 14, fixedLength: true));
            AlterColumn("dbo.wms_sort_work_dispatch", "order_date", c => c.String(nullable: false, maxLength: 14, fixedLength: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_sort_work_dispatch", "order_date", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.wms_sort_order_dispatch", "order_date", c => c.String(nullable: false, maxLength: 14));
        }
    }
}
