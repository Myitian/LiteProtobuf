using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufType<T> where T : IProtobufType<T>, allows ref struct
{
    public static abstract bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out T? value);
    public static abstract bool TryCreateFulfilled<TReader>(scoped ref TReader reader, WireType wireType, [NotNullWhen(true)] out T? value, out ParseStatus status)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    void WriteProtobuf<TWriter>(ref TWriter writer, int index)
        where TWriter : IBinaryWriter<TWriter>, allows ref struct;
}