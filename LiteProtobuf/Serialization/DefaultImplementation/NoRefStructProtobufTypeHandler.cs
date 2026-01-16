using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class NoRefStructProtobufTypeHandler<T>
    where T : struct
{
    public static bool TryCreateFulfilled<THandler, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T value,
        out ParseStatus status)
        where THandler : IProtobufTypeFactory<T>, IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
    }
}