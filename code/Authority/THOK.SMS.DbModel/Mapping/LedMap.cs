using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.SMS.DbModel.Mapping
{
    public class LedMap : EntityMappingBase<Led>
    {
        public LedMap()
            : base("Sms")
        {
            // Primary Key
            this.HasKey(t => t.LedCode);

            // Properties
            this.Property(t => t.LedCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.SortingLineCode)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.LedName)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.LedType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);
            this.Property(t => t.LedIp)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.XAxes);
            this.Property(t => t.YAxes);
            this.Property(t => t.Width)
                .IsRequired();
            this.Property(t => t.Height)
                .IsRequired();
            this.Property(t => t.LedGroupCode)
                .HasMaxLength(20);
            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.LedCode).HasColumnName(ColumnMap.Value.To("LedCode"));
            this.Property(t => t.SortingLineCode).HasColumnName(ColumnMap.Value.To("SortingLineCode"));
            this.Property(t => t.LedName).HasColumnName(ColumnMap.Value.To("LedName"));
            this.Property(t => t.LedType).HasColumnName(ColumnMap.Value.To("LedType"));
            this.Property(t => t.LedIp).HasColumnName(ColumnMap.Value.To("LedIp"));
            this.Property(t => t.XAxes).HasColumnName(ColumnMap.Value.To("XAxes"));
            this.Property(t => t.YAxes).HasColumnName(ColumnMap.Value.To("YAxes"));
            this.Property(t => t.Width).HasColumnName(ColumnMap.Value.To("Width"));
            this.Property(t => t.Height).HasColumnName(ColumnMap.Value.To("Height"));
            this.Property(t => t.LedGroupCode).HasColumnName(ColumnMap.Value.To("LedGroupCode"));
            this.Property(t => t.OrderNo).HasColumnName(ColumnMap.Value.To("OrderNo"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));


            // Relationships
            this.HasRequired(t => t.GroupLed)
                .WithMany(t => t.Leds)
                .HasForeignKey(d => d.LedGroupCode)
                .WillCascadeOnDelete(false);
        }
    }
}
