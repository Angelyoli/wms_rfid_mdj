using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class BatchMap : EntityMappingBase<Batch>
    {
        public BatchMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.BatchId);

            // Properties
            this.Property(t => t.BatchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.OrderDate)
                .IsRequired();
            this.Property(t => t.BatchNo)
                .IsRequired();
            this.Property(t => t.BatchName)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.OperateDate)
                .IsRequired();
            this.Property(t => t.OperatePersonId)
                .IsRequired();
            this.Property(t => t.OptimizeSchedule)
                .IsRequired();
            this.Property(t => t.VerifyPersonId);
            this.Property(t => t.Description)
                .HasMaxLength(200);
            this.Property(t => t.ProjectBatchNo);
            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);
         
            // Table & Column Mappings
            this.Property(t => t.BatchId).HasColumnName(ColumnMap.Value.To("BatchId"));
            this.Property(t => t.OrderDate).HasColumnName(ColumnMap.Value.To("OrderDate"));
            this.Property(t => t.BatchNo).HasColumnName(ColumnMap.Value.To("BatchNo"));
            this.Property(t => t.BatchName).HasColumnName(ColumnMap.Value.To("BatchName"));
            this.Property(t => t.OperateDate).HasColumnName(ColumnMap.Value.To("OperateDate"));
            this.Property(t => t.OperatePersonId).HasColumnName(ColumnMap.Value.To("OperatePersonId"));
            this.Property(t => t.OptimizeSchedule).HasColumnName(ColumnMap.Value.To("OptimizeSchedule"));
            this.Property(t => t.VerifyPersonId).HasColumnName(ColumnMap.Value.To("VerifyPersonId"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.ProjectBatchNo).HasColumnName(ColumnMap.Value.To("ProjectBatchNo"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
        }
    }
}

