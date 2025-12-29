using Myitian.LiteProtobuf.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf;

public static class Defaults<T> where T : IProtobufType<T>
{
    public static bool TryCreateFulfilled<TReader>(
        scoped ref TReader reader,
        WireType wireType,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!T.TryCreateInstance(wireType, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, wireType, out status);
    }
    public static bool TryCreateFulfilled<TReader>(
        TReader reader,
        WireType wireType,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!T.TryCreateInstance(wireType, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(reader, wireType, out status);
    }
}