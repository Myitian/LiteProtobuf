namespace Myitian.LiteProtobuf.Serialization;

/// <summary>
/// Indicates the field type. May not be read by some serializers.
/// <list type="bullet">
/// <item>Bit 5~4: Repeated encoding preference</item>
/// <item>Bit 3: Whether it is a repeated field</item>
/// <item>Bit 2~0: Basic field type</item>
/// </list>
/// </summary>
[Flags]
public enum FieldType
{
    Auto,
    Variant, // There are no variations for Auto and LengthDelimited

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
    public static bool IsRepeated(this FieldType type)
        => (type & FieldType.Repeated) != 0;
    public static RepeatedEncoding GetRepeatedEncoding(this FieldType type)
        => (type & (FieldType.Packed | FieldType.NonPacked)) switch
        {
            FieldType.Packed => RepeatedEncoding.Packed,
            FieldType.NonPacked => RepeatedEncoding.NonPacked,
            _ => RepeatedEncoding.Auto,
        };
    public static WireType? GetWireType(this FieldType type)
        => (type & (FieldType)0b111) switch
        {
            // 0, 1
            FieldType.Auto or FieldType.Variant => null,
            // 2, 3
            FieldType.VarInt or FieldType.VarIntZigZag => WireType.VarInt,
            // 4
            FieldType.Fixed32 => WireType.Fixed32,
            // 5
            FieldType.Fixed64 => WireType.Fixed64,
            // 6, 7
            _ => WireType.LengthDelimited
        };
}