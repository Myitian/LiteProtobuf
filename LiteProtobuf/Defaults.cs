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
    public static class StructReadOnlyProtobufType<T>
        where T : struct, IReadOnlyProtobufType<T>, allows ref struct
    {
        public static bool TryCreateInstance(
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value)
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
        public static T CreateInstance(
            FieldInfo fieldInfo,
            SerializationOptions? options)
        {
            if (!T.TryCreateInstance(fieldInfo, options, out T? value))
                throw new InvalidDataException($"Invalid data: {fieldInfo}");
            return value;
        }
    }
    public static class AllowsRefStructReadOnlyProtobufType<T>
        where T : IReadOnlyProtobufType<T>, allows ref struct
    {
        public static bool TryCreateFulfilled<TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>
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
    public static class NewProtobufTypeFactory<T>
        where T : new()
    {
        public static bool TryCreateInstance<THandler>(
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value)
            where THandler : IProtobufTypeFactory<T>
        {
            if (!THandler.IsFieldInfoValid(fieldInfo, options))
            {
                value = default;
                return false;
            }
            value = new();
            return true;
        }
        public static T CreateInstance<THandler>(
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IProtobufTypeFactory<T>
        {
            if (!THandler.IsFieldInfoValid(fieldInfo, options))
                throw new InvalidDataException($"Invalid data: {fieldInfo}");
            return new();
        }
    }
    public static class StructProtobufTypeFactory<T>
        where T : struct, allows ref struct
    {
        public static bool TryCreateInstance<THandler>(
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value)
            where THandler : IProtobufTypeFactory<T>
        {
            if (!THandler.IsFieldInfoValid(fieldInfo, options))
            {
                value = default;
                return false;
            }
            value = new();
            return true;
        }
        public static T CreateInstance<THandler>(
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IProtobufTypeFactory<T>
        {
            if (!THandler.IsFieldInfoValid(fieldInfo, options))
                throw new InvalidDataException($"Invalid data: {fieldInfo}");
            return new();
        }
    }
    public static class ReadOnlyStructProtobufTypeHandler<T>
        where T : struct
    {
        public static bool TryCreateFulfilled<THandler, TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value,
            out ParseStatus status)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
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
    public static class AllowsRefStructReadOnlyStructProtobufTypeHandler<T>
        where T : struct, allows ref struct
    {
        public static bool TryCreateFulfilled<THandler, TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value,
            out ParseStatus status)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>
        {
            if (!THandler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return THandler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
        }
        public static bool TryCreateFulfilled<THandler, TReader>(
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T value,
            out ParseStatus status)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!THandler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return THandler.TryReadProtobuf(ref value, reader, fieldInfo, options, out status);
        }
        public static T CreateFulfilled<THandler, TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            T value = THandler.CreateInstance(fieldInfo, options);
            THandler.ReadProtobuf(ref value, ref reader, fieldInfo, options);
            return value;
        }
        public static T CreateFulfilled<THandler, TReader>(
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            T value = THandler.CreateInstance(fieldInfo, options);
            THandler.ReadProtobuf(ref value, reader, fieldInfo, options);
            return value;
        }
        public static void ReadProtobuf<THandler, TReader>(
            scoped ref T self,
            ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!THandler.TryReadProtobuf(ref self, ref reader, fieldInfo, options, out ParseStatus status))
                throw IBinaryReader.GetExceptionByStatus(status);
        }
        public static void ReadProtobuf<THandler, TReader>(
            scoped ref T self,
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyStructProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!THandler.TryReadProtobuf(ref self, reader, fieldInfo, options, out ParseStatus status))
                throw IBinaryReader.GetExceptionByStatus(status);
        }
    }
    public static class ReadOnlyClassProtobufTypeHandler<T>
        where T : class
    {
        public static bool TryCreateFulfilled<THandler, TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!THandler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return THandler.TryReadProtobuf(value, ref reader, fieldInfo, options, out status);
        }
        public static bool TryCreateFulfilled<THandler, TReader>(
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options,
            [NotNullWhen(true)] out T? value,
            out ParseStatus status)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!THandler.TryCreateInstance(fieldInfo, options, out value))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
            return THandler.TryReadProtobuf(value, reader, fieldInfo, options, out status);
        }
        public static T CreateFulfilled<THandler, TReader>(
            scoped ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            T value = THandler.CreateInstance(fieldInfo, options);
            THandler.ReadProtobuf(value, ref reader, fieldInfo, options);
            return value;
        }
        public static T CreateFulfilled<THandler, TReader>(
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            T value = THandler.CreateInstance(fieldInfo, options);
            THandler.ReadProtobuf(value, reader, fieldInfo, options);
            return value;
        }
        public static void ReadProtobuf<THandler, TReader>(
            T self,
            ref TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!THandler.TryReadProtobuf(self, ref reader, fieldInfo, options, out ParseStatus status))
                throw IBinaryReader.GetExceptionByStatus(status);
        }
        public static void ReadProtobuf<THandler, TReader>(
            T self,
            TReader reader,
            FieldInfo fieldInfo,
            SerializationOptions? options)
            where THandler : IReadOnlyClassProtobufTypeHandler<T>
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!THandler.TryReadProtobuf(self, reader, fieldInfo, options, out ParseStatus status))
                throw IBinaryReader.GetExceptionByStatus(status);
        }
    }
}