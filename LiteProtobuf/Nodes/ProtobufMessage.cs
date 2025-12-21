using Myitian.LiteProtobuf.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Myitian.LiteProtobuf.Nodes;

public sealed class ProtobufMessage()
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
    public static bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out ProtobufMessage? value)
    {
        if (wireType is not WireType.LengthDelimited)
        {
            value = null;
            return false;
        }
        value = new();
        return true;
    }
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, WireType wireType, [NotNullWhen(true)] out ProtobufMessage? value, out ParseStatus status)
        where TReader : IBinaryReader<TReader>, allows ref struct
    {
        if (!TryCreateInstance(wireType, out value))
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        return value.TryReadProtobuf(ref reader, wireType, out status);
    }

    public override bool TryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
    {
        if (receivedWireType is not WireType.LengthDelimited)
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        if (!TReader.TryCreateLengthDelimitedReader(ref reader, out TReader? subReader, out status))
        {
            subReader?.Dispose();
            return false;
        }
        try
        {
            Children.Clear();
            ParseStatus subStatus;
            while (ProtobufUtility.TryReadTag(ref subReader, out int index, out WireType childWireType, out subStatus))
            {
                if (!TryCreateInstance(childWireType, out ProtobufNode? child)
                    || !child.TryReadProtobuf(ref subReader, childWireType, out subStatus))
                {
                    status = ParseStatus.InvalidData;
                    return false;
                }
                Children.Add(new(index, child));
            }
            status = subStatus == ParseStatus.ExactEndOfStream ? ParseStatus.Success : ParseStatus.InvalidData;
            return true;
        }
        finally
        {
            subReader?.Dispose();
        }
    }
    public override void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
    {
        if (receivedWireType is not WireType.LengthDelimited)
            throw new InvalidDataException();
        TReader? subReader = TReader.CreateLengthDelimitedReader(ref reader);
        try
        {
            ReadProtobufBody(ref subReader);
        }
        finally
        {
            subReader?.Dispose();
        }
    }

    public void ReadProtobufBody<TReader>(ref TReader subReader)
        where TReader : IBinaryReader<TReader>, allows ref struct
    {
        ParseStatus subStatus;
        while (ProtobufUtility.TryReadTag(ref subReader, out int index, out WireType childWireType, out subStatus))
        {
            if (!TryCreateInstance(childWireType, out ProtobufNode? child))
                throw new InvalidDataException($"Invalid wire type: {childWireType}");
            child.ReadProtobuf(ref subReader, childWireType);
            Children.Add(new(index, child));
        }
        if (subStatus != ParseStatus.ExactEndOfStream)
            throw new InvalidDataException();
    }

    public override void WriteProtobuf<TWriter>(ref TWriter writer, int index)
    {
        ProtobufUtility.WriteTag(ref writer, index, Type);
        TWriter subWriter = TWriter.CreateLengthDelimitedWriter(ref writer);
        try
        {
            WriteProtobufBody(ref subWriter);
        }
        finally
        {
            subWriter?.Dispose();
        }
    }

    public void WriteProtobufBody<TWriter>(ref TWriter writer)
        where TWriter : IBinaryWriter<TWriter>, allows ref struct
    {
        foreach ((int i, ProtobufNode node) in Children)
            node.WriteProtobuf(ref writer, i);
    }

    public override string ToString()
    {
        return $"{{Message, ChildCount = {Children.Count}}}";
    }
    public StringBuilder ToString(StringBuilder? sb = null, int recursion = -1, int indent = 2, int depth = 0, int? id = null)
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
                        message.ToString(sb, nextRecursion, indent, depth, childID);
                    else
                        sb.Append(' ', indent * depth).Append(childID).Append(": ").AppendLine(child.ToString());
                }
            }
        }
        return sb;
    }
}