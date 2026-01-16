using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufTypeFieldInfoValidator<T>
    where T : allows ref struct
{
    public static abstract bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options);
    public static abstract bool IsFieldInfoValidForInstance(in T value, FieldInfo fieldInfo, SerializationOptions? options);
}
public interface IProtobufTypeFactory<T> : IProtobufTypeFieldInfoValidator<T>
    where T : allows ref struct
{
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
}
public interface IReadOnlyStructProtobufTypeHandler<T> : IProtobufTypeFieldInfoValidator<T>
    where T : struct, allows ref struct
{
    public static abstract bool TryReadProtobuf<TReader>(scoped ref T self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract bool TryReadProtobuf<TReader>(scoped ref T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    public static abstract void ReadProtobuf<TReader>(scoped ref T self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract void ReadProtobuf<TReader>(scoped ref T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>;
}
public interface IWriteOnlyStructProtobufTypeHandler<T>
    where T : struct, allows ref struct
{
    public static abstract void WriteProtobuf<TWriter>(in T self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    public static abstract void WriteProtobuf<TWriter>(in T self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}
public interface IStructProtobufTypeHandler<T>
    : IProtobufTypeFactory<T>, IReadOnlyStructProtobufTypeHandler<T>, IWriteOnlyStructProtobufTypeHandler<T>
    where T : struct, allows ref struct;
public interface IReadOnlyClassProtobufTypeHandler<T> : IProtobufTypeFieldInfoValidator<T>
    where T : class
{
    public static abstract bool TryReadProtobuf<TReader>(T self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract bool TryReadProtobuf<TReader>(T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : class, IClassBinaryReader<TReader>;
    public static abstract void ReadProtobuf<TReader>(T self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public static abstract void ReadProtobuf<TReader>(T self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : class, IClassBinaryReader<TReader>;
}
public interface IWriteOnlyClassProtobufTypeHandler<T>
    where T : class
{
    public static abstract void WriteProtobuf<TWriter>(T self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    public static abstract void WriteProtobuf<TWriter>(T self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : class, IClassBinaryWriter<TWriter>;
}
public interface IClassProtobufTypeHandler<T>
    : IProtobufTypeFactory<T>, IReadOnlyClassProtobufTypeHandler<T>, IWriteOnlyClassProtobufTypeHandler<T>
    where T : class;