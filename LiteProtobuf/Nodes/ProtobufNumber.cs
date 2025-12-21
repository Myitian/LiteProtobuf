using Myitian.LiteProtobuf.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Nodes;

public sealed class ProtobufNumber(WireType type, ulong value)
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
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, WireType wireType, [NotNullWhen(true)] out ProtobufNumber? value, out ParseStatus status)
        where TReader : IBinaryReader<TReader>, allows ref struct
    {
        if (!TryCreateInstance(wireType, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, wireType, out status);
    }
    public override bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
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
    public override void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
    {
        switch (receivedWireType)
        {
            case WireType.VarInt:
                Value = reader.ReadVarInt<ulong>();
                break;
            case WireType.Fixed64:
                Value = reader.ReadFixed64<ulong>();
                break;
            case WireType.Fixed32:
                Value = reader.ReadFixed32<uint>();
                break;
            default:
                throw new InvalidDataException();
        }
    }
    public override void WriteProtobuf<TWriter>(ref TWriter writer, int index)
    {
        ProtobufUtility.WriteTag(ref writer, index, Type);
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
    public override string ToString()
    {
        return $"{{Number, Type = {Type}, Value = {Value}}}";
    }
}