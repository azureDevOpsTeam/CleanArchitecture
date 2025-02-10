#region Usings

using System.Linq.Expressions;

#endregion

namespace InfrastructureLayer.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> Combine<T>(this Expression<Func<T, bool>> expressionOne, Expression<Func<T, bool>> expressionTwo)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expressionOne.Parameters[0], parameter);
            var left = leftVisitor.Visit(expressionOne.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expressionTwo.Parameters[0], parameter);
            var right = rightVisitor.Visit(expressionTwo.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left ?? throw new InvalidOperationException(nameof(left)), right ?? throw new InvalidOperationException(nameof(right))), parameter);
        }
    }
}