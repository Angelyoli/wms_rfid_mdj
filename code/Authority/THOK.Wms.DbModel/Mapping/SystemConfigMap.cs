using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class SystemConfigMap : EntityMappingBase<SystemConfig>
    {
        public SystemConfigMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired();

            this.Property(t => t.ParameterName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ParameterValue)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Remark)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.System)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.Property(t => t.Id).HasColumnName(ColumnMap.Value.To("Id"));
            this.Property(t => t.ParameterName).HasColumnName(ColumnMap.Value.To("ParameterName"));
            this.Property(t => t.ParameterValue).HasColumnName(ColumnMap.Value.To("ParameterValue"));
            this.Property(t => t.Remark).HasColumnName(ColumnMap.Value.To("Remark"));
            this.Property(t => t.Username).HasColumnName(ColumnMap.Value.To("Username"));
            this.Property(t => t.System).HasColumnName(ColumnMap.Value.To("System"));
        }
    }
}
