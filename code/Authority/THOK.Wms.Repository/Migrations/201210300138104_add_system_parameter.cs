namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_system_parameter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.auth_system_parameter",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        parameter_name = c.String(nullable: false, maxLength: 20),
                        parameter_value = c.String(nullable: false, maxLength: 20),
                        remark = c.String(nullable: false, maxLength: 20),
                        user_name = c.String(nullable: false, maxLength: 20),
                        system_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.auth_system", t => t.system_id)
                .Index(t => t.system_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.auth_system_parameter", new[] { "system_id" });
            DropForeignKey("dbo.auth_system_parameter", "system_id", "dbo.auth_system");
            DropTable("dbo.auth_system_parameter");
        }
    }
}
