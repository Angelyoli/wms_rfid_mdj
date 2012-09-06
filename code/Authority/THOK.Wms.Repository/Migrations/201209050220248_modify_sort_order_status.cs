namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_sort_order_status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_sort_order", "status", c => c.String(nullable: false, maxLength: 1, fixedLength: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_sort_order", "status");
        }
    }
}
