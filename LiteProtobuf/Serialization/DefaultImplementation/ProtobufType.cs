using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class ProtobufType
{
    public static bool TryCreateInstance<T>(
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value)
        where T : ICreatableProtobufType<T>, new(), allows ref struct
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
        where T : ICreatableProtobufType<T>, allows ref struct
    {
        if (!T.TryCreateInstance(fieldInfo, options, out T? value))
            throw new InvalidDataException($"Invalid data: {fieldInfo}");
        return value;
    }
    public static bool TryCreateFulfilled<TReader, T>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>
        where T : ICreatableProtobufType<T>, IReadOnlyProtobufType, allows ref struct
    {
        if (!T.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
    }
    public static bool TryCreateFulfilled<TReader, T>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
        where T : ICreatableProtobufType<T>, IReadOnlyProtobufType, allows ref struct
    {
        if (!T.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(reader, fieldInfo, options, out status);
    }
    public static T CreateFulfilled<TReader, T>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : ICreatableProtobufType<T>, IReadOnlyProtobufType, allows ref struct
    {
        T value = T.CreateInstance(fieldInfo, options);
        value.ReadProtobuf(ref reader, fieldInfo, options);
        return value;
    }
    public static T CreateFulfilled<TReader, T>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
        where T : ICreatableProtobufType<T>, IReadOnlyProtobufType, allows ref struct
    {
        T value = T.CreateInstance(fieldInfo, options);
        value.ReadProtobuf(reader, fieldInfo, options);
        return value;
    }
}