namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufFieldCollection
{
    void ReadProtobufBody<TReader>(scoped ref TReader subReader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    void ReadProtobufBody<TReader>(TReader subReader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>;
    bool TryReadProtobufBody<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryReadProtobufBody<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    void AddProtobufField<TReader>(scoped ref TReader subReader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    void AddProtobufField<TReader>(TReader subReader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>;
    bool TryAddProtobufField<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryAddProtobufField<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    void WriteProtobufBody<TWriter>(scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    void WriteProtobufBody<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}