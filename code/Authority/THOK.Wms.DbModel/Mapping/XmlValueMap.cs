using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
   public class XmlValueMap : EntityMappingBase<XmlValue>
    {
       public XmlValueMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.DateTime);

            this.Property(t => t.XmlValueText)
                .IsMaxLength();

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.DateTime).HasColumnName(ColumnMap.Value.To("DateTime"));
            this.Property(t => t.XmlValueText).HasColumnName(ColumnMap.Value.To("XmlValueText"));
            
            // Relationships
        }
    }
}
