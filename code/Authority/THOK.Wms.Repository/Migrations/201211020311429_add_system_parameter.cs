namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_system_parameter : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.auth_system_parameter", "system_id", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.auth_system_parameter", "system_id", c => c.Guid(nullable: false));
        }
    }
}
