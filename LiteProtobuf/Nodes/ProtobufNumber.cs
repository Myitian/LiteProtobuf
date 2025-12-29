using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Nodes;

[DefaultTryCreateFulfilled(typeof(ProtobufNumber))]
public sealed partial class ProtobufNumber(WireType type, ulong value)
    : ProtobufNode(type), IProtobufType<ProtobufNumber>
{
    public ulong Value { get; set; } = value;
    public static bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out ProtobufNumber? value)
    {
        if (wireType is not (WireType.VarInt or WireType.Fixed32 or WireType.Fixed64))
        {
            value = null;
            return false;
        }
        value = new(wireType, 0);
        return true;
    }
    protected override bool SharedTryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
    {
        if (receivedWireType != Type)
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        switch (receivedWireType)
        {
            case WireType.VarInt when reader.TryReadVarInt(out ulong u, out status):
                Value = u;
                return true;
            case WireType.Fixed64 when reader.TryReadFixed64(out ulong u, out status):
                Value = u;
                return true;
            case WireType.Fixed32 when reader.TryReadFixed32(out uint u, out status):
                Value = u;
                return true;
            default:
                status = ParseStatus.InvalidData;
                return false;
        }
    }
    protected override void SharedReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
    {
        Value = receivedWireType switch
        {
            WireType.VarInt => reader.ReadVarInt<ulong>(),
            WireType.Fixed64 => reader.ReadFixed64<ulong>(),
            WireType.Fixed32 => reader.ReadFixed32<uint>(),
            _ => throw new InvalidDataException(),
        };
    }
    private void SharedWriteProtobuf<TWriter>(ref TWriter writer, int index)
         where TWriter : IBinaryWriter, allows ref struct
    {
        switch (Type)
        {
            case WireType.VarInt:
                writer.WriteVarInt(Value);
                break;
            case WireType.Fixed64:
                writer.WriteFixed64(Value);
                break;
            case WireType.Fixed32:
                writer.WriteFixed32((uint)Value);
                break;
            default:
                throw new NotSupportedException();
        }
    }
    public override void WriteProtobuf<TWriter>(ref TWriter writer, int index)
    {
        writer.WriteTag(index, Type);
        SharedWriteProtobuf(ref writer, index);
    }
    public override void WriteProtobuf<TWriter>(TWriter writer, int index)
    {
        writer.WriteTag(index, Type);
        SharedWriteProtobuf(ref writer, index);
    }
    public override string ToString()
    {
        return $"{{Number, Type = {Type}, Value = {Value}}}";
    }
}