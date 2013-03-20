using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRabbit.HashOrm
{
    public class Filter:IFilter 
    {
        private string Expression = "1=1";
        public Filter()
        {
        }
        public Filter(string colName,object Value,ComparisonOperators cop)
        {
            if (SqlFactory.GetColType(Value.GetType()) == 1)
            {
                Expression = System.String.Format(" {0} {1} '{2}' ", colName, cops.getcopstr(cop), Value);
            }
            else
            {
                Expression = System.String.Format(" {0} {1} {2} ", colName, cops.getcopstr(cop), Value);
            }
        }

        #region IFilter 成员

        public string GetExpression()
        {
            return System.String.Format("({0})", Expression); 
        }

        #endregion
    }
}
