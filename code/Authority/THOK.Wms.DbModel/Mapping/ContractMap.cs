using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class ContractMap : EntityMappingBase<Contract>
    {
        public ContractMap() : base("Inter")
        {
            // Primary Key
            this.HasKey(t => t.ContractCode);

            // Properties
            this.Property(t => t.ContractCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.MasterID)
                .IsRequired();
            this.Property(t => t.SupplySideCode)
                .HasMaxLength(50);
            this.Property(t => t.DemandSideCode)
                .HasMaxLength(50);
            this.Property(t => t.ContractDate);
            this.Property(t => t.StartDade);
            this.Property(t => t.EndDate);
            this.Property(t => t.SupplySideCode)
                .HasMaxLength(100);
            this.Property(t => t.SendAddress)
                .HasMaxLength(100);
            this.Property(t => t.ReceivePlaceCode)
                .HasMaxLength(100);
            this.Property(t => t.ReceiveAddress)
                .HasMaxLength(100);
            this.Property(t => t.SaleDate)
                .HasMaxLength(100);
            this.Property(t => t.State)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.ContractCode).HasColumnName(ColumnMap.Value.To("ContractCode"));
            this.Property(t => t.MasterID).HasColumnName(ColumnMap.Value.To("MasterID"));
            this.Property(t => t.SupplySideCode).HasColumnName(ColumnMap.Value.To("SupplySideCode"));
            this.Property(t => t.DemandSideCode).HasColumnName(ColumnMap.Value.To("DemandSideCode"));
            this.Property(t => t.ContractDate).HasColumnName(ColumnMap.Value.To("ContractDate"));
            this.Property(t => t.StartDade).HasColumnName(ColumnMap.Value.To("StartDade"));
            this.Property(t => t.EndDate).HasColumnName(ColumnMap.Value.To("EndDate"));
            this.Property(t => t.SendPlaceCode).HasColumnName(ColumnMap.Value.To("SendPlaceCode"));
            this.Property(t => t.SendAddress).HasColumnName(ColumnMap.Value.To("SendAddress"));
            this.Property(t => t.ReceivePlaceCode).HasColumnName(ColumnMap.Value.To("ReceivePlaceCode"));
            this.Property(t => t.ReceiveAddress).HasColumnName(ColumnMap.Value.To("ReceiveAddress"));
            this.Property(t => t.SaleDate).HasColumnName(ColumnMap.Value.To("SaleDate"));
            this.Property(t => t.State).HasColumnName(ColumnMap.Value.To("State"));

            // Relationships
        }
    }
}
