using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufTypeStaticHandler<T> where T : allows ref struct
{
    public static abstract bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out T? value);
    public static abstract bool TryCreateFulfilled<TReader>(scoped ref TReader reader, WireType wireType, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract bool TryCreateFulfilled<TReader>(TReader reader, WireType wireType, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
}