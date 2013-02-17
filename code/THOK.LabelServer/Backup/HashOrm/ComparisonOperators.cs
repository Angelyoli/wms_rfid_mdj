using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRabbit.HashOrm
{
    public enum ComparisonOperators
    {
        Equal ,// "=";
        NotEqual ,// "!=";
        Greater ,// ">";
        Less ,// "<";
        NotLessThan ,// ">=";
        NotGreaterThan ,// "<=";
        Like ,// " like ";
        NotLike ,// " not like ";
        In ,// " in ";
        NotIn ,// " not in ";
        BetweenAnd // " between ";
    }
    class cops
    {
        public const string Equal = "=";
        public const string NotEqual = "!=";
        public const string Greater = ">";
        public const string Less = "<";
        public const string NotLessThan = ">=";
        public const string NotGreaterThan = "<=";
        public const string Like = " like ";
        public const string NotLike = " not like ";
        public const string In = " in ";
        public const string NotIn = " not in ";
        public const string BetweenAnd = " between ";
        public static string getcopstr(ComparisonOperators cop)
        {
            switch (cop)
            {
                case ComparisonOperators.Equal:
                    return cops.Equal;
                case ComparisonOperators.NotEqual:
                    return cops.NotEqual;
                case ComparisonOperators.Greater:
                    return cops.Greater;
                case ComparisonOperators.Less:
                    return cops.Less;
                case ComparisonOperators.NotLessThan:
                    return cops.NotLessThan;
                case ComparisonOperators.NotGreaterThan:
                    return cops.NotGreaterThan;
                case ComparisonOperators.Like:
                    return cops.Like;
                case ComparisonOperators.NotLike:
                    return cops.NotLike;
                case ComparisonOperators.In:
                    return cops.In;
                case ComparisonOperators.NotIn:
                    return cops.NotIn;
                case ComparisonOperators.BetweenAnd:
                    return cops.BetweenAnd;
                default:
                    return cops.Equal;
            }
        }
    }
}
