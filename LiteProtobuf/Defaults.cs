using Myitian.LiteProtobuf.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf;

public static class Defaults
{
    public static class NewReadOnlyProtobufType<T>
        where T : IReadOnlyProtobufType<T>, new()
    {
        public static bool TryCreateInstance(
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value)
        {
            if (!T.IsFieldInfoValid(fieldInfo, options))
            {
                value = default;
                return false;
            }
            value = new();
            return true;
        }
    }
    public static class ReadOnlyProtobufType<T>
        where T : IReadOnlyProtobufType<T>
    {
        public static T CreateInstance(
            FieldInfo fieldInfo,
            SerializationOptions? options)
        {
            if (!T.TryCreateInstance(fieldInfo, options, out T? value))
                throw IBinaryReader.GetExceptionByStatus(ParseStatus.InvalidData);
            return value;
        }
        public static bool TryCreateFulfilled<TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!T.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
        }
        public static bool TryCreateFulfilled<TReader>(
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!T.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return value.TryReadProtobuf(reader, fieldInfo, options, out status);
        }
        public static T CreateFulfilled<TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            T value = T.CreateInstance(fieldInfo, options);
            value.ReadProtobuf(ref reader, fieldInfo, options);
            return value;
        }
        public static T CreateFulfilled<TReader>(
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where TReader : class, IClassBinaryReader<TReader>
        {
            T value = T.CreateInstance(fieldInfo, options);
            value.ReadProtobuf(reader, fieldInfo, options);
            return value;
        }
    }
    public static class ReadOnlyStructProtobufTypeHandler<T>
        where T : struct
    {
        public static bool TryCreateFulfilled<THandler, TReader>(
            in THandler handler,
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value,
            out ParseStatus status)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!handler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return handler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
        }
        public static bool TryCreateFulfilled<THandler, TReader>(
            in THandler handler,
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value,
            out ParseStatus status)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!handler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return handler.TryReadProtobuf(ref value, reader, fieldInfo, options, out status);
        }
    }
    public static class ReadOnlyClassProtobufTypeHandler<T>
        where T : class
    {
        public static bool TryCreateFulfilled<THandler, TReader>(
            in THandler handler,
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!handler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return handler.TryReadProtobuf(value, ref reader, fieldInfo, options, out status);
        }
        public static bool TryCreateFulfilled<THandler, TReader>(
            in THandler handler,
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!handler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return handler.TryReadProtobuf(value, reader, fieldInfo, options, out status);
        }
        public static void ReadProtobuf<THandler, TReader>(
            in THandler handler,
            T self,
            ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!handler.TryReadProtobuf(self, ref reader, fieldInfo, options, out ParseStatus status))
                throw IBinaryReader.GetExceptionByStatus(status);
        }
        public static void ReadProtobuf<THandler, TReader>(
            in THandler handler,
            T self,
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!handler.TryReadProtobuf(self, reader, fieldInfo, options, out ParseStatus status))
                throw IBinaryReader.GetExceptionByStatus(status);
        }
    }
}