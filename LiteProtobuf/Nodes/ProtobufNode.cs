using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Nodes;

[DefaultTryCreateFulfilled(typeof(ProtobufNode))]
public abstract partial class ProtobufNode(WireType type)
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
    protected abstract bool SharedTryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : IBinaryReader, allows ref struct;
    protected abstract void SharedReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
        where TReader : IBinaryReader, allows ref struct;
    public virtual bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        => SharedTryReadProtobuf(ref reader, receivedWireType, out status);
    public virtual bool TryReadProtobuf<TReader>(TReader reader, WireType receivedWireType, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
        => SharedTryReadProtobuf(ref reader, receivedWireType, out status);
    public virtual void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        => SharedReadProtobuf(ref reader, receivedWireType);
    public virtual void ReadProtobuf<TReader>(TReader reader, WireType receivedWireType)
        where TReader : class, IClassBinaryReader<TReader>
        => SharedReadProtobuf(ref reader, receivedWireType);
    public abstract void WriteProtobuf<TWriter>(ref TWriter writer, int index)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    public abstract void WriteProtobuf<TWriter>(TWriter writer, int index)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}