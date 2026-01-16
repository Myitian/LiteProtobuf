using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class ProtobufType
{
    public static bool TryCreateInstance<T>(
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value)
        where T : IReadOnlyProtobufType<T>, new(), allows ref struct
    {
        if (!T.IsFieldInfoValid(fieldInfo, options))
        {
            value = default;
            return false;
        }
        value = new();
        return true;
    }
    public static T CreateInstance<T>(
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : IReadOnlyProtobufType<T>, allows ref struct
    {
        if (!T.TryCreateInstance(fieldInfo, options, out T? value))
            throw new InvalidDataException($"Invalid data: {fieldInfo}");
        return value;
    }
    public static bool TryCreateFulfilled<T, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where T : IReadOnlyProtobufType<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>
    {
        if (!T.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
    }
    public static bool TryCreateFulfilled<T, TReader>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where T : IReadOnlyProtobufType<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!T.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(reader, fieldInfo, options, out status);
    }
    public static T CreateFulfilled<T, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : IReadOnlyProtobufType<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        T value = T.CreateInstance(fieldInfo, options);
        value.ReadProtobuf(ref reader, fieldInfo, options);
        return value;
    }
    public static T CreateFulfilled<T, TReader>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : IReadOnlyProtobufType<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        T value = T.CreateInstance(fieldInfo, options);
        value.ReadProtobuf(reader, fieldInfo, options);
        return value;
    }
}