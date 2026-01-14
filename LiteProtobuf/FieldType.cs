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
public static class FieldTypeExtension
{
    public static RepeatedEncoding GetRepeatedEncoding(this FieldType type)
    {
        const FieldType mask = FieldType.Packed | FieldType.NonPacked;
        return (type & mask) switch
        {
            FieldType.Packed => RepeatedEncoding.Packed,
            FieldType.NonPacked => RepeatedEncoding.NonPacked,
            _ => RepeatedEncoding.Auto,
        };
    }
}