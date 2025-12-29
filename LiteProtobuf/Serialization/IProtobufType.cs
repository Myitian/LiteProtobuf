namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufType<T> : IProtobufTypeStaticHandler<T> where T : IProtobufType<T>, allows ref struct
{
    bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    bool TryReadProtobuf<TReader>(TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>;
    void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    void ReadProtobuf<TReader>(TReader reader, WireType receivedWireType)
        where TReader : class, IClassBinaryReader<TReader>;
    void WriteProtobuf<TWriter>(ref TWriter writer, int index)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    void WriteProtobuf<TWriter>(TWriter writer, int index)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}