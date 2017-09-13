using System.Linq.Expressions;

namespace Metzpressions
{
    public static class ModifyExpressions
    {
        public static Expression ReverseLambda(LambdaExpression exp)
        {
            Reverse modifier = new Reverse();
            return modifier.Modify(exp);
        }

        public static Expression CombineTerms(Expression exp)
        {
            CombineTerms modifier = new CombineTerms();
            return modifier.Modify(exp);
        }
    }
}
