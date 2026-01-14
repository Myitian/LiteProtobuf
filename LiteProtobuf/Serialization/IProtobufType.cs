using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public interface IReadOnlyProtobufType<T>
    where T : IReadOnlyProtobufType<T>, allows ref struct
{
    public static abstract bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options);
    public static abstract bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value);
    public static abstract T CreateInstance(FieldInfo fieldInfo, SerializationOptions? options);
    public static abstract bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    public static abstract T CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract T CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>;
    bool IsFieldInfoValidForInstance(FieldInfo fieldInfo, SerializationOptions? options);
    bool TryReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    void ReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    void ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>;
}
public interface IWriteOnlyProtobufType<T>
    where T : IWriteOnlyProtobufType<T>, allows ref struct
{
    void WriteProtobuf<TWriter>(ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    void WriteProtobuf<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}
public interface IProtobufType<T>
    : IReadOnlyProtobufType<T>, IWriteOnlyProtobufType<T>
    where T : IProtobufType<T>, allows ref struct;