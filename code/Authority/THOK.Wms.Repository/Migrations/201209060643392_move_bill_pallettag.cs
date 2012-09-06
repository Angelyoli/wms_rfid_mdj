namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class move_bill_pallettag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_move_bill_detail", "pallet_tag)", c => c.Int());
            AddColumn("dbo.wms_out_bill_allot", "pallet_tag)", c => c.Int());
            DropColumn("dbo.wms_out_bill_allot", "out_pallet_tag)");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wms_out_bill_allot", "out_pallet_tag)", c => c.Int(nullable: false));
            DropColumn("dbo.wms_out_bill_allot", "pallet_tag)");
            DropColumn("dbo.wms_move_bill_detail", "pallet_tag)");
        }
    }
}
