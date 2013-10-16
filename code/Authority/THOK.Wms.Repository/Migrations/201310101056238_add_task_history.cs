namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_task_history : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wcs_task_history",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        task_id = c.Int(nullable: false),
                        task_type = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        task_level = c.Int(nullable: false),
                        path_id = c.Int(nullable: false),
                        product_code = c.String(nullable: false, maxLength: 20),
                        product_name = c.String(nullable: false, maxLength: 20),
                        origin_storage_code = c.String(nullable: false, maxLength: 50),
                        target_storage_code = c.String(nullable: false, maxLength: 50),
                        origin_position_id = c.Int(nullable: false),
                        target_position_id = c.Int(nullable: false),
                        current_position_id = c.Int(nullable: false),
                        current_position_state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        tag_state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        quantity = c.Int(nullable: false),
                        task_quantity = c.Int(nullable: false),
                        operate_quantity = c.Int(nullable: false),
                        order_id = c.String(maxLength: 20),
                        order_type = c.String(maxLength: 2, fixedLength: true),
                        allot_id = c.Int(nullable: false),
                        download_state = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        storage_sequence = c.Int(nullable: false),
                        clear_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wcs_task_history");
        }
    }
}
