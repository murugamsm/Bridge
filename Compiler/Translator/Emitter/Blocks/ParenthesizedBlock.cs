﻿using Bridge.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;

namespace Bridge.Translator
{
    public class ParenthesizedBlock : ConversionBlock
    {
        public ParenthesizedBlock(IEmitter emitter, ParenthesizedExpression parenthesizedExpression)
            : base(emitter, parenthesizedExpression)
        {
            this.Emitter = emitter;
            this.ParenthesizedExpression = parenthesizedExpression;
        }

        public ParenthesizedExpression ParenthesizedExpression
        {
            get;
            set;
        }

        protected override Expression GetExpression()
        {
            return this.ParenthesizedExpression;
        }

        protected override void EmitConversionExpression()
        {
            var ignoreParentheses = this.IgnoreParentheses(this.ParenthesizedExpression.Expression);

            if (!ignoreParentheses)
            {
                this.WriteOpenParentheses();
            }

            this.ParenthesizedExpression.Expression.AcceptVisitor(this.Emitter);

            if (!ignoreParentheses)
            {
                this.WriteCloseParentheses();
            }
        }

        protected bool IgnoreParentheses(Expression expression)
        {
            if (expression is CastExpression)
            {
                var rr = this.Emitter.Resolver.ResolveNode(expression, this.Emitter);
                if (rr is ConstantResolveResult)
                {
                    return false;
                }
                /*var simpleType = ((CastExpression)expression).Type as SimpleType;

                if (simpleType != null && simpleType.Identifier == "dynamic")
                {
                    return true;
                }*/
                return true;
            }
            return false;
        }
    }
}