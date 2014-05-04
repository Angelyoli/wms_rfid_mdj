using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class BatchStatusMap : EntityMappingBase<BatchStatus>
    {
        public BatchStatusMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.BatchStatusId);

            // Properties
            this.Property(t => t.BatchStatusId)
                .IsRequired();
            this.Property(t => t.BatchId)
                .IsRequired();
            this.Property(t => t.SortingLineCode)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.State)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.Property(t => t.BatchStatusId).HasColumnName(ColumnMap.Value.To("BatchStatusId"));
            this.Property(t => t.BatchId).HasColumnName(ColumnMap.Value.To("BatchId"));
            this.Property(t => t.SortingLineCode).HasColumnName(ColumnMap.Value.To("SortingLineCode"));
            this.Property(t => t.State).HasColumnName(ColumnMap.Value.To("State"));

            // Relationships
            this.HasRequired(t => t.batch)
                .WithMany(t => t.BatchStatuss)
                .HasForeignKey(d => d.BatchId)
                .WillCascadeOnDelete(false);
        }
    }
}

