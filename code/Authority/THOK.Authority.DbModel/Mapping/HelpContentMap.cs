using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class HelpContentMap : EntityMappingBase<HelpContent>
    {
        public HelpContentMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ContentCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ContentName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ContentPath)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.NodeType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.NodeOrder)
                .IsRequired();

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.ContentCode).HasColumnName(ColumnMap.Value.To("ContentCode"));
            this.Property(t => t.ContentName).HasColumnName(ColumnMap.Value.To("ContentName"));
            this.Property(t => t.ContentText).HasColumnName(ColumnMap.Value.To("ContentText"));
            this.Property(t => t.ContentPath).HasColumnName(ColumnMap.Value.To("ContentPath"));
            this.Property(t => t.NodeType).HasColumnName(ColumnMap.Value.To("NodeType"));
            this.Property(t => t.FatherNodeID).HasColumnName(ColumnMap.Value.To("FatherNodeID"));
            this.Property(t => t.ModuleID).HasColumnName(ColumnMap.Value.To("ModuleID"));
            this.Property(t => t.NodeOrder).HasColumnName(ColumnMap.Value.To("NodeOrder"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));


            // Relationships
            this.HasRequired(t => t.FatherNode)
                .WithMany(t => t.HelpContents)
                .HasForeignKey(d => d.FatherNodeID);
            this.HasRequired(t => t.Module)
                .WithMany(t => t.HelpModules)
                .HasForeignKey(d => d.ModuleID);

        }
    }
}