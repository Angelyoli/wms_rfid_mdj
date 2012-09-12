namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_help : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.auth_help_content",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        content_code = c.String(nullable: false, maxLength: 20),
                        content_name = c.String(nullable: false, maxLength: 50),
                        content_text = c.String(),
                        content_path = c.String(nullable: false, maxLength: 100),
                        node_type = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        father_node_id = c.Guid(nullable: false),
                        module_id = c.Guid(nullable: false),
                        node_order = c.Int(nullable: false),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.auth_help_content", t => t.father_node_id)
                .ForeignKey("dbo.auth_module", t => t.module_id, cascadeDelete: true)
                .Index(t => t.father_node_id)
                .Index(t => t.module_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.auth_help_content", new[] { "module_id" });
            DropIndex("dbo.auth_help_content", new[] { "father_node_id" });
            DropForeignKey("dbo.auth_help_content", "module_id", "dbo.auth_module");
            DropForeignKey("dbo.auth_help_content", "father_node_id", "dbo.auth_help_content");
            DropTable("dbo.auth_help_content");
        }
    }
}
