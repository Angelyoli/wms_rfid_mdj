using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class ChannelAllotMap : EntityMappingBase<ChannelAllot>
    {
        public ChannelAllotMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.ChannelAllotCode);

            // Properties
            this.Property(t => t.ChannelAllotCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.BatchSortId)
                .IsRequired();
            this.Property(t => t.ChannelCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.ProductCode)
                .HasMaxLength(20);
            this.Property(t => t.ProductName)
                .HasMaxLength(50);
            this.Property(t => t.InQuantity)
                .IsRequired();
            this.Property(t => t.OutQuantity)
               .IsRequired();
            this.Property(t => t.RealQuantity)
                .IsRequired();
            this.Property(t => t.RemainQuantity)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ChannelAllotCode).HasColumnName(ColumnMap.Value.To("ChannelAllotCode"));
            this.Property(t => t.BatchSortId).HasColumnName(ColumnMap.Value.To("BatchSortId"));
            this.Property(t => t.ChannelCode).HasColumnName(ColumnMap.Value.To("ChannelCode"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.ProductName).HasColumnName(ColumnMap.Value.To("ProductName"));
            this.Property(t => t.InQuantity).HasColumnName(ColumnMap.Value.To("InQuantity"));
            this.Property(t => t.OutQuantity).HasColumnName(ColumnMap.Value.To("OutQuantity"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));
            this.Property(t => t.RemainQuantity).HasColumnName(ColumnMap.Value.To("RemainQuantity"));

            // Relationships
            this.HasRequired(t => t.batchSort)
                .WithMany(t => t.ChannelAllots)
                .HasForeignKey(d => d.BatchSortId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.channel)
                .WithMany(t => t.ChannelAllots)
                .HasForeignKey(d => d.ChannelCode)
                .WillCascadeOnDelete(false);

        }
    }
}
