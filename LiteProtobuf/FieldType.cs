namespace Myitian.LiteProtobuf;

[Flags]
public enum FieldType
{
    Auto,
    Variant,

    VarInt = 0b_000_010,
    VarIntZigZag = VarInt | Variant,

    Fixed32 = 0b_000_100,
    Fixed64 = Fixed32 | Variant,

    LengthDelimited = 0b_000_110,

    Repeated = 0b_001_000,
    Packed = 0b_010_000,
    NonPacked = 0b_100_000,

    RepeatedVarInt = Repeated | VarInt,
    RepeatedVarIntZigZag = Repeated | VarIntZigZag,
    RepeatedFixed32 = Repeated | Fixed32,
    RepeatedFixed64 = Repeated | Fixed64,
    RepeatedLengthDelimited = Repeated | LengthDelimited,
}