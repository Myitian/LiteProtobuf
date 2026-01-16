using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class NoRefStructProtobufType
{
    public static bool TryCreateFulfilled<T, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where T : IReadOnlyProtobufType<T>
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!T.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
    }
}