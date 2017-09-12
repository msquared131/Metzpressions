using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Metzpressions
{
    internal class CombineTerms : ExpressionVisitor
    {
        public Expression Modify(Expression exp)
        {
            return Visit(exp);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            var left = Visit(b.Left);
            var right = Visit(b.Right);
            var LeftLeft = ((BinaryExpression)left).Left;
            var LeftRight = ((BinaryExpression)left).Right;
            var RightLeft = ((BinaryExpression)right).Left;
            var RightRight = ((BinaryExpression)right).Right;
            decimal leftConstant = 0;
            decimal rightConstant = 0;
            Expression parameter = Expression.Parameter(typeof(decimal), "x");

            if (b.Type == typeof(BinaryExpression))
            {
                switch (b.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant(leftConstant + rightConstant), parameter);                        
                }
            }
            return b;
        }

        private bool CanCombine(Expression left, Expression right)
        {

            return false;
        }
    }
}
