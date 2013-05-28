namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_SortingLowerLimit_SortType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_sorting_lowerlimit", "sort_type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_sorting_lowerlimit", "sort_type");
        }
    }
}
