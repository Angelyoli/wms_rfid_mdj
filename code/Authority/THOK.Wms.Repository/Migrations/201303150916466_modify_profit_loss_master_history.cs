namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_profit_loss_master_history : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_profit_loss_bill_master_history", "bill_type_code", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_profit_loss_bill_master_history", "bill_type_code", c => c.String(nullable: false, maxLength: 1));
        }
    }
}
