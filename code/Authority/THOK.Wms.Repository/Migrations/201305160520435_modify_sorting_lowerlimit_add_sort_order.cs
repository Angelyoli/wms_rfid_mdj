namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_sorting_lowerlimit_add_sort_order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_sorting_lowerlimit", "sort_order", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_sorting_lowerlimit", "sort_order");
        }
    }
}
