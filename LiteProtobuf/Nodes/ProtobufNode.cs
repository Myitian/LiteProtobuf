using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Nodes;

[DefaultCreateInstance(typeof(ProtobufNode))]
[DefaultTryCreateFulfilled(typeof(ProtobufNode))]
[DefaultCreateFulfilled(typeof(ProtobufNode))]
public abstract partial class ProtobufNode(WireType type)
    : IProtobufType<ProtobufNode>
{
    public WireType Type { get; } = type;

    public virtual ProtobufNode Expand(int recursion = -1)
    {
        return this;
    }
    public virtual bool IsFieldInfoValidForInstance(FieldInfo fieldInfo, SerializationOptions? options)
        => fieldInfo.ReceivedWireType == Type;
    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return ProtobufByteArray.IsFieldInfoValid(fieldInfo, options)
            || ProtobufNumber.IsFieldInfoValid(fieldInfo, options);
    }
    public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out ProtobufNode? value)
    {
        if (ProtobufByteArray.TryCreateInstance(fieldInfo, options, out ProtobufByteArray? v1))
        {
            value = v1;
            return true;
        }
        if (ProtobufNumber.TryCreateInstance(fieldInfo, options, out ProtobufNumber? v2))
        {
            value = v2;
            return true;
        }
        value = null;
        return false;
    }
    protected abstract bool SharedTryReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : IBinaryReader, allows ref struct;
    protected abstract void SharedReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : IBinaryReader, allows ref struct;
    protected abstract void SharedWriteProtobuf<TWriter>(ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : IBinaryWriter, allows ref struct;
    public virtual bool TryReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        => SharedTryReadProtobuf(ref reader, fieldInfo, options, out status);
    public virtual bool TryReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
        => SharedTryReadProtobuf(ref reader, fieldInfo, options, out status);
    public virtual void ReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        => SharedReadProtobuf(ref reader, fieldInfo, options);
    public virtual void ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
        => SharedReadProtobuf(ref reader, fieldInfo, options);
    public virtual void WriteProtobuf<TWriter>(ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        writer.WriteTag(fieldInfo.Index, Type);
        SharedWriteProtobuf(ref writer, fieldInfo, options);
    }
    public virtual void WriteProtobuf<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        writer.WriteTag(fieldInfo.Index, Type);
        SharedWriteProtobuf(ref writer, fieldInfo, options);
    }
}