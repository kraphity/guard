using System.Linq.Expressions;
using System.Text;

namespace Kraphity.Guard
{
    internal static class ExpressionExtensions
    {
        private const string MemberPathSeparator = ".";

        internal static string ToMemberPath(this LambdaExpression expression)
        {
            var sb = new StringBuilder();
            var memberExpression = GetMemberExpression(expression);

            while (memberExpression != null)
            {
                if (sb.Length > 0) sb.Insert(0, MemberPathSeparator);
                sb.Insert(0, memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return sb.ToString();
        }
        
        private static MemberExpression GetMemberExpression(LambdaExpression expression)
        {
            MemberExpression m;

            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var u = expression.Body as UnaryExpression;
                    m = u?.Operand as MemberExpression;
                    break;
                default:
                    m = expression.Body as MemberExpression;
                    break;
            }

            return m;
        }
    }
}
