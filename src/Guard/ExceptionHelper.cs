using System;
using System.Linq.Expressions;

namespace Kraphity.Guard
{
    internal static class ExceptionHelper
    {
        internal static Exception BuildArgumentInvalidException(LambdaExpression paramName, string message)
        {
            return new ArgumentException(message ?? Resources.ArgumentInvalidMessage, paramName.ToMemberPath());
        }

        internal static Exception BuildArgumentEmptyException(LambdaExpression paramName, string message)
        {
            return new ArgumentException(message ?? Resources.ArgumentEmptyMessage, paramName.ToMemberPath());
        }

        internal static Exception BuildArgumentNullException(LambdaExpression paramName, string message)
        {
            var pn = paramName.ToMemberPath();
            return message == null ?
                new ArgumentNullException(pn) :
                new ArgumentNullException(pn, message);
        }

        internal static Exception BuildArgumentOutOfRangeException(LambdaExpression paramName, string message, object value)
        {
            return new ArgumentOutOfRangeException(paramName.ToMemberPath(), value, message ?? Resources.ArgumentOutOfRangeMessage);
        }
    }
}