using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Text;

namespace Myitian.LiteProtobuf.Nodes;

[DefaultTryCreateInstance(typeof(ProtobufMessage))]
[DefaultCreateInstance(typeof(ProtobufMessage))]
[DefaultTryCreateFulfilled(typeof(ProtobufMessage))]
[DefaultCreateFulfilled(typeof(ProtobufMessage))]
public sealed partial class ProtobufMessage()
    : ProtobufNode(WireType.LengthDelimited), IProtobufType<ProtobufMessage>
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
    protected override bool SharedTryReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        => throw new NotSupportedException();
    protected override void SharedReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        => throw new NotSupportedException();
    protected override void SharedWriteProtobuf<TWriter>(ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        => throw new NotSupportedException();
    public override bool TryReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
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
            Children.Clear();
            ParseStatus subStatus;
            while (subReader.TryReadTag(out fieldInfo, out subStatus))
            {
                if (!TryCreateInstance(fieldInfo, options, out ProtobufNode? child)
                    || !child.TryReadProtobuf(ref subReader, fieldInfo, options, out subStatus))
                {
                    status = ParseStatus.InvalidData;
                    return false;
                }
                Children.Add(new(fieldInfo.Index, child));
            }
            status = subStatus == ParseStatus.ExactEndOfStream ? ParseStatus.Success : ParseStatus.InvalidData;
            return true;
        }
        finally
        {
            subReader.Dispose();
        }
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
        using (subReader)
        {
            Children.Clear();
            ParseStatus subStatus;
            while (subReader.TryReadTag(out fieldInfo, out subStatus))
            {
                if (!TryCreateInstance(fieldInfo, options, out ProtobufNode? child)
                    || !child.TryReadProtobuf(subReader, fieldInfo, options, out subStatus))
                {
                    status = ParseStatus.InvalidData;
                    return false;
                }
                Children.Add(new(fieldInfo.Index, child));
            }
            status = subStatus == ParseStatus.ExactEndOfStream ? ParseStatus.Success : ParseStatus.InvalidData;
            return true;
        }
    }
    public override void ReadProtobuf<TReader>(ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
            throw new InvalidDataException();
        TReader subReader = TReader.CreateLengthDelimitedReader(ref reader);
        try
        {
            ReadProtobufBody(ref subReader);
        }
        finally
        {
            subReader.Dispose();
        }
    }
    public override void ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
            throw new InvalidDataException();
        using TReader subReader = TReader.CreateLengthDelimitedReader(reader);
        ReadProtobufBody(subReader);
    }
    public void ReadProtobufBody<TReader>(scoped ref TReader subReader)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        ParseStatus subStatus;
        while (subReader.TryReadTag(out FieldInfo fi, out subStatus))
        {
            if (!TryCreateInstance(fi, null, out ProtobufNode? child))
                throw new InvalidDataException($"Invalid wire type: {fi}");
            child.ReadProtobuf(ref subReader, fi, null);
            Children.Add(new(fi.Index, child));
        }
        if (subStatus != ParseStatus.ExactEndOfStream)
            throw new InvalidDataException();
    }

    public void ReadProtobufBody<TReader>(TReader subReader)
        where TReader : class, IClassBinaryReader<TReader>
    {
        ParseStatus subStatus;
        while (subReader.TryReadTag(out FieldInfo fi, out subStatus))
        {
            if (!TryCreateInstance(fi, null, out ProtobufNode? child))
                throw new InvalidDataException($"Invalid data: {fi}");
            child.ReadProtobuf(subReader, fi, null);
            Children.Add(new(fi.Index, child));
        }
        if (subStatus != ParseStatus.ExactEndOfStream)
            throw new InvalidDataException();
    }
    public override void WriteProtobuf<TWriter>(ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        writer.WriteTag(fieldInfo.Index, Type);
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
        writer.WriteTag(fieldInfo.Index, Type);
        using TWriter subWriter = TWriter.CreateLengthDelimitedWriter(writer);
        WriteProtobufBody(subWriter, options);
    }
    public void WriteProtobufBody<TWriter>(ref TWriter writer, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        foreach ((int i, ProtobufNode node) in Children)
            node.WriteProtobuf(ref writer, new() { Index = i }, options);
    }
    public void WriteProtobufBody<TWriter>(TWriter writer, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        foreach ((int i, ProtobufNode node) in Children)
            node.WriteProtobuf(writer, new() { Index = i }, options);
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
            sb.AppendLine(((object)this).ToString());
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