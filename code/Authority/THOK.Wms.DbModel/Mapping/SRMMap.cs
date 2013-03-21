using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.Wms.DbModel.Mapping
{
    public class SRMMap : EntityMappingBase<SRM>
    {
        public SRMMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.SRMName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.State)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.SRMName).HasColumnName(ColumnMap.Value.To("SRMName"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.State).HasColumnName(ColumnMap.Value.To("State"));
        }
    }
}
