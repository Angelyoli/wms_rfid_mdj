using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class BillDetailMap : EntityMappingBase<BillDetail>
    {
        public BillDetailMap()
            : base("Inter")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired();
            this.Property(t => t.MasterID)
                .IsRequired();
            this.Property(t => t.PieceCigarCode)
                .IsRequired()
                .HasMaxLength(13);
            this.Property(t => t.BoxCigarCode)
                .IsRequired()
                .HasMaxLength(13);
            this.Property(t => t.BillQuantity)
                .IsRequired()
                .HasPrecision(16, 4);
            this.Property(t => t.FixedQuantity)
                .IsRequired()
                .HasPrecision(16, 4);
            this.Property(t => t.RealQuantity)
                .IsRequired()
                .HasPrecision(16, 4);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.MasterID).HasColumnName(ColumnMap.Value.To("MasterID"));
            this.Property(t => t.PieceCigarCode).HasColumnName(ColumnMap.Value.To("PieceCigarCode"));
            this.Property(t => t.BoxCigarCode).HasColumnName(ColumnMap.Value.To("BoxCigarCode"));
            this.Property(t => t.BillQuantity).HasColumnName(ColumnMap.Value.To("BillQuantity"));
            this.Property(t => t.FixedQuantity).HasColumnName(ColumnMap.Value.To("FixedQuantity"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));

            // Relationships
            this.HasRequired(t => t.BillMaster)
                .WithMany(t => t.BillDetails)
                .HasForeignKey(d => d.MasterID)
                .WillCascadeOnDelete(false);
        }
    }
}
