using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class SortOrderAllotDetailMap : EntityMappingBase<SortOrderAllotDetail>
    {
        public SortOrderAllotDetailMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.OrderDetailCode);

            // Properties
            this.Property(t => t.OrderDetailCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.OrderMasterCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.ChannelCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.Quantity)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.OrderDetailCode).HasColumnName(ColumnMap.Value.To("OrderDetailCode"));
            this.Property(t => t.OrderMasterCode).HasColumnName(ColumnMap.Value.To("OrderMasterCode"));
            this.Property(t => t.ChannelCode).HasColumnName(ColumnMap.Value.To("ChannelCode"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.ProductName).HasColumnName(ColumnMap.Value.To("ProductName"));
            this.Property(t => t.Quantity).HasColumnName(ColumnMap.Value.To("Quantity"));

            // Relationships
            this.HasRequired(t => t.sortOrderAllotMaster)
                .WithMany(t => t.SortOrderAllotDetails)
                .HasForeignKey(d => d.OrderMasterCode)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.channel)
                .WithMany(t => t.SortOrderAllotDetails)
                .HasForeignKey(d => d.ChannelCode)
                .WillCascadeOnDelete(false);

        }
    }
}
