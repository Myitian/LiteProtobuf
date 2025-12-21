using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public interface IProtobufTypeStaticHandler<T> where T : allows ref struct
{
    public static abstract bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out T? value);
    public static abstract bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, [NotNullWhen(true)] out T? value)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    public static abstract bool ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, ref T instance)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    public static abstract void WriteProtobuf<TWriter>(ref TWriter writer, int index, in T instance)
        where TWriter : IBinaryWriter<TWriter>, allows ref struct;
}