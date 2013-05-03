namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_InterTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.inter_bill_detail", "bill_quantity", c => c.Decimal(nullable: false, precision: 16, scale: 4));
            AlterColumn("dbo.inter_bill_detail", "fixed_quantity", c => c.Decimal(nullable: false, precision: 16, scale: 4));
            AlterColumn("dbo.inter_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 16, scale: 4));
            AlterColumn("dbo.inter_contract_detail", "quantity", c => c.Decimal(nullable: false, precision: 16, scale: 4));
            AlterColumn("dbo.inter_contract_detail", "price", c => c.Decimal(nullable: false, precision: 16, scale: 2));
            AlterColumn("dbo.inter_contract_detail", "amount", c => c.Decimal(nullable: false, precision: 16, scale: 2));
            AlterColumn("dbo.inter_contract_detail", "tax_amount", c => c.Decimal(nullable: false, precision: 16, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.inter_contract_detail", "tax_amount", c => c.Int(nullable: false));
            AlterColumn("dbo.inter_contract_detail", "amount", c => c.Int(nullable: false));
            AlterColumn("dbo.inter_contract_detail", "price", c => c.String());
            AlterColumn("dbo.inter_contract_detail", "quantity", c => c.String(nullable: false));
            AlterColumn("dbo.inter_bill_detail", "real_quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.inter_bill_detail", "fixed_quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.inter_bill_detail", "bill_quantity", c => c.Int(nullable: false));
        }
    }
}
