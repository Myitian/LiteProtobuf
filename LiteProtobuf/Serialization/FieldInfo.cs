namespace Myitian.LiteProtobuf.Serialization;

public struct FieldInfo
{
    /// <summary>
    /// The field number.
    /// </summary>
    public int Number;
    /// <summary>
    /// Indicates the field type. May not be read by some serializers.
    /// </summary>
    public FieldTypeHint FieldTypeHint;
    /// <summary>
    /// Only used during deserialization.
    /// </summary>
    public WireType ReceivedWireType;
    /// <summary>
    /// Custom information associated with the field.
    /// </summary>
    public int CustomAttribute;

    public override readonly string ToString()
    {
        return $"{{{nameof(Number)} = {Number}, {nameof(FieldTypeHint)} = {FieldTypeHint}, {nameof(ReceivedWireType)} = {ReceivedWireType}, {nameof(CustomAttribute)} = {CustomAttribute}";
    }
}