namespace Myitian.LiteProtobuf.Serialization;

public struct FieldInfo
{
    public int Index;
    public FieldType FieldTypeHint;
    public WireType ReceivedWireType;
    public int CustomAttribute;

    public override readonly string ToString()
    {
        return $"{{{nameof(Index)} = {Index}, {nameof(FieldTypeHint)} = {FieldTypeHint}, {nameof(ReceivedWireType)} = {ReceivedWireType}, {nameof(CustomAttribute)} = {CustomAttribute}";
    }
}