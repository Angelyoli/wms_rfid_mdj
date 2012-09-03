using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class DayEndMap : EntityMappingBase<DayEnd>
    {
            public DayEndMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.DayEndID);

            // Properties
            this.Property(t => t.DayEndID)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.DaySettleDate)
                .IsRequired();

            this.Property(t => t.DayWarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DayProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DayUnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DayBeginning)
                .IsRequired();

            this.Property(t => t.DayEntryAmount)
                .IsRequired();

            this.Property(t => t.DayDeliveryAmount)
                .IsRequired();

            this.Property(t => t.DayProfitAmount)
                .IsRequired();

            this.Property(t => t.DayLossAmount)
                .IsRequired();

            this.Property(t => t.DayEnding)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.DayEndID).HasColumnName(ColumnMap.Value.To("DayEndID"));
            this.Property(t => t.DaySettleDate).HasColumnName(ColumnMap.Value.To("DaySettleDate"));
            this.Property(t => t.DayWarehouseCode).HasColumnName(ColumnMap.Value.To("DayWarehouseCode"));
            this.Property(t => t.DayProductCode).HasColumnName(ColumnMap.Value.To("DayProductCode"));
            this.Property(t => t.DayUnitCode).HasColumnName(ColumnMap.Value.To("DayUnitCode"));
            this.Property(t => t.DayBeginning).HasColumnName(ColumnMap.Value.To("DayBeginning"));
            this.Property(t => t.DayEntryAmount).HasColumnName(ColumnMap.Value.To("DayEntryAmount"));
            this.Property(t => t.DayDeliveryAmount).HasColumnName(ColumnMap.Value.To("DayDeliveryAmount"));
            this.Property(t => t.DayProfitAmount).HasColumnName(ColumnMap.Value.To("DayProfitAmount"));
            this.Property(t => t.DayLossAmount).HasColumnName(ColumnMap.Value.To("DayLossAmount"));
            this.Property(t => t.DayEnding).HasColumnName(ColumnMap.Value.To("DayEnding"));

            // Relationships
            this.HasRequired(t => t.Warehouse)
                .WithMany(t => t.DayEnds)
                .HasForeignKey(d => d.DayWarehouseCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.DayEnds)
                .HasForeignKey(d => d.DayProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.DayEnds)
                .HasForeignKey(d => d.DayUnitCode)
                .WillCascadeOnDelete(false);
        }
    }
}
