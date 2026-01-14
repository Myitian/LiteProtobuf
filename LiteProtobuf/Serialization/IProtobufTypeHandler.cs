using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufTypeFactory<T>
    where T : allows ref struct
{
    bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options);
    bool IsFieldInfoValidForInstance(in T value, FieldInfo fieldInfo, SerializationOptions? options);
    bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value);
    bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
}
public interface IReadOnlyStructProtobufTypeHandler<T>
    : IProtobufTypeFactory<T>
    where T : struct, allows ref struct
{
    bool TryReadProtobuf<TReader>(ref T self, ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryReadProtobuf<TReader>(ref T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    void ReadProtobuf<TReader>(ref T self, ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    void ReadProtobuf<TReader>(ref T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>;
}
public interface IWriteOnlyStructProtobufTypeHandler<T>
    where T : struct, allows ref struct
{
    WireType WriteProtobuf<TWriter>(ref T self, ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    WireType WriteProtobuf<TWriter>(ref T self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}
public interface IStructProtobufTypeHandler<T>
    : IReadOnlyStructProtobufTypeHandler<T>, IWriteOnlyStructProtobufTypeHandler<T>
    where T : struct, allows ref struct;
public interface IReadOnlyClassProtobufTypeHandler<T>
    : IProtobufTypeFactory<T>
    where T : class
{
    bool TryReadProtobuf<TReader>(T self, ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryReadProtobuf<TReader>(T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : class, IClassBinaryReader<TReader>;
    void ReadProtobuf<TReader>(T self, ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    void ReadProtobuf<TReader>(T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : class, IClassBinaryReader<TReader>;
}
public interface IWriteOnlyClassProtobufTypeHandler<T>
    where T : class
{
    void WriteProtobuf<TWriter>(T self, ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    void WriteProtobuf<TWriter>(T self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : class, IClassBinaryWriter<TWriter>;
}
public interface IClassProtobufTypeHandler<T>
    : IReadOnlyClassProtobufTypeHandler<T>, IWriteOnlyClassProtobufTypeHandler<T>
    where T : class;