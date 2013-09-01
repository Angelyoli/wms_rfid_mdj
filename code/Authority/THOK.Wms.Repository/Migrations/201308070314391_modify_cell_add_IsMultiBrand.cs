namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_cell_add_IsMultiBrand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_cell", "IsMultiBrand", c => c.String(nullable: false, maxLength: 1, fixedLength: true, defaultValue: "1"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_cell", "IsMultiBrand");
        }
    }
}
