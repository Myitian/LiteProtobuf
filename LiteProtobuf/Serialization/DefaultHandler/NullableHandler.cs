using Myitian.LiteProtobuf.Serialization.DefaultImplementation;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultHandler;

public sealed class NullableHandler<T> : IStructProtobufTypeHandler<T?>
    where T : struct, ICreatableProtobufType<T>, IReadOnlyProtobufType, IWriteOnlyProtobufType
{
    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return T.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool IsFieldInfoValidForInstance(scoped in T? value, FieldInfo fieldInfo, SerializationOptions? options)
    {
        return value.HasValue ?
            value.Value.IsFieldInfoValidForInstance(fieldInfo, options) :
            T.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value)
    {
        return ProtobufTypeFactory.TryCreateInstance<T?, NullableHandler<T>>(fieldInfo, options, out value);
    }
    public static T? CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return ProtobufTypeFactory.CreateInstance<T?, NullableHandler<T>>(fieldInfo, options);
    }
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return NoRefStructProtobufTypeHandler.TryCreateFulfilled<TReader, T?, NullableHandler<T>>(ref reader, fieldInfo, options, out value, out status);
    }
    public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return StructProtobufTypeHandler.TryCreateFulfilled<TReader, T?, NullableHandler<T>>(reader, fieldInfo, options, out value, out status);
    }
    public static T? CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return StructProtobufTypeHandler.CreateFulfilled<TReader, T?, NullableHandler<T>>(ref reader, fieldInfo, options);
    }
    public static T? CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return StructProtobufTypeHandler.CreateFulfilled<TReader, T?, NullableHandler<T>>(reader, fieldInfo, options);
    }
    public static bool TryReadProtobuf<TReader>(scoped ref T? self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (self.HasValue)
            self.Value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
        else if (T.TryCreateFulfilled(ref reader, fieldInfo, options, out T value, out status))
            self = value;
        else
            return false;
        return true;
    }
    public static bool TryReadProtobuf<TReader>(scoped ref T? self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (self.HasValue)
            self.Value.TryReadProtobuf(reader, fieldInfo, options, out status);
        else if (T.TryCreateFulfilled(reader, fieldInfo, options, out T value, out status))
            self = value;
        else
            return false;
        return true;
    }
    public static void ReadProtobuf<TReader>(scoped ref T? self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (self.HasValue)
            self.Value.ReadProtobuf(ref reader, fieldInfo, options);
        else
            self = T.CreateFulfilled(ref reader, fieldInfo, options);
    }
    public static void ReadProtobuf<TReader>(scoped ref T? self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (self.HasValue)
            self.Value.ReadProtobuf(reader, fieldInfo, options);
        else
            self = T.CreateFulfilled(reader, fieldInfo, options);
    }
    public static void WriteProtobuf<TWriter>(in T? self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        self?.WriteProtobuf(ref writer, fieldInfo, options);
    }
    public static void WriteProtobuf<TWriter>(in T? self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        self?.WriteProtobuf(writer, fieldInfo, options);
    }
}
public sealed class NullableFactory<T> : IProtobufTypeFactory<T?>
    where T : struct, ICreatableProtobufType<T>, IReadOnlyProtobufType, IWriteOnlyProtobufType
{
    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return T.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value)
    {
        return ProtobufTypeFactory.TryCreateInstance<T?, NullableHandler<T>>(fieldInfo, options, out value);
    }
    public static T? CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return ProtobufTypeFactory.CreateInstance<T?, NullableHandler<T>>(fieldInfo, options);
    }
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return NoRefStructProtobufTypeHandler.TryCreateFulfilled<TReader, T?, NullableHandler<T>>(ref reader, fieldInfo, options, out value, out status);
    }
    public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return StructProtobufTypeHandler.TryCreateFulfilled<TReader, T?, NullableHandler<T>>(reader, fieldInfo, options, out value, out status);
    }
    public static T? CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return StructProtobufTypeHandler.CreateFulfilled<TReader, T?, NullableHandler<T>>(ref reader, fieldInfo, options);
    }
    public static T? CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return StructProtobufTypeHandler.CreateFulfilled<TReader, T?, NullableHandler<T>>(reader, fieldInfo, options);
    }
}
public sealed class NullableReadOnlyHandler<T> : IStructProtobufTypeReadOnlyHandler<T?>
    where T : struct, ICreatableProtobufType<T>, IReadOnlyProtobufType
{
    public static bool IsFieldInfoValidForInstance(scoped in T? value, FieldInfo fieldInfo, SerializationOptions? options)
    {
        return value.HasValue ?
            value.Value.IsFieldInfoValidForInstance(fieldInfo, options) :
            T.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool TryReadProtobuf<TReader>(scoped ref T? self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (self.HasValue)
            self.Value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
        else if (T.TryCreateFulfilled(ref reader, fieldInfo, options, out T value, out status))
            self = value;
        else
            return false;
        return true;
    }
    public static bool TryReadProtobuf<TReader>(scoped ref T? self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (self.HasValue)
            self.Value.TryReadProtobuf(reader, fieldInfo, options, out status);
        else if (T.TryCreateFulfilled(reader, fieldInfo, options, out T value, out status))
            self = value;
        else
            return false;
        return true;
    }
    public static void ReadProtobuf<TReader>(scoped ref T? self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (self.HasValue)
            self.Value.ReadProtobuf(ref reader, fieldInfo, options);
        else
            self = T.CreateFulfilled(ref reader, fieldInfo, options);
    }
    public static void ReadProtobuf<TReader>(scoped ref T? self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (self.HasValue)
            self.Value.ReadProtobuf(reader, fieldInfo, options);
        else
            self = T.CreateFulfilled(reader, fieldInfo, options);
    }
}
public sealed class NullableWriteOnlyHandler<T> : IStructProtobufTypeWriteOnlyHandler<T?>
    where T : struct, IWriteOnlyProtobufType
{
    public static void WriteProtobuf<TWriter>(in T? self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        self?.WriteProtobuf(ref writer, fieldInfo, options);
    }
    public static void WriteProtobuf<TWriter>(in T? self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        self?.WriteProtobuf(writer, fieldInfo, options);
    }
}