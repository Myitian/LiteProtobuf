using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Text;

namespace Myitian.LiteProtobuf.Nodes;

[GeneratedDefaultImplementation(
    TryCreateInstance = true,
    CreateInstance = true,
    TryCreateFulfilled = true,
    CreateFulfilled = true)]
public sealed partial class ProtobufMessage()
    : ProtobufNode(WireType.LengthDelimited), IProtobufType<ProtobufMessage>, IProtobufFieldCollection
{
    public List<KeyValuePair<int, ProtobufNode>> Children { get; } = [];

    public override ProtobufNode Expand(int recursion = -1)
    {
        ExpandChildren(recursion);
        return this;
    }
    public void ExpandChildren(int recursion = -1)
    {
        if (recursion == 0)
            return;
        int nextRecursion = Math.Max(recursion, 0) - 1;
        for (int i = 0; i < Children.Count; i++)
        {
            KeyValuePair<int, ProtobufNode> kvp = Children[i];
            Children[i] = new(kvp.Key, kvp.Value.Expand(nextRecursion));
        }
    }
    public static new bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return fieldInfo.ReceivedWireType is WireType.LengthDelimited;
    }
    protected override bool SharedTryReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        => throw new NotSupportedException();
    protected override void SharedReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        => throw new NotSupportedException();
    protected override void SharedWriteProtobuf<TWriter>(scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        => throw new NotSupportedException();
    public override bool TryReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        if (!TReader.TryCreateLengthDelimitedReader(ref reader, out TReader subReader, out status))
        {
            subReader.Dispose();
            return false;
        }
        try
        {
            return TryReadProtobufBody(ref subReader, options, out status);
        }
        finally
        {
            subReader.Dispose();
        }
    }
    public bool TryReadProtobufBody<TReader>(scoped ref TReader reader, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        Children.Clear();
        ParseStatus subStatus;
        while (reader.TryReadTag(out FieldInfo fi, out subStatus))
        {
            if (!TryAddProtobufField(ref reader, fi, options, out _))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
        }
        status = subStatus == ParseStatus.ExactEndOfStream ? ParseStatus.Success : ParseStatus.InvalidData;
        return true;
    }
    public bool TryAddProtobufField<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!TryCreateInstance(fieldInfo, options, out ProtobufNode? child))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        if (!child.TryReadProtobuf(ref reader, fieldInfo, options, out status))
            return false;
        Children.Add(new(fieldInfo.Number, child));
        return true;
    }
    public override bool TryReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        if (!TReader.TryCreateLengthDelimitedReader(reader, out TReader? subReader, out status))
        {
            subReader?.Dispose();
            return false;
        }
        try
        {
            return TryReadProtobufBody(subReader, options, out status);
        }
        finally
        {
            subReader.Dispose();
        }
    }
    public bool TryReadProtobufBody<TReader>(TReader reader, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        Children.Clear();
        ParseStatus subStatus;
        while (reader.TryReadTag(out FieldInfo fi, out subStatus))
        {
            if (!TryAddProtobufField(reader, fi, options, out _))
            {
                status = ParseStatus.InvalidData;
                return false;
            }
        }
        status = subStatus == ParseStatus.ExactEndOfStream ? ParseStatus.Success : ParseStatus.InvalidData;
        return true;
    }
    public bool TryAddProtobufField<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!TryCreateInstance(fieldInfo, options, out ProtobufNode? child))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        if (!child.TryReadProtobuf(reader, fieldInfo, options, out status))
            return false;
        Children.Add(new(fieldInfo.Number, child));
        return true;
    }
    public override void ReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
            throw new InvalidDataException();
        TReader subReader = TReader.CreateLengthDelimitedReader(ref reader);
        try
        {
            ReadProtobufBody(ref subReader, options);
        }
        finally
        {
            subReader.Dispose();
        }
    }
    public void ReadProtobufBody<TReader>(scoped ref TReader subReader, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        ParseStatus subStatus;
        while (subReader.TryReadTag(out FieldInfo fi, out subStatus))
            AddProtobufField(ref subReader, fi, options);
        if (subStatus != ParseStatus.ExactEndOfStream)
            throw new InvalidDataException();
    }
    public void AddProtobufField<TReader>(scoped ref TReader subReader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!TryCreateInstance(fieldInfo, options, out ProtobufNode? child))
            throw new InvalidDataException($"Invalid data: {fieldInfo}");
        child.ReadProtobuf(ref subReader, fieldInfo, options);
        Children.Add(new(fieldInfo.Number, child));
    }
    public override void ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
            throw new InvalidDataException();
        using TReader subReader = TReader.CreateLengthDelimitedReader(reader);
        ReadProtobufBody(subReader, options);
    }
    public void ReadProtobufBody<TReader>(TReader subReader, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        ParseStatus subStatus;
        while (subReader.TryReadTag(out FieldInfo fi, out subStatus))
            AddProtobufField(subReader, fi, options);
        if (subStatus != ParseStatus.ExactEndOfStream)
            throw new InvalidDataException();
    }
    public void AddProtobufField<TReader>(TReader subReader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        if (!TryCreateInstance(fieldInfo, options, out ProtobufNode? child))
            throw new InvalidDataException($"Invalid data: {fieldInfo}");
        child.ReadProtobuf(subReader, fieldInfo, options);
        Children.Add(new(fieldInfo.Number, child));
    }
    public override void WriteProtobuf<TWriter>(scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        writer.WriteTag(fieldInfo.Number, Type);
        TWriter subWriter = TWriter.CreateLengthDelimitedWriter(ref writer);
        try
        {
            WriteProtobufBody(ref subWriter, options);
        }
        finally
        {
            subWriter.Dispose();
        }
    }
    public override void WriteProtobuf<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        writer.WriteTag(fieldInfo.Number, Type);
        using TWriter subWriter = TWriter.CreateLengthDelimitedWriter(writer);
        WriteProtobufBody(subWriter, options);
    }
    public void WriteProtobufBody<TWriter>(scoped ref TWriter writer, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        foreach ((int i, ProtobufNode node) in Children)
            node.WriteProtobuf(ref writer, new() { Number = i }, options);
    }
    public void WriteProtobufBody<TWriter>(TWriter writer, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        foreach ((int i, ProtobufNode node) in Children)
            node.WriteProtobuf(writer, new() { Number = i }, options);
    }

    public override string ToString()
    {
        return $"{{Message, ChildCount = {Children.Count}}}";
    }
    public StringBuilder ToFormattedString(StringBuilder? sb = null, int recursion = -1, int indent = 2, int depth = 0, int? id = null)
    {
        sb ??= new();
        if (recursion != 0)
        {
            sb.Append(' ', indent * depth);
            if (id.HasValue)
                sb.Append(id.Value).Append(": ");
            sb.AppendLine(ToString());
            int nextRecursion = Math.Max(recursion, 0) - 1;
            if (nextRecursion != 0)
            {
                depth++;
                foreach ((int childID, ProtobufNode child) in Children)
                {
                    if (child is ProtobufMessage message)
                        message.ToFormattedString(sb, nextRecursion, indent, depth, childID);
                    else
                        sb.Append(' ', indent * depth).Append(childID).Append(": ").AppendLine(child.ToString());
                }
            }
        }
        return sb;
    }
}