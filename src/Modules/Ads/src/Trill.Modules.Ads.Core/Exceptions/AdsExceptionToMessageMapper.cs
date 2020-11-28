using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;

namespace Trill.Modules.Ads.Core.Exceptions
{
    internal class AdsExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public IEnumerable<Type> ExceptionTypes { get; } = Enumerable.Empty<Type>();

        public IActionRejected Map<T>(T exception) where T : Exception => null;
    }
}