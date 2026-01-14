using Myitian.LiteProtobuf.Serialization;

namespace Myitian.LiteProtobuf.SourceGeneration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class DefaultTryCreateInstanceAttribute : Attribute;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class DefaultCreateInstanceAttribute : Attribute;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class DefaultTryCreateFulfilledAttribute : Attribute;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class DefaultCreateFulfilledAttribute : Attribute;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class GeneratedProtobufTypeSerializerAttribute : Attribute
{
    public bool Read { get; set; } = true;
    public bool TryRead { get; set; } = true;
    public bool Write { get; set; } = true;
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
    public Type? ReadHandler { get => field ?? Handler; set; } = null;
    public Type? WriteHandler { get => field ?? Handler; set; } = null;
}