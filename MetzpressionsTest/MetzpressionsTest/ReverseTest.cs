﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Metzpressions;
using System.Linq.Expressions;

namespace MetzpressionsTest
{
    [TestClass]
    public class ReverseTest
    {
        [TestMethod]
        public void LikeTerms()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input + input * 3;            
            TestExpression(exp);
        }
        
        [TestMethod]
        public void Addition()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input + 3;
            TestExpression(exp);
        }

        [TestMethod]
        public void Subtraction()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input - 3;
            TestExpression(exp);
        }

        [TestMethod]
        public void Mulitplication()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input * 3;
            TestExpression(exp);
        }

        [TestMethod]
        public void Division()
        {
            Expression<Func<decimal, decimal>> exp = (input) => input / 3;
            TestExpression(exp);
        }

        private static void TestExpression(Expression<Func<decimal, decimal>> exp)
        {
            decimal initialInput = new Random().Next(1, 10);
            decimal initialOutput = exp.Compile()(initialInput);
            var reveral = ModifyExpressions.ReverseLambda(exp);
            if (Decimal.Round(((Expression<Func<decimal, decimal>>)reveral).Compile()(initialOutput), 4) == Decimal.Round(initialInput, 4))
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
