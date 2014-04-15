namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSmsBatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sms_batch",
                c => new
                    {
                        batch_id = c.Int(nullable: false, identity: true),
                        order_date = c.DateTime(nullable: false),
                        batch_no = c.Int(nullable: false),
                        batch_name = c.String(nullable: false, maxLength: 100),
                        operate_date = c.DateTime(nullable: false),
                        operate_person_id = c.Guid(nullable: false),
                        optimize_schedule = c.Int(nullable: false),
                        verify_person_id = c.Guid(nullable: false),
                        description = c.String(maxLength: 200),
                        project_batch_no = c.Int(nullable: false),
                        state = c.String(nullable: false, maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.batch_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.sms_batch");
        }
    }
}
