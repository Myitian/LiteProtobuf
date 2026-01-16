using Myitian.LiteProtobuf.Serialization;

namespace Myitian.LiteProtobuf.SourceGeneration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class GeneratedDefaultImplementationAttribute : Attribute
{
    public bool TryCreateInstance { get; set; }
    public bool CreateInstance { get; set; }
    public bool TryCreateFulfilled { get; set; }
    public bool CreateFulfilled { get; set; }
}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class GeneratedProtobufTypeSerializerAttribute : Attribute
{
    public bool Read { get; set; } = true;
    public bool TryRead { get; set; } = true;
    public bool Write { get; set; } = true;
    public bool NoSort { get; set; } = false;
}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ProtobufFieldAttribute(int number, FieldType fieldType = FieldType.Auto) : Attribute
{
    public int Number { get; } = number;
    public FieldType FieldType { get; } = fieldType;
    public int CustomAttribute { get; set; } = 0;
    public bool NoRead { get; set; } = false;
    public bool NoWrite { get; set; } = false;
    public Type? Handler { get; set; } = null;
    public Type? Factory { get => field ?? Handler; set; } = null;
    public Type? ReadHandler { get => field ?? Handler; set; } = null;
    public Type? WriteHandler { get => field ?? Handler; set; } = null;
}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class RemainingProtobufFieldsAttribute : Attribute;