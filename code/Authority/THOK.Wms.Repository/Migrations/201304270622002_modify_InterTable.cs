namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_InterTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.inter_pallet", "operate_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.inter_pallet", "operate_date", c => c.DateTime(nullable: false));
        }
    }
}
