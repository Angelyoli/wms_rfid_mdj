namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_BusinessSystemsDailyBalance_id_Identity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_business_systems_daily_balance", "id", c => c.Guid(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_business_systems_daily_balance", "id", c => c.Guid(nullable: false));
        }
    }
}
