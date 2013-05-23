namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_alarmInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wcs_alarm_info",
                c => new
                    {
                        alarm_code = c.String(nullable: false, maxLength: 20),
                        description = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.alarm_code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wcs_alarm_info");
        }
    }
}
