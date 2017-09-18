using System.Linq.Expressions;
using System.Collections.Generic;

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

            if (CanCombine(b.Left, b.Right))
            {
                Expression left;
                Expression right;
                switch (b.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant((double)GetConstant(b.Left).Value + (double)GetConstant(b.Right).Value), GetParameter(b.Left));
                    case ExpressionType.Subtract:
                        return Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant((double)GetConstant(b.Left).Value - (double)GetConstant(b.Right).Value), GetParameter(b.Left));
                    case ExpressionType.Multiply:
                        left = Expression.Constant((double)GetConstant(b.Left).Value * (double)GetConstant(b.Right).Value);
                        right = Expression.MakeBinary(ExpressionType.Power, GetParameter(b.Left), Expression.MakeBinary(ExpressionType.Add, GetExponent(b.Left), GetExponent(b.Right)));
                        return Expression.MakeBinary(ExpressionType.Multiply, left, right);
                    case ExpressionType.Divide:
                        left = Expression.Constant((double)GetConstant(b.Left).Value / (double)GetConstant(b.Right).Value);
                        right = Expression.MakeBinary(ExpressionType.Power, GetParameter(b.Left), Expression.MakeBinary(ExpressionType.Subtract, GetExponent(b.Left), GetExponent(b.Right)));
                        return Expression.MakeBinary(ExpressionType.Multiply, left, right);
                }
            }
            return b;
        }

        private bool CanCombine(Expression left, Expression right)
        {
            return (GetParameter(left) != null &&
                    GetParameter(left) == GetParameter(right));
        }

        private ParameterExpression GetParameter(Expression exp)
        {
            List<ExpressionType> binaryTypes = new List<ExpressionType> { ExpressionType.Multiply, ExpressionType.Divide, ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Power };
            if (binaryTypes.Contains(exp.NodeType))
            {
                var binary = (BinaryExpression)exp;
                var left = GetParameter(binary.Left);
                var right = GetParameter(binary.Right);

                if (left.NodeType == ExpressionType.Parameter)
                {
                    return left;
                }
                if (right.NodeType == ExpressionType.Parameter)
                {
                    return right;
                }
            }
            if (exp.NodeType == ExpressionType.Parameter)
            {
                return (ParameterExpression)exp;
            }
            return null;
        }

        private Expression GetExponent(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Power)
            {
                return ((BinaryExpression)exp).Right;
            }
            return null;
        }

        private ConstantExpression GetConstant(Expression exp)
        {
            List<ExpressionType> binaryTypes = new List<ExpressionType> { ExpressionType.Multiply, ExpressionType.Divide, ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Power };
            if (binaryTypes.Contains(exp.NodeType))
            {
                var binary = (BinaryExpression)exp;
                var left = GetConstant(binary.Left);
                var right = GetConstant(binary.Right);

                if (left.NodeType == ExpressionType.Constant)
                {
                    return left;
                }
                if (right.NodeType == ExpressionType.Constant)
                {
                    return right;
                }
            }
            if (exp.NodeType == ExpressionType.Parameter)
            {
                return Expression.Constant(1d);
            }
            return null;
        }
    }
}
