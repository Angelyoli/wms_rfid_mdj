using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class BillMasterMap : EntityMappingBase<BillMaster>
    {
        public BillMasterMap()
            : base("Inter")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired();
            this.Property(t => t.UUID)
                .HasMaxLength(64);
            this.Property(t => t.BillType)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.BillDate)
                .IsRequired();
            this.Property(t => t.MakerName)
                .HasMaxLength(50);
            this.Property(t => t.OperateDate);
            this.Property(t => t.CigaretteType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);
            this.Property(t => t.BillCompanyCode)
                .IsRequired()
                .HasMaxLength(8);
            this.Property(t => t.SupplierCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.SupplierType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);
            this.Property(t => t.State)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);
         
            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.UUID).HasColumnName(ColumnMap.Value.To("UUID"));
            this.Property(t => t.BillType).HasColumnName(ColumnMap.Value.To("BillType"));
            this.Property(t => t.BillDate).HasColumnName(ColumnMap.Value.To("BillDate"));
            this.Property(t => t.MakerName).HasColumnName(ColumnMap.Value.To("MakerName"));
            this.Property(t => t.OperateDate).HasColumnName(ColumnMap.Value.To("OperateDate"));
            this.Property(t => t.CigaretteType).HasColumnName(ColumnMap.Value.To("CigaretteType"));
            this.Property(t => t.BillCompanyCode).HasColumnName(ColumnMap.Value.To("BillCompanyCode"));
            this.Property(t => t.SupplierCode).HasColumnName(ColumnMap.Value.To("SupplierCode"));
            this.Property(t => t.SupplierType).HasColumnName(ColumnMap.Value.To("SupplierType"));
            this.Property(t => t.State).HasColumnName(ColumnMap.Value.To("State"));

            // Relationships
        }
    }
}
