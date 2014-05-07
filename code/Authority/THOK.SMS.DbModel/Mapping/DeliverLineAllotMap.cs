using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class DeliverLineAllotMap : EntityMappingBase<DeliverLineAllot>
    {
        public DeliverLineAllotMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.DeliverLineAllotCode);

            // Properties
            this.Property(t => t.DeliverLineAllotCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.BatchSortId)
                .IsRequired();
            this.Property(t => t.DeliverLineCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.DeliverQuantity)
               .IsRequired();
            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.Property(t => t.DeliverLineAllotCode).HasColumnName(ColumnMap.Value.To("DeliverLineAllotCode"));
            this.Property(t => t.BatchSortId).HasColumnName(ColumnMap.Value.To("BatchSortId"));
            this.Property(t => t.DeliverLineCode).HasColumnName(ColumnMap.Value.To("DeliverLineCode"));
            this.Property(t => t.DeliverQuantity).HasColumnName(ColumnMap.Value.To("DeliverQuantity"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
            this.HasRequired(t => t.batchSort)
                .WithMany(t => t.DeliverLineAllots)
                .HasForeignKey(d => d.BatchSortId)
                .WillCascadeOnDelete(false);
        }
    }
}
