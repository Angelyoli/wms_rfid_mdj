using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class ChannelMap : EntityMappingBase<Channel>
    {
        public ChannelMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.ChannelCode);

            // Properties
            this.Property(t => t.ChannelCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.SortingLineCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.ChannelName)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.ChannelType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);
            this.Property(t => t.LedCode)
                .HasMaxLength(20);
            this.Property(t => t.DefaultProductCode)
                .HasMaxLength(20);
            this.Property(t => t.RemainQuantity)
                .IsRequired();
            this.Property(t => t.MiddleQuantity)
                .IsRequired();
            this.Property(t => t.MaxQuantity)
                .IsRequired();
            this.Property(t => t.GroupNo)
                .IsRequired();
            this.Property(t => t.OrderNo)
                .IsRequired();
            this.Property(t => t.Address)
                .IsRequired();
            this.Property(t => t.CellCode)
                .HasMaxLength(20);
            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.Property(t => t.ChannelCode).HasColumnName(ColumnMap.Value.To("ChannelCode"));
            this.Property(t => t.SortingLineCode).HasColumnName(ColumnMap.Value.To("SortingLineCode"));
            this.Property(t => t.ChannelName).HasColumnName(ColumnMap.Value.To("ChannelName"));
            this.Property(t => t.ChannelType).HasColumnName(ColumnMap.Value.To("ChannelType"));
            this.Property(t => t.LedCode).HasColumnName(ColumnMap.Value.To("LedCode"));
            this.Property(t => t.DefaultProductCode).HasColumnName(ColumnMap.Value.To("DefaultProductCode"));
            this.Property(t => t.RemainQuantity).HasColumnName(ColumnMap.Value.To("RemainQuantity"));
            this.Property(t => t.MiddleQuantity).HasColumnName(ColumnMap.Value.To("MiddleQuantity"));
            this.Property(t => t.MaxQuantity).HasColumnName(ColumnMap.Value.To("MaxQuantity"));
            this.Property(t => t.GroupNo).HasColumnName(ColumnMap.Value.To("GroupNo"));
            this.Property(t => t.OrderNo).HasColumnName(ColumnMap.Value.To("OrderNo"));
            this.Property(t => t.Address).HasColumnName(ColumnMap.Value.To("Address"));
            this.Property(t => t.CellCode).HasColumnName(ColumnMap.Value.To("CellCode"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
            this.HasRequired(t => t.led)
                .WithMany(t => t.Channels)
                .HasForeignKey(d => d.LedCode)
                .WillCascadeOnDelete(false);

        }
    }
}
