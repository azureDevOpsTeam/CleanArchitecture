#region Usings

using System.Linq.Expressions;

#endregion

namespace InfrastructureLayer.Extensions
{
    internal class ReplaceExpressionVisitor : ExpressionVisitor
    {
        #region Fields

        private readonly Expression _newValue;
        private readonly Expression _oldValue;

        #endregion Fields

        #region Methods

        #region Constructors

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        #endregion Constructors

        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }

        #endregion Methods
    }
}