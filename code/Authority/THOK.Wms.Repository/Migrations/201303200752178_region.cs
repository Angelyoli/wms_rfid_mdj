namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class region : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_region",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        region_name = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wms_region");
        }
    }
}
