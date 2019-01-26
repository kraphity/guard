using System;

namespace Kraphity.Guard
{
    internal static class ExceptionHelper
    {
        internal static Exception BuildArgumentEmptyException(string paramName, string message)
        {
            return new ArgumentException(message ?? Resources.ArgumentEmptyMessage, paramName);
        }

        internal static Exception BuildArgumentNullException(string paramName, string message)
        {
            return message == null ?
                new ArgumentNullException(paramName) :
                new ArgumentNullException(paramName, message);
        }

        internal static Exception BuildArgumentOutOfRangeException(string paramName, string message, object value)
        {
            return new ArgumentOutOfRangeException(paramName, value, message ?? Resources.ArgumentOutOfRangeMessage);
        }
    }
}