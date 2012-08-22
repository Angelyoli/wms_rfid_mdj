namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_unit_list : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.wms_unit_list", "unit_name01");
            DropColumn("dbo.wms_unit_list", "unit_name02");
            DropColumn("dbo.wms_unit_list", "unit_name03");
            DropColumn("dbo.wms_unit_list", "unit_name04");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wms_unit_list", "unit_name04", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wms_unit_list", "unit_name03", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wms_unit_list", "unit_name02", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wms_unit_list", "unit_name01", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
