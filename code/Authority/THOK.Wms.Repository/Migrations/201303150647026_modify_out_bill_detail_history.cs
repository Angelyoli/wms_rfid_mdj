namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_out_bill_detail_history : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_out_bill_detail_history", "id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_out_bill_detail_history", "id", c => c.Int(nullable: false, identity: true));
        }
    }
}
