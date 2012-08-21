namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_in_out_bill_target_cell : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_in_bill_master", "target_cell_code", c => c.String());
            AddColumn("dbo.wms_out_bill_master", "target_cell_code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_out_bill_master", "target_cell_code");
            DropColumn("dbo.wms_in_bill_master", "target_cell_code");
        }
    }
}
