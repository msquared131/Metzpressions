using System.Linq.Expressions;

namespace Metzpressions
{
    internal class Reverse : ExpressionVisitor
    {
        protected Expression LeftSide;
        protected Expression RightSide;
        protected Expression ExpressionNotEvaluating;
        protected Expression ExpressionEvaluated;

        public LambdaExpression Modify(LambdaExpression exp)
        {
            LeftSide = exp.Parameters[0];
            RightSide = exp.Body;
            var parameters = exp.Parameters;
            do
            {
                LeftSide = ModifyExpressions.CombineTerms(LeftSide);
                RightSide = ModifyExpressions.CombineTerms(RightSide);
                if (LeftSide.NodeType == ExpressionType.Constant ||
                    LeftSide.NodeType == ExpressionType.Parameter)
                {
                    ExpressionNotEvaluating = LeftSide;
                    RightSide = Visit(RightSide);
                    LeftSide = ExpressionNotEvaluating;
                    ExpressionEvaluated = RightSide;
                }
                else
                {
                    ExpressionNotEvaluating = RightSide;
                    LeftSide = Visit(LeftSide);
                    RightSide = ExpressionNotEvaluating;
                    ExpressionEvaluated = LeftSide;
                }
            }
            while (ExpressionEvaluated.NodeType != ExpressionType.Parameter &&
                   !ExpressionEvaluated.Equals(parameters[0]));

            return Expression.Lambda(ExpressionNotEvaluating, parameters);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {

            ExpressionType newType;
            Expression expressionToMove;
            Expression expressionToStay;
            switch (b.NodeType)
            {
                case ExpressionType.Add:
                    newType = ExpressionType.Subtract;
                    break;
                case ExpressionType.Subtract:
                    newType = ExpressionType.Add;
                    break;
                case ExpressionType.Multiply:
                    newType = ExpressionType.Divide;
                    break;
                case ExpressionType.Divide:
                    newType = ExpressionType.Multiply;
                    break;
                default:
                    newType = b.NodeType;
                    break;
            }

            if ((b.Left.NodeType == ExpressionType.Constant ||
                 (b.Left.NodeType == ExpressionType.Parameter &&
                  b.Right.NodeType != ExpressionType.Constant)) &&
                b.NodeType != ExpressionType.Divide)
            {
                expressionToMove = b.Left;
                expressionToStay = b.Right;
            }
            else
            {
                expressionToMove = b.Right;
                expressionToStay = b.Left;
            }
            ExpressionNotEvaluating = Expression.MakeBinary(newType, ExpressionNotEvaluating, expressionToMove);
            return expressionToStay;
        }
    }
}

