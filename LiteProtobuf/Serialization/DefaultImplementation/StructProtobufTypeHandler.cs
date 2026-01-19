using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class StructProtobufTypeHandler
{
    public static bool TryCreateFulfilled<TReader, T, THandler>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>
        where T : allows ref struct
        where THandler : IProtobufTypeFactory<T>, IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
    }
    public static bool TryCreateFulfilled<TReader, T, THandler>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value,
        out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
        where T : allows ref struct
        where THandler : IProtobufTypeFactory<T>, IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryCreateInstance(fieldInfo, options, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return THandler.TryReadProtobuf(ref value, reader, fieldInfo, options, out status);
    }
    public static T CreateFulfilled<TReader, T, THandler>(
        scoped ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : allows ref struct
        where THandler : IProtobufTypeFactory<T>, IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(ref value, ref reader, fieldInfo, options);
        return value;
    }
    public static T CreateFulfilled<TReader, T, THandler>(
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
        where T : allows ref struct
        where THandler : IProtobufTypeFactory<T>, IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        T value = THandler.CreateInstance(fieldInfo, options);
        THandler.ReadProtobuf(ref value, reader, fieldInfo, options);
        return value;
    }
    public static void ReadProtobuf<TReader, T, THandler>(
        scoped ref T self,
        ref TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        where T : allows ref struct
        where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryReadProtobuf(ref self, ref reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
    public static void ReadProtobuf<TReader, T, THandler>(
        scoped ref T self,
        TReader reader,
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
        where T : allows ref struct
        where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
    {
        if (!THandler.TryReadProtobuf(ref self, reader, fieldInfo, options, out ParseStatus status))
            throw IBinaryReader.GetExceptionByStatus(status);
    }
}