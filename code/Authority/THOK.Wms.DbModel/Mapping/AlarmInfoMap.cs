using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class AlarmInfoMap : EntityMappingBase<AlarmInfo>
    {
        public AlarmInfoMap()
            : base("Wcs")
        {
            // Primary Key
            this.HasKey(t => t.AlarmCode);

            // Properties
            this.Property(t => t.AlarmCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.Property(t => t.AlarmCode).HasColumnName(ColumnMap.Value.To("AlarmCode"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
        }
    }
}
