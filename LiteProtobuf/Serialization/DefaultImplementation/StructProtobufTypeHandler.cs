using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class StructProtobufTypeHandler
{
    public static bool TryCreateInstance<T, THandler>(
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T value)
        where T : struct, allows ref struct
        where THandler : IProtobufTypeFactory<T>, allows ref struct
    {
        if (!THandler.IsFieldInfoValid(fieldInfo, options))
        {
            value = default;
            return false;
        }
        value = new();
        return true;
    }
    public static T CreateInstance<T, THandler>(
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : struct, allows ref struct
        where THandler : IProtobufTypeFactory<T>, allows ref struct
    {
        if (!THandler.IsFieldInfoValid(fieldInfo, options))
            throw new InvalidDataException($"Invalid data: {fieldInfo}");
        return new();
    }
    public static bool TryCreateFulfilled<T, THandler, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T value,
        out ParseStatus status)
        where T : struct, allows ref struct
        where THandler : IProtobufTypeFactory<T>, IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
    }
    public static bool TryCreateFulfilled<T, THandler, TReader>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T value,
        out ParseStatus status)
        where T : struct, allows ref struct
        where THandler : IProtobufTypeFactory<T>, IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(ref value, reader, fieldInfo, options, out status);
    }
    public static T CreateFulfilled<T, THandler, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : struct, allows ref struct
        where THandler : IProtobufTypeFactory<T>, IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(ref value, ref reader, fieldInfo, options);
        return value;
    }
    public static T CreateFulfilled<T, THandler, TReader>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : struct, allows ref struct
        where THandler : IProtobufTypeFactory<T>, IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(ref value, reader, fieldInfo, options);
        return value;
    }
    public static void ReadProtobuf<T, THandler, TReader>(
        scoped ref T self,
        ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : struct, allows ref struct
        where THandler : IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!THandler.TryReadProtobuf(ref self, ref reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
    public static void ReadProtobuf<T, THandler, TReader>(
        scoped ref T self,
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : struct, allows ref struct
        where THandler : IReadOnlyStructProtobufTypeHandler<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!THandler.TryReadProtobuf(ref self, reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
}