namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WCSBaseInfoTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_position", "channel_code", c => c.String(maxLength: 50));
            AddColumn("dbo.wcs_srm", "opcservice_name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_srm", "get_request", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_srm", "get_allow", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_srm", "get_complete", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_srm", "put_request", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_srm", "put_allow", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_srm", "put_complete", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.wcs_product_size", "product_no", c => c.Int(nullable: false));
            AddColumn("dbo.wcs_task", "download_state", c => c.String(nullable: false, maxLength: 2, fixedLength: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_task", "download_state");
            DropColumn("dbo.wcs_product_size", "product_no");
            DropColumn("dbo.wcs_srm", "put_complete");
            DropColumn("dbo.wcs_srm", "put_allow");
            DropColumn("dbo.wcs_srm", "put_request");
            DropColumn("dbo.wcs_srm", "get_complete");
            DropColumn("dbo.wcs_srm", "get_allow");
            DropColumn("dbo.wcs_srm", "get_request");
            DropColumn("dbo.wcs_srm", "opcservice_name");
            DropColumn("dbo.wcs_position", "channel_code");
        }
    }
}
