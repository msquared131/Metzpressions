using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Metzpressions;
using System.Linq.Expressions;

namespace MetzpressionsTest
{
    [TestClass]
    public class ExpressionTest
    {
        [TestMethod]
        public void Addition()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input + 3;
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void Subtraction()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input - 3;
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void Mulitplication()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input * 3;
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void Division()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input / 3;
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void Power()
        {
            var parameter = Expression.Parameter(typeof(double), "x");            
            var exp = Expression.MakeBinary(ExpressionType.Power, parameter, Expression.Constant(2d));
            var lamda = Expression.Lambda(exp, parameter);
            TestDoubleExpression((Expression<Func<double, double>>)lamda);            
        }

        [TestMethod]
        public void AddLikeTerms()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input + input * 3;
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void SubtractLikeTerms()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input - input * 3;
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void DivideLikeTerms()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input / (input * 3);
            TestDecimalExpression(exp);
        }

        [TestMethod]
        public void MultiplyLikeTerms()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input * (input * 3);
            TestDecimalExpression(exp);
        }             

        private static void TestDecimalExpression(Expression<Func<decimal, decimal>> exp)
        {
            decimal initialInput = new Random().Next(1, 10);
            decimal initialOutput = exp.Compile()(initialInput);
            var reversal = ModifyExpressions.ReverseLambda(exp);
            
            if (Decimal.Round(((Expression<Func<decimal, decimal>>)reversal).Compile()(initialOutput), 4) == Decimal.Round(initialInput, 4))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false, $"Expression {exp} failed");
            }
        }

        private static void TestDoubleExpression(Expression<Func<double, double>> exp)
        {
            double initialInput = new Random().Next(1, 10);
            double initialOutput = exp.Compile()(initialInput);
            var reveral = ModifyExpressions.ReverseLambda(exp);
            if (Math.Round(((Expression<Func<double, double>>)reveral).Compile()(initialOutput), 4) == Math.Round(initialInput, 4))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false, $"Expression {exp} failed");
            }
        }
    }
}
