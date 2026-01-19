namespace Myitian.LiteProtobuf.Serialization;

/// <summary>
/// Indicates the field type. Some serializers may not read or follow this instruction.
/// <para>Bit layout, LSB=0:<list type="bullet">
/// <item>Bit 0~2: Basic field type preference</item>
/// <item>Bit 3~4: Repeated preference</item>
/// <item>Bit 5~6: Repeated encoding preference</item>
/// </list></para>
/// </summary>
[Flags]
public enum FieldTypeHint
{
    Auto,

    VarInt = 0b_000_010,
    VarIntZigZag = VarInt | 1,

    Fixed32 = 0b_000_100,
    Fixed64 = Fixed32 | 1,

    LengthDelimited = 0b_000_110,

    Repeated = 0b_00_01_000,
    NonRepeated = 0b_00_10_000,
    Packed = 0b_01_00_000,
    NonPacked = 0b_10_00_000
}
public static class FieldTypeExtension
{
    extension(FieldTypeHint type)
    {
        public bool? IsRepeated => (type & (FieldTypeHint.Repeated | FieldTypeHint.NonRepeated)) switch
        {
            FieldTypeHint.Repeated => true,
            FieldTypeHint.NonRepeated => false,
            _ => null,
        };
        public bool? IsPacked => (type & (FieldTypeHint.Packed | FieldTypeHint.NonPacked)) switch
        {
            FieldTypeHint.Packed => true,
            FieldTypeHint.NonPacked => false,
            _ => null,
        };
        public WireType? AsWireType => (type & (FieldTypeHint)0b111) switch
        {
            // 2, 3
            FieldTypeHint.VarInt or FieldTypeHint.VarIntZigZag => WireType.VarInt,
            // 4
            FieldTypeHint.Fixed32 => WireType.Fixed32,
            // 5
            FieldTypeHint.Fixed64 => WireType.Fixed64,
            // 6
            FieldTypeHint.LengthDelimited => WireType.LengthDelimited,
            // 0, 1, 7
            _ => null,
        };
    }
}