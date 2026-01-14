using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Nodes;

[DefaultCreateInstance(typeof(ProtobufNumber))]
[DefaultTryCreateFulfilled(typeof(ProtobufNumber))]
[DefaultCreateFulfilled(typeof(ProtobufNumber))]
public sealed partial class ProtobufNumber(WireType type, ulong value)
    : ProtobufNode(type), IProtobufType<ProtobufNumber>
{
    public ulong Value { get; set; } = value;

    public static new bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return fieldInfo.ReceivedWireType is WireType.VarInt or WireType.Fixed64 or WireType.Fixed32;
    }
    public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out ProtobufNumber? value)
    {
        if (!IsFieldInfoValid(fieldInfo, options))
        {
            value = null;
            return false;
        }
        value = new(fieldInfo.ReceivedWireType, 0);
        return true;
    }
    protected override bool SharedTryReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        if (!IsFieldInfoValid(fieldInfo, options))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        switch (fieldInfo.ReceivedWireType)
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
    protected override void SharedReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        Value = fieldInfo.ReceivedWireType switch
        {
            WireType.VarInt => reader.ReadVarInt<ulong>(),
            WireType.Fixed64 => reader.ReadFixed64<ulong>(),
            WireType.Fixed32 => reader.ReadFixed32<uint>(),
            _ => throw new InvalidDataException(),
        };
    }
    protected override void SharedWriteProtobuf<TWriter>(ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
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
    public override string ToString()
    {
        switch (Type)
        {
            case WireType.Fixed32:
                return $"{{Number, Type = {Type}, Value = {Value} (as Single = {(BitConverter.UInt32BitsToSingle((uint)Value))})}}";
            case WireType.Fixed64:
                return $"{{Number, Type = {Type}, Value = {Value} (as Double = {(BitConverter.UInt64BitsToDouble(Value))})}}";
            default:
                return $"{{Number, Type = {Type}, Value = {Value} (as ZigZag = {ProtobufUtility.DecodeZigZag((long)Value)})}}";
        }
    }
}