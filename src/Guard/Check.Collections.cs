using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kraphity.Guard
{
    public static partial class Check
    {
        public static void NotEmpty(IEnumerable value, string paramName, string message = null)
        {
            NotNull(value, paramName, message);

            If(value.Count() > 0, () => ExceptionHelper.BuildArgumentEmptyException(paramName, message));
        }

        public static void NotEmpty<TCollection, TElement>(TCollection value, string paramName, string message = null) where TCollection : class, IEnumerable<TElement>
        {
            NotNull(value, paramName, message);

            If(value.Any(), () => ExceptionHelper.BuildArgumentEmptyException(paramName, message));
        }

        public static void NotEmpty(IEnumerable value, Expression<Func<IEnumerable>> paramName, string message = null)
        {
            NotEmpty(value, paramName.ToMemberPath(), message);
        }

        public static void NotEmpty<TCollection, TElement>(TCollection value, Expression<Func<TCollection>> paramName, string message = null) where TCollection : class, IEnumerable<TElement>
        {
            NotEmpty<TCollection, TElement>(value, paramName.ToMemberPath(), message);
        }
    }
}
