namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bill_Operator_canReal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_move_bill_detail", "operator", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_move_bill_detail", "can_real_operate", c => c.String(maxLength: 1));
            AddColumn("dbo.wms_check_bill_detail", "operator", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_in_bill_allot", "operator", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_out_bill_allot", "operator", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_out_bill_allot", "can_real_operate", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_out_bill_allot", "can_real_operate");
            DropColumn("dbo.wms_out_bill_allot", "operator");
            DropColumn("dbo.wms_in_bill_allot", "operator");
            DropColumn("dbo.wms_check_bill_detail", "operator");
            DropColumn("dbo.wms_move_bill_detail", "can_real_operate");
            DropColumn("dbo.wms_move_bill_detail", "operator");
        }
    }
}
