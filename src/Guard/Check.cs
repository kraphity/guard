using System;
using System.Linq.Expressions;

namespace Kraphity.Guard
{
    public static partial class Check
    {
        public static void If(bool condition, Func<Exception> exceptionFactory)
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException(nameof(exceptionFactory));
            }

            if (!condition)
            {
                throw exceptionFactory();
            }
        }

        public static void NotNull<T>(T value, Expression<Func<T>> paramName, string message = null) where T : class
        {
            If(value != null, () => ExceptionHelper.BuildArgumentNullException(paramName, message));
        }               
    }
}