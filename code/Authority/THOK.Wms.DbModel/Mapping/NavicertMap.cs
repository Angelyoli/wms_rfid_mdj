using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class NavicertMap : EntityMappingBase<Navicert>
    {
        public NavicertMap()
            : base("Inter")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired();
            this.Property(t => t.MasterID)
                .IsRequired();
            this.Property(t => t.NavicertCode)
                .IsRequired()
                .HasMaxLength(8);
            this.Property(t => t.TruckPlateNo)
                .HasMaxLength(50);
            this.Property(t => t.ContractCode)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.MasterID).HasColumnName(ColumnMap.Value.To("MasterID"));
            this.Property(t => t.NavicertCode).HasColumnName(ColumnMap.Value.To("NavicertCode"));
            this.Property(t => t.NavicertDate).HasColumnName(ColumnMap.Value.To("NavicertDate"));
            this.Property(t => t.TruckPlateNo).HasColumnName(ColumnMap.Value.To("TruckPlateNo"));
            this.Property(t => t.ContractCode).HasColumnName(ColumnMap.Value.To("ContractCode"));           

            // Relationships
            this.HasRequired(t => t.BillMaster)
                .WithMany(t => t.Navicerts)
                .HasForeignKey(d => d.MasterID)
                .WillCascadeOnDelete(false);
        }
    }
}
