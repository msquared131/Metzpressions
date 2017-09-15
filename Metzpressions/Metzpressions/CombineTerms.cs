using System.Linq.Expressions;

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
                switch (b.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant((decimal)GetConstant(b.Left).Value + (decimal)GetConstant(b.Right).Value), GetParameter(b.Left));
                    case ExpressionType.Subtract:
                        return Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant((decimal)GetConstant(b.Left).Value - (decimal)GetConstant(b.Right).Value), GetParameter(b.Left));
                    case ExpressionType.Multiply:
                        var left = Expression.Constant((decimal)GetConstant(b.Left).Value * (decimal)GetConstant(b.Right).Value);
                        var right = Expression.MakeBinary(ExpressionType.Power, GetParameter(b.Left), GetParameter(b.Right));
                        return Expression.MakeBinary(ExpressionType.Multiply, left, right);
                    case ExpressionType.Divide:
                        return Expression.Constant((decimal)GetConstant(b.Left).Value / (decimal)GetConstant(b.Right).Value);
                }
            }
            return b;
        }

        private bool CanCombine(Expression left, Expression right)
        {
            return (GetParameter(left) == GetParameter(right));
        }

        private ParameterExpression GetParameter(Expression exp)
        {
            if(exp.NodeType == ExpressionType.Multiply)
            {
                var binary = (BinaryExpression)exp;
                if(binary.Left.NodeType == ExpressionType.Parameter)
                {
                    return (ParameterExpression)binary.Left;
                }
                if (binary.Right.NodeType == ExpressionType.Parameter)
                {
                    return (ParameterExpression)binary.Right;
                }
            }
            if(exp.NodeType == ExpressionType.Parameter)
            {
                return (ParameterExpression)exp;
            }
            return null;
        }        

        private ConstantExpression GetConstant(Expression exp)
        {
            if(exp.NodeType == ExpressionType.Multiply)
            {
                var binary = (BinaryExpression)exp;
                if (binary.Left.NodeType == ExpressionType.Constant)
                {
                    return (ConstantExpression)binary.Left;
                }
                if (binary.Right.NodeType == ExpressionType.Constant)
                {
                    return (ConstantExpression)binary.Right;
                }
            }
            if (exp.NodeType == ExpressionType.Parameter)
            {
                return Expression.Constant(1m);
            }
            return null;
        }        
    }
}
