namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_in_bill_detail_history : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_in_bill_detail_history", "id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_in_bill_detail_history", "id", c => c.Int(nullable: false, identity: true));
        }
    }
}
