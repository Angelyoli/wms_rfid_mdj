namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_task_and_history_createtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_task", "create_time", c => c.DateTime(nullable: false));
            AddColumn("dbo.wcs_task_history", "create_time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_task_history", "create_time");
            DropColumn("dbo.wcs_task", "create_time");
        }
    }
}
