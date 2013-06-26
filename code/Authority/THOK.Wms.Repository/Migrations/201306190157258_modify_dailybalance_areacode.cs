namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_dailybalance_areacode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_daily_balance", "AreaCode", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.wms_daily_balance_history", "AreaCode", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.wms_daily_balance", "AreaCode");
            AddForeignKey("dbo.wms_daily_balance", "AreaCode", "dbo.wms_area", "area_code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.wms_daily_balance", "AreaCode", "dbo.wms_area");
            DropIndex("dbo.wms_daily_balance", new[] { "AreaCode" });
            DropColumn("dbo.wms_daily_balance_history", "AreaCode");
            DropColumn("dbo.wms_daily_balance", "AreaCode");
        }
    }
}
