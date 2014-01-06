namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_system_parameter_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.auth_system_parameter", "parameter_name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.auth_system_parameter", "parameter_value", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.auth_system_parameter", "remark", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.auth_system_parameter", "remark", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.auth_system_parameter", "parameter_value", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.auth_system_parameter", "parameter_name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
