namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_system_parameter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_system_parameter",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        parameter_name = c.String(nullable: false, maxLength: 20),
                        parameter_value = c.String(nullable: false, maxLength: 20),
                        remark = c.String(nullable: false, maxLength: 20),
                        user_name = c.String(nullable: false, maxLength: 20),
                        system_id = c.Guid(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wms_system_parameter");
        }
    }
}
