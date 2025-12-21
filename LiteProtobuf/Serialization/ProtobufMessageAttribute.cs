namespace Myitian.LiteProtobuf.Serialization;

// For the source generation of serialization code. Work in progress.

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ProtobufFieldAttribute(int index, FieldType fieldType = FieldType.Auto) : Attribute
{
    public int Index { get; } = index;
    public FieldType FieldType { get; } = fieldType;
    public Type? Converter { get; set; } = null;
}
