using System;
using System.Linq.Expressions;

namespace Kraphity.Guard
{
    public static partial class Check
    {
        public static void InRange<T>(bool condition, string paramName, T value, string message = null)
        {
            if (paramName == null)
            {
                throw new ArgumentNullException(nameof(paramName));
            }

            If(condition, () => ExceptionHelper.BuildArgumentOutOfRangeException(paramName, message, value));
        }

        public static void InRange<T>(bool condition, Expression<Func<T>> paramName, T value, string message = null)
        {
            InRange<T>(condition, paramName.ToMemberPath(), value, message);
        }
    }
}
