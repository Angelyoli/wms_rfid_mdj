using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class SortSupplyMap : EntityMappingBase<SortSupply>
    {
        public SortSupplyMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.SortSupplyCode);

            // Properties
            this.Property(t => t.SortSupplyCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.BatchSortId)
                .IsRequired();
            this.Property(t => t.SupplyId)
                .IsRequired();
            this.Property(t => t.PackNo)
               .IsRequired();
            this.Property(t => t.ChannelCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.ProductCode)
                .HasMaxLength(20);
            this.Property(t => t.ProductName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.Property(t => t.SortSupplyCode).HasColumnName(ColumnMap.Value.To("SortSupplyCode"));
            this.Property(t => t.BatchSortId).HasColumnName(ColumnMap.Value.To("BatchSortId"));
            this.Property(t => t.SupplyId).HasColumnName(ColumnMap.Value.To("SupplyId"));
            this.Property(t => t.PackNo).HasColumnName(ColumnMap.Value.To("PackNo"));
            this.Property(t => t.ChannelCode).HasColumnName(ColumnMap.Value.To("ChannelCode"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.ProductName).HasColumnName(ColumnMap.Value.To("ProductName"));

            // Relationships
            this.HasRequired(t => t.batchSort)
                .WithMany(t => t.SortSupplys)
                .HasForeignKey(d => d.BatchSortId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.channel)
                .WithMany(t => t.SortSupplys)
                .HasForeignKey(d => d.ChannelCode)
                .WillCascadeOnDelete(false);

        }
    }
}
