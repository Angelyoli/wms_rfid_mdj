namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_position : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_position", "has_get_request", c => c.Boolean(nullable: false));
            AddColumn("dbo.wcs_position", "has_put_request", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_position", "has_put_request");
            DropColumn("dbo.wcs_position", "has_get_request");
        }
    }
}
