namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Xml_Value : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_xml_value",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        date_time = c.DateTime(nullable: false),
                        xml_value_text = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wms_xml_value");
        }
    }
}
