using System;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public enum RepeatedInfo : byte
{
    Auto,
    Packed,
    NonPacked,
    NonRepeated
}
public enum BasicFieldType : byte
{
    Auto,
    VarInt,
    VarIntZigZag,
    Fixed32,
    Fixed64,
    LengthDelimited
}
public readonly record struct FieldType
{
    const int Auto = 0;
    const int Variant = 1;
    const int VarInt = 0b_000_010;
    const int VarIntZigZag = VarInt | Variant;
    const int Fixed32 = 0b_000_100;
    const int Fixed64 = Fixed32 | Variant;
    const int LengthDelimited = 0b_000_110;
    const int Repeated = 0b_001_000;
    const int Packed = 0b_010_000;
    const int NonPacked = 0b_100_000;
    const int RepeatedVarInt = Repeated | VarInt;
    const int RepeatedVarIntZigZag = Repeated | VarIntZigZag;
    const int RepeatedFixed32 = Repeated | Fixed32;
    const int RepeatedFixed64 = Repeated | Fixed64;
    const int RepeatedLengthDelimited = Repeated | LengthDelimited;

    public RepeatedInfo RepeatedInfo { get; }
    public BasicFieldType BasicFieldType { get; }

    public FieldType(RepeatedInfo info, BasicFieldType type)
    {
        RepeatedInfo = info;
        BasicFieldType = type;
    }
    public FieldType(object value) : this(RepeatedInfo.NonRepeated, BasicFieldType.Auto)
    {
        int v = (int)Convert.ToInt64(value);
        if ((v & Repeated) != 0)
        {
            RepeatedInfo = (v & (Packed | NonPacked)) switch
            {
                Packed => RepeatedInfo.Packed,
                NonPacked => RepeatedInfo.NonPacked,
                _ => RepeatedInfo.Auto
            };
        }
        BasicFieldType = (v & 0b111) switch
        {
            // 0, 1
            Auto or Variant => BasicFieldType.Auto,
            // 2
            VarInt => BasicFieldType.VarInt,
            // 3
            VarIntZigZag => BasicFieldType.VarIntZigZag,
            // 4
            Fixed32 => BasicFieldType.Fixed32,
            // 5
            Fixed64 => BasicFieldType.Fixed64,
            // 6, 7
            _ => BasicFieldType.LengthDelimited
        };
    }
}