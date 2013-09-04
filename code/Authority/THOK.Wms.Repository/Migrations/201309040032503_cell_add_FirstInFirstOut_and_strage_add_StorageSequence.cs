namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cell_add_FirstInFirstOut_and_strage_add_StorageSequence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_cell", "first_in_first_out", c => c.Boolean(nullable: false));
            AddColumn("dbo.wms_storage", "storage_sequence", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_storage", "storage_sequence");
            DropColumn("dbo.wms_cell", "first_in_first_out");
        }
    }
}
