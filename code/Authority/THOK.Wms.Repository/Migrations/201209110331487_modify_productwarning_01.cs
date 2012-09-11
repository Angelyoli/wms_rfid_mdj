namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_productwarning_01 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.wms_storage", "ProductWarning_ProductCode", "dbo.wms_product_warning");
            DropForeignKey("dbo.wms_storage", "ProductWarning_ProductCode1", "dbo.wms_product_warning");
            DropForeignKey("dbo.wms_product_warning", "Storage_StorageCode", "dbo.wms_storage");
            DropIndex("dbo.wms_storage", new[] { "ProductWarning_ProductCode" });
            DropIndex("dbo.wms_storage", new[] { "ProductWarning_ProductCode1" });
            DropIndex("dbo.wms_product_warning", new[] { "Storage_StorageCode" });
            RenameColumn(table: "dbo.wms_product_warning", name: "Unit_UnitCode", newName: "unit_code");
            DropColumn("dbo.wms_storage", "ProductWarning_ProductCode");
            DropColumn("dbo.wms_storage", "ProductWarning_ProductCode1");
            DropColumn("dbo.wms_product_warning", "Storage_StorageCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wms_product_warning", "Storage_StorageCode", c => c.String(maxLength: 50));
            AddColumn("dbo.wms_storage", "ProductWarning_ProductCode1", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_storage", "ProductWarning_ProductCode", c => c.String(maxLength: 20));
            RenameColumn(table: "dbo.wms_product_warning", name: "unit_code", newName: "Unit_UnitCode");
            CreateIndex("dbo.wms_product_warning", "Storage_StorageCode");
            CreateIndex("dbo.wms_storage", "ProductWarning_ProductCode1");
            CreateIndex("dbo.wms_storage", "ProductWarning_ProductCode");
            AddForeignKey("dbo.wms_product_warning", "Storage_StorageCode", "dbo.wms_storage", "storage_code");
            AddForeignKey("dbo.wms_storage", "ProductWarning_ProductCode1", "dbo.wms_product_warning", "product_code");
            AddForeignKey("dbo.wms_storage", "ProductWarning_ProductCode", "dbo.wms_product_warning", "product_code");
        }
    }
}
