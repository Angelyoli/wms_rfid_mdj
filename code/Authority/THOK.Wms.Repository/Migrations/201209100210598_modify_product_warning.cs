namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product_warning : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_storage", "ProductWarning_ProductCode", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_storage", "ProductWarning_ProductCode1", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_product_warning", "Storage_StorageCode", c => c.String(maxLength: 50));
            AddColumn("dbo.wms_product_warning", "Unit_UnitCode", c => c.String(maxLength: 20));
            AddForeignKey("dbo.wms_storage", "ProductWarning_ProductCode", "dbo.wms_product_warning", "product_code");
            AddForeignKey("dbo.wms_storage", "ProductWarning_ProductCode1", "dbo.wms_product_warning", "product_code");
            AddForeignKey("dbo.wms_product_warning", "Storage_StorageCode", "dbo.wms_storage", "storage_code");
            AddForeignKey("dbo.wms_product_warning", "Unit_UnitCode", "dbo.wms_unit", "unit_code");
            CreateIndex("dbo.wms_storage", "ProductWarning_ProductCode");
            CreateIndex("dbo.wms_storage", "ProductWarning_ProductCode1");
            CreateIndex("dbo.wms_product_warning", "Storage_StorageCode");
            CreateIndex("dbo.wms_product_warning", "Unit_UnitCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_product_warning", new[] { "Unit_UnitCode" });
            DropIndex("dbo.wms_product_warning", new[] { "Storage_StorageCode" });
            DropIndex("dbo.wms_storage", new[] { "ProductWarning_ProductCode1" });
            DropIndex("dbo.wms_storage", new[] { "ProductWarning_ProductCode" });
            DropForeignKey("dbo.wms_product_warning", "Unit_UnitCode", "dbo.wms_unit");
            DropForeignKey("dbo.wms_product_warning", "Storage_StorageCode", "dbo.wms_storage");
            DropForeignKey("dbo.wms_storage", "ProductWarning_ProductCode1", "dbo.wms_product_warning");
            DropForeignKey("dbo.wms_storage", "ProductWarning_ProductCode", "dbo.wms_product_warning");
            DropColumn("dbo.wms_product_warning", "Unit_UnitCode");
            DropColumn("dbo.wms_product_warning", "Storage_StorageCode");
            DropColumn("dbo.wms_storage", "ProductWarning_ProductCode1");
            DropColumn("dbo.wms_storage", "ProductWarning_ProductCode");
        }
    }
}
