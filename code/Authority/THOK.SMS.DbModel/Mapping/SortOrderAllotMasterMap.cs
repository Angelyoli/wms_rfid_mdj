using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class SortOrderAllotMasterMap : EntityMappingBase<SortOrderAllotMaster>
    {
        public SortOrderAllotMasterMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.OrderMasterCode);

            // Properties
            this.Property(t => t.OrderMasterCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.BatchSortId)
                .IsRequired();
            this.Property(t => t.PackNo)
                .IsRequired();
            this.Property(t => t.OrderId)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.CustomerOrder)
                .IsRequired();
            this.Property(t => t.CustomerDeliverOrder)
                .IsRequired();
            this.Property(t => t.Quantity)
                .IsRequired();
            this.Property(t => t.ExportNo)
                .IsRequired();
            this.Property(t => t.StartTime);
            this.Property(t => t.FinishTime);
            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.Property(t => t.OrderMasterCode).HasColumnName(ColumnMap.Value.To("OrderMasterCode"));
            this.Property(t => t.BatchSortId).HasColumnName(ColumnMap.Value.To("BatchSortId"));
            this.Property(t => t.PackNo).HasColumnName(ColumnMap.Value.To("PackNo"));
            this.Property(t => t.OrderId).HasColumnName(ColumnMap.Value.To("OrderId"));
            this.Property(t => t.CustomerOrder).HasColumnName(ColumnMap.Value.To("CustomerOrder"));
            this.Property(t => t.CustomerDeliverOrder).HasColumnName(ColumnMap.Value.To("CustomerDeliverOrder"));
            this.Property(t => t.Quantity).HasColumnName(ColumnMap.Value.To("Quantity"));
            this.Property(t => t.ExportNo).HasColumnName(ColumnMap.Value.To("ExportNo"));
            this.Property(t => t.StartTime).HasColumnName(ColumnMap.Value.To("StartTime"));
            this.Property(t => t.FinishTime).HasColumnName(ColumnMap.Value.To("FinishTime"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
            this.HasRequired(t => t.batchSort)
                .WithMany(t => t.SortOrderAllotMasters)
                .HasForeignKey(d => d.BatchSortId)
                .WillCascadeOnDelete(false);

        }
    }
}
