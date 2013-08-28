namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_srm_canceltask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_srm", "cancel_task", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_srm", "cancel_task");
        }
    }
}
