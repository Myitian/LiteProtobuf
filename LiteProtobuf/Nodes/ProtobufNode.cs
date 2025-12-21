using Myitian.LiteProtobuf.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Nodes;

public abstract class ProtobufNode(WireType type)
    : IProtobufType<ProtobufNode>
{
    public WireType Type { get; } = type;

    public virtual ProtobufNode Expand(int recursion = -1)
    {
        return this;
    }
    public static bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out ProtobufNode? value)
    {
        if (ProtobufByteArray.TryCreateInstance(wireType, out ProtobufByteArray? v1))
        {
            value = v1;
            return true;
        }
        if (ProtobufNumber.TryCreateInstance(wireType, out ProtobufNumber? v2))
        {
            value = v2;
            return true;
        }
        value = null;
        return false;
    }
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, WireType wireType, [NotNullWhen(true)] out ProtobufNode? value, out ParseStatus status)
        where TReader : IBinaryReader<TReader>, allows ref struct
    {
        if (!TryCreateInstance(wireType, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, wireType, out status);
    }
    public abstract bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    public abstract void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
        where TReader : IBinaryReader<TReader>, allows ref struct;
    public abstract void WriteProtobuf<TWriter>(ref TWriter writer, int index)
        where TWriter : IBinaryWriter<TWriter>, allows ref struct;
}