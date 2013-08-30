namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_task_downloadstate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wcs_task", "download_state", c => c.String(nullable: false, maxLength: 1, fixedLength: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wcs_task", "download_state", c => c.String(nullable: false, maxLength: 2, fixedLength: true));
        }
    }
}
