namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class move_bill_pallettag2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.wms_move_bill_detail", name: "pallet_tag)", newName: "pallet_tag");
            RenameColumn(table: "dbo.wms_out_bill_allot", name: "pallet_tag)", newName: "pallet_tag");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.wms_out_bill_allot", name: "pallet_tag", newName: "pallet_tag)");
            RenameColumn(table: "dbo.wms_move_bill_detail", name: "pallet_tag", newName: "pallet_tag)");
        }
    }
}
