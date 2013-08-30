namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_product_size : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_product_size", "length", c => c.Int(nullable: false));
            AddColumn("dbo.wcs_product_size", "width", c => c.Int(nullable: false));
            AddColumn("dbo.wcs_product_size", "height", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wcs_product_size", "height");
            DropColumn("dbo.wcs_product_size", "width");
            DropColumn("dbo.wcs_product_size", "length");
        }
    }
}
