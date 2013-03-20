using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRabbit.HashOrm
{
    public class FilterTree:IFilter
    {
        private string Expression = "";
        public FilterTree(string expression,params IFilter [] filters)
        {
            if (filters.Count() > 0 )
            {
                List<string> filersStr= new List<string>();
                foreach (IFilter filter in filters)
                {
                    filersStr.Add(filter.GetExpression());
                }
                Expression = System.String.Format(expression, filersStr.ToArray());
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
