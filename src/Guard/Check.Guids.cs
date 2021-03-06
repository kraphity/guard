﻿using System;
using System.Linq.Expressions;

namespace Kraphity.Guard
{
    public static partial class Check
    {
        public static void NotEmpty(Guid value, string paramName, string message = null)
        {
            If(value != Guid.Empty, () => ExceptionHelper.BuildArgumentEmptyException(paramName, message));
        }

        public static void NotEmpty(Guid value, Expression<Func<Guid>> paramName, string message = null)
        {
            NotEmpty(value, paramName.ToMemberPath(), message);
        }
    }
}
