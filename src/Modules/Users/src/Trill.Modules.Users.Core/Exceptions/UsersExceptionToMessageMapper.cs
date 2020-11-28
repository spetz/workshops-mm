using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;

namespace Trill.Modules.Users.Core.Exceptions
{
    internal class UsersExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public IEnumerable<Type> ExceptionTypes { get; } = Enumerable.Empty<Type>();

        public IActionRejected Map<T>(T exception) where T : Exception => null;
    }
}