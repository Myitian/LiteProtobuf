using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class ClassProtobufTypeHandler
{
    public static bool TryCreateFulfilled<TReader, T, THandler>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : class
        where THandler : IProtobufTypeFactory<T>, IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(value, ref reader, fieldInfo, options, out status);
    }
    public static bool TryCreateFulfilled<TReader, T, THandler>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
        where T : class
        where THandler : IProtobufTypeFactory<T>, IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(value, reader, fieldInfo, options, out status);
    }
    public static T CreateFulfilled<TReader, T, THandler>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : class
        where THandler : IProtobufTypeFactory<T>, IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(value, ref reader, fieldInfo, options);
        return value;
    }
    public static T CreateFulfilled<TReader, T, THandler>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
        where T : class
        where THandler : IProtobufTypeFactory<T>, IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(value, reader, fieldInfo, options);
        return value;
    }
    public static void ReadProtobuf<TReader, T, THandler>(
        T self,
        ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : class
        where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryReadProtobuf(self, ref reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
    public static void ReadProtobuf<TReader, T, THandler>(
        T self,
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
        where T : class
        where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryReadProtobuf(self, reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
}