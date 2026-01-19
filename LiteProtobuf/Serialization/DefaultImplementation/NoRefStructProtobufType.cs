using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class NoRefStructProtobufType
{
    public static bool TryCreateFulfilled<TReader, T>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : ICreatableProtobufType<T>, IReadOnlyProtobufType
    {
        if (!T.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
    }
}