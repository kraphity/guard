using System;
using System.Linq.Expressions;

namespace Kraphity.Guard
{
    public static partial class Check
    {
        public static void NotEmpty(string value, string paramName, string message = null)
        {
            If(value != string.Empty, () => ExceptionHelper.BuildArgumentEmptyException(paramName, message));
        }

        public static void NotEmpty(string value, Expression<Func<string>> paramName, string message = null)
        {
            NotEmpty(value, paramName.ToMemberPath(), message);
        }

        public static void NotWhitespace(string value, string paramName, string message = null)
        {
            If(value == null || !string.IsNullOrWhiteSpace(value), () => ExceptionHelper.BuildArgumentEmptyException(paramName, message));
        }

        public static void NotWhitespace(string value, Expression<Func<string>> paramName, string message = null)
        {
            NotWhitespace(value, paramName.ToMemberPath(), message);
        }

        public static void NotNullOrEmpty(string value, string paramName, string message = null)
        {
            NotNull(value, paramName, message);
            NotEmpty(value, paramName, message);
        }

        public static void NotNullOrEmpty(string value, Expression<Func<string>> paramName, string message = null)
        {
            NotNullOrEmpty(value, paramName.ToMemberPath(), message);
        }

        public static void NotNullOrWhitespace(string value, string paramName, string message = null)
        {
            NotNull(value, paramName, message);
            NotWhitespace(value, paramName, message);
        }

        public static void NotNullOrWhitespace(string value, Expression<Func<string>> paramName, string message = null)
        {
            NotNullOrWhitespace(value, paramName.ToMemberPath(), message);
        }
    }
}
