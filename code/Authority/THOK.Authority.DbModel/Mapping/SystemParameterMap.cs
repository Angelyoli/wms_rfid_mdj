using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class SystemParameterMap : EntityMappingBase<SystemParameter>
    {
        public SystemParameterMap()
            : base("Auth")
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

            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SystemID);

            // Table & Column Mappings
            this.Property(t => t.Id).HasColumnName(ColumnMap.Value.To("Id"));
            this.Property(t => t.ParameterName).HasColumnName(ColumnMap.Value.To("ParameterName"));
            this.Property(t => t.ParameterValue).HasColumnName(ColumnMap.Value.To("ParameterValue"));
            this.Property(t => t.Remark).HasColumnName(ColumnMap.Value.To("Remark"));
            this.Property(t => t.UserName).HasColumnName(ColumnMap.Value.To("UserName"));
            this.Property(t => t.SystemID).HasColumnName(ColumnMap.Value.To("SystemID"));

            // Relationships
            this.HasRequired(t => t.System)
                .WithMany(t => t.SystemParameters)
                .HasForeignKey(d => d.SystemID)
                .WillCascadeOnDelete(false);
        }
    }
}
