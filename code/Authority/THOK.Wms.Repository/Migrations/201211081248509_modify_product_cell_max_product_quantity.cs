namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product_cell_max_product_quantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_product", "cell_max_product_quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_product", "cell_max_product_quantity");
        }
    }
}
