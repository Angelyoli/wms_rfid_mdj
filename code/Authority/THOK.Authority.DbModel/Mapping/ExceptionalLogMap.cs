using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class ExceptionalLogMap:EntityMappingBase<ExceptionalLog>
    {
        public ExceptionalLogMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.ExceptionalLogID);
            // Properties
            this.Property(t => t.CatchTime)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.ModuleName)
                .IsRequired();

            this.Property(t => t.FunctionName)
                .IsRequired();

            this.Property(t => t.ExceptionalType)
                .IsRequired();

            this.Property(t => t.ExceptionalDescription)
                .IsRequired();

            this.Property(t => t.State)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ExceptionalLogID).HasColumnName(ColumnMap.Value.To("ExceptionalLogID"));
            this.Property(t => t.CatchTime).HasColumnName(ColumnMap.Value.To("CatchTime"));
            this.Property(t => t.ModuleName).HasColumnName(ColumnMap.Value.To("ModuleName"));
            this.Property(t => t.FunctionName).HasColumnName(ColumnMap.Value.To("FunctionName"));
            this.Property(t => t.ExceptionalType).HasColumnName(ColumnMap.Value.To("ExceptionalType"));
            this.Property(t => t.ExceptionalDescription).HasColumnName(ColumnMap.Value.To("ExceptionalDescription"));
            this.Property(t => t.State).HasColumnName(ColumnMap.Value.To("State"));
        }
    }
}
