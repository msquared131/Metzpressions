using System.Linq.Expressions;

namespace Metzpressions
{
    public static class ModifyExpressioncs
    {
        public static Expression ReverseLambda(LambdaExpression exp)
        {
            Reverse modifier = new Reverse();
            return modifier.Modify(exp);
        }
    }
}
