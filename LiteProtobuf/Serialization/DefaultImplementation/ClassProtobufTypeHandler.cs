using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class ClassProtobufTypeHandler
{
    public static bool TryCreateFulfilled<T, THandler, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where T : class
        where THandler : IProtobufTypeFactory<T>, IReadOnlyClassProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(value, ref reader, fieldInfo, options, out status);
    }
    public static bool TryCreateFulfilled<T, THandler, TReader>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where T : class
        where THandler : IProtobufTypeFactory<T>, IReadOnlyClassProtobufTypeHandler<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(value, reader, fieldInfo, options, out status);
    }
    public static T CreateFulfilled<T, THandler, TReader>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : class
        where THandler : IProtobufTypeFactory<T>, IReadOnlyClassProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(value, ref reader, fieldInfo, options);
        return value;
    }
    public static T CreateFulfilled<T, THandler, TReader>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : class
        where THandler : IProtobufTypeFactory<T>, IReadOnlyClassProtobufTypeHandler<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(value, reader, fieldInfo, options);
        return value;
    }
    public static void ReadProtobuf<T, THandler, TReader>(
        T self,
        ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : class
        where THandler : IReadOnlyClassProtobufTypeHandler<T>, allows ref struct
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!THandler.TryReadProtobuf(self, ref reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
    public static void ReadProtobuf<T, THandler, TReader>(
        T self,
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : class
        where THandler : IReadOnlyClassProtobufTypeHandler<T>, allows ref struct
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!THandler.TryReadProtobuf(self, reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
}