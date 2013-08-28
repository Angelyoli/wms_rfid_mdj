namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_task : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wcs_task", "Position_ID", c => c.Int());
            CreateIndex("dbo.wcs_task", "path_id");
            CreateIndex("dbo.wcs_task", "origin_position_id");
            CreateIndex("dbo.wcs_task", "target_position_id");
            CreateIndex("dbo.wcs_task", "current_position_id");
            CreateIndex("dbo.wcs_task", "Position_ID");
            AddForeignKey("dbo.wcs_task", "path_id", "dbo.wcs_path", "id");
            AddForeignKey("dbo.wcs_task", "origin_position_id", "dbo.wcs_position", "id");
            AddForeignKey("dbo.wcs_task", "target_position_id", "dbo.wcs_position", "id");
            AddForeignKey("dbo.wcs_task", "current_position_id", "dbo.wcs_position", "id");
            AddForeignKey("dbo.wcs_task", "Position_ID", "dbo.wcs_position", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.wcs_task", "Position_ID", "dbo.wcs_position");
            DropForeignKey("dbo.wcs_task", "current_position_id", "dbo.wcs_position");
            DropForeignKey("dbo.wcs_task", "target_position_id", "dbo.wcs_position");
            DropForeignKey("dbo.wcs_task", "origin_position_id", "dbo.wcs_position");
            DropForeignKey("dbo.wcs_task", "path_id", "dbo.wcs_path");
            DropIndex("dbo.wcs_task", new[] { "Position_ID" });
            DropIndex("dbo.wcs_task", new[] { "current_position_id" });
            DropIndex("dbo.wcs_task", new[] { "target_position_id" });
            DropIndex("dbo.wcs_task", new[] { "origin_position_id" });
            DropIndex("dbo.wcs_task", new[] { "path_id" });
            DropColumn("dbo.wcs_task", "Position_ID");
        }
    }
}
