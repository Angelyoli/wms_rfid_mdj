using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRabbit.HashOrm
{
    public interface IFilter
    {
        String GetExpression();
    }
}
