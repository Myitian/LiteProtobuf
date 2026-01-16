using Myitian.LiteProtobuf.Serialization.DefaultImplementation;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultHandler;

public sealed class NonPackedClassCollectionHandler<T, TCollection, THandler>
    : IClassProtobufTypeHandler<TCollection>
    where TCollection : class, ICollection<T>, new()
    where THandler : IClassProtobufTypeHandler<T>
    where T : class
{
    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return THandler.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool IsFieldInfoValidForInstance(in TCollection value, FieldInfo fieldInfo, SerializationOptions? options)
    {
        return THandler.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value)
    {
        return ProtobufTypeFactory.TryCreateInstance<TCollection, NonPackedClassCollectionHandler<T, TCollection, THandler>>(fieldInfo, options, out value);
    }
    public static TCollection CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return ProtobufTypeFactory.CreateInstance<TCollection, NonPackedClassCollectionHandler<T, TCollection, THandler>>(fieldInfo, options);
    }
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return ClassProtobufTypeHandler.TryCreateFulfilled<TCollection, NonPackedClassCollectionHandler<T, TCollection, THandler>, TReader>(ref reader, fieldInfo, options, out value, out status);
    }
    public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value, out ParseStatus status)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return ClassProtobufTypeHandler.TryCreateFulfilled<TCollection, NonPackedClassCollectionHandler<T, TCollection, THandler>, TReader>(reader, fieldInfo, options, out value, out status);
    }
    public static TCollection CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return ClassProtobufTypeHandler.CreateFulfilled<TCollection, NonPackedClassCollectionHandler<T, TCollection, THandler>, TReader>(ref reader, fieldInfo, options);
    }
    public static TCollection CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return ClassProtobufTypeHandler.CreateFulfilled<TCollection, NonPackedClassCollectionHandler<T, TCollection, THandler>, TReader>(reader, fieldInfo, options);
    }
    public static bool TryReadProtobuf<TReader>(TCollection self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!THandler.TryCreateFulfilled(ref reader, fieldInfo, options, out T? value, out status))
            return false;
        self.Add(value);
        return true;
    }
    public static bool TryReadProtobuf<TReader>(TCollection self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!THandler.TryCreateFulfilled(reader, fieldInfo, options, out T? value, out status))
            return false;
        self.Add(value);
        return true;
    }
    public static void ReadProtobuf<TReader>(TCollection self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        T value = THandler.CreateFulfilled(ref reader, fieldInfo, options);
        self.Add(value);
    }
    public static void ReadProtobuf<TReader>(TCollection self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        T value = THandler.CreateFulfilled(reader, fieldInfo, options);
        self.Add(value);
    }
    public static void WriteProtobuf<TWriter>(TCollection self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        foreach (T value in self)
            THandler.WriteProtobuf(value, ref writer, fieldInfo, options);
    }
    public static void WriteProtobuf<TWriter>(TCollection self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        foreach (T value in self)
            THandler.WriteProtobuf(value, writer, fieldInfo, options);
    }
}
public sealed class NonPackedClassCollectionReadOnlyHandler<T, THandler>
    : IReadOnlyClassProtobufTypeHandler<ICollection<T>>
    where THandler : IProtobufTypeFactory<T>
    where T : class
{
    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return THandler.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool IsFieldInfoValidForInstance(in ICollection<T> value, FieldInfo fieldInfo, SerializationOptions? options)
    {
        return THandler.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool TryReadProtobuf<TReader>(ICollection<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!THandler.TryCreateFulfilled(ref reader, fieldInfo, options, out T? value, out status))
            return false;
        self.Add(value);
        return true;
    }
    public static bool TryReadProtobuf<TReader>(ICollection<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!THandler.TryCreateFulfilled(reader, fieldInfo, options, out T? value, out status))
            return false;
        self.Add(value);
        return true;
    }
    public static void ReadProtobuf<TReader>(ICollection<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        T value = THandler.CreateFulfilled(ref reader, fieldInfo, options);
        self.Add(value);
    }
    public static void ReadProtobuf<TReader>(ICollection<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        T value = THandler.CreateFulfilled(reader, fieldInfo, options);
        self.Add(value);
    }
}
public sealed class NonPackedClassEnumerableWriteOnlyHandler<T, THandler>
    : IWriteOnlyClassProtobufTypeHandler<IEnumerable<T>>
    where THandler : IWriteOnlyClassProtobufTypeHandler<T>
    where T : class
{
    public static void WriteProtobuf<TWriter>(IEnumerable<T> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        foreach (T value in self)
            THandler.WriteProtobuf(value, ref writer, fieldInfo, options);
    }
    public static void WriteProtobuf<TWriter>(IEnumerable<T> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        foreach (T value in self)
            THandler.WriteProtobuf(value, writer, fieldInfo, options);
    }
}
public sealed class NonPackedClassReadOnlySpanWriteOnlyHandler<T, THandler>
    : IWriteOnlyStructProtobufTypeHandler<ReadOnlySpan<T>>
    where THandler : IWriteOnlyClassProtobufTypeHandler<T>
    where T : class
{
    public static void WriteProtobuf<TWriter>(in ReadOnlySpan<T> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        foreach (T value in self)
            THandler.WriteProtobuf(value, ref writer, fieldInfo, options);
    }
    public static void WriteProtobuf<TWriter>(in ReadOnlySpan<T> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        foreach (T value in self)
            THandler.WriteProtobuf(value, writer, fieldInfo, options);
    }
}