using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class DwvIoutSynchroInfoMap:EntityMappingBase<DwvIoutSynchroInfo>
    {
        public DwvIoutSynchroInfoMap()
            : base("Wms")
        {

            // Primary Key
            this.HasKey(t => t.ID);

            this.Property(t => t.SyncTypeCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SyncTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
               .IsRequired()
               .HasMaxLength(200);

            this.Property(t => t.UpdateDate)
               .IsRequired()
               .HasMaxLength(14);

            this.Property(t => t.IsImport)
               .IsRequired()
               .HasMaxLength(1);

            this.Property(t => t.Remark)
               .IsRequired()
               .HasMaxLength(100);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.SyncTypeCode).HasColumnName(ColumnMap.Value.To("SyncTypeCode"));
            this.Property(t => t.SyncTypeName).HasColumnName(ColumnMap.Value.To("SyncTypeName"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.UpdateDate).HasColumnName(ColumnMap.Value.To("UpdateDate"));
            this.Property(t => t.IsImport).HasColumnName(ColumnMap.Value.To("IsImport"));
            this.Property(t => t.Remark).HasColumnName(ColumnMap.Value.To("Remark"));
        }
    }
}
