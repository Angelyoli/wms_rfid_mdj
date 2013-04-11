namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_exceptional_log : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.auth_exceptional_log",
                c => new
                    {
                        exceptional_log_id = c.Guid(nullable: false),
                        catch_time = c.String(nullable: false, maxLength: 30),
                        module_name = c.String(nullable: false),
                        function_name = c.String(nullable: false),
                        exceptional_type = c.String(nullable: false),
                        exceptional_description = c.String(nullable: false),
                        state = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.exceptional_log_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.auth_exceptional_log");
        }
    }
}
