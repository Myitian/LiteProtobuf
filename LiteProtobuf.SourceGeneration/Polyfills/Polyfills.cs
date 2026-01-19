// .NET Foundation, MIT license.

#pragma warning disable IDE0130

using Myitian.LiteProtobuf.SourceGeneration;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
    {
        public string ParameterName { get; } = parameterName;
    }
}
namespace System.Diagnostics
{
    internal sealed class UnreachableException : Exception
    {
        public UnreachableException()
            : base(SR.Arg_UnreachableException)
        {
        }
        public UnreachableException(string? message)
            : base(message ?? SR.Arg_UnreachableException)
        {
        }
        public UnreachableException(string? message, Exception? innerException)
            : base(message ?? SR.Arg_UnreachableException, innerException)
        {
        }
    }
}
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute;
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute;
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute;
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute;
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute
    {
        public bool ReturnValue { get; } = returnValue;
    }
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute
    {
        public bool ReturnValue { get; } = returnValue;
    }
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute
    {
        public string ParameterName { get; } = parameterName;
    }
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute;
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute(bool parameterValue) : Attribute
    {
        public bool ParameterValue { get; } = parameterValue;
    }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed class MemberNotNullAttribute(params string[] members) : Attribute
    {
        public string[] Members { get; } = members;
    }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed class MemberNotNullWhenAttribute(bool returnValue, params string[] members) : Attribute
    {
        public bool ReturnValue { get; } = returnValue;
        public string[] Members { get; } = members;
    }
}
namespace System
{
    internal static class ExceptionPolyfills
    {
        extension(ArgumentNullException)
        {
            public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (argument is null)
                    ThrowArgumentNullException(paramName);
            }
        }

        [DoesNotReturn]
        private static void ThrowArgumentNullException(string? paramName)
            => throw new ArgumentNullException(paramName);

        extension(ObjectDisposedException)
        {
            public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance)
            {
                if (condition)
                    ThrowObjectDisposedException(instance);
            }

            public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
            {
                if (condition)
                    ThrowObjectDisposedException(type);
            }
        }

        [DoesNotReturn]
        private static void ThrowObjectDisposedException(object? instance)
            => throw new ObjectDisposedException(instance?.GetType().FullName);

        [DoesNotReturn]
        private static void ThrowObjectDisposedException(Type? type)
            => throw new ObjectDisposedException(type?.FullName);
    }
}