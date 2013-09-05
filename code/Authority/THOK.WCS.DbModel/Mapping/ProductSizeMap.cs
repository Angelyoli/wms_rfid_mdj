using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.WCS.DbModel.Mapping
{
    public class ProductSizeMap : EntityMappingBase<ProductSize>
    {
        public ProductSizeMap()
            : base("Wcs")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.ProductNo)
                .IsRequired();
            this.Property(t => t.SizeNo)
                .IsRequired();
            this.Property(t => t.AreaNo)
               .IsRequired();
            this.Property(t => t.Length)
               .IsRequired();
            this.Property(t => t.Width)
               .IsRequired();
            this.Property(t => t.Height)
               .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.ProductNo).HasColumnName(ColumnMap.Value.To("ProductNo"));
            this.Property(t => t.SizeNo).HasColumnName(ColumnMap.Value.To("SizeNo"));
            this.Property(t => t.AreaNo).HasColumnName(ColumnMap.Value.To("AreaNo"));
            this.Property(t => t.Length).HasColumnName(ColumnMap.Value.To("Length"));
            this.Property(t => t.Width).HasColumnName(ColumnMap.Value.To("Width"));
            this.Property(t => t.Height).HasColumnName(ColumnMap.Value.To("Height"));
        }
    }
}
