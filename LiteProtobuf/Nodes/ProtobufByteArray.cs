using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Myitian.LiteProtobuf.Nodes;

[DefaultTryCreateFulfilled(typeof(ProtobufByteArray))]
public partial class ProtobufByteArray()
    : ProtobufNode(WireType.LengthDelimited), IProtobufType<ProtobufByteArray>
{
    public byte[]? Data { get; set; }
    public override ProtobufNode Expand(int recursion = -1)
    {
        if (recursion == 0)
            return this;
        return ExpandToMessage(Data, recursion) ?? (ProtobufNode?)AsString() ?? this;

        static ProtobufMessage? ExpandToMessage(ReadOnlySpan<byte> data, int recursion)
        {
            SpanBinaryReader reader = new(data);
            ProtobufMessage result = new();
            try
            {
                ParseStatus subStatus;
                int nextRecursion = Math.Max(recursion, 0) - 1;
                while (reader.TryReadTag(out int index, out WireType childWireType, out subStatus))
                {
                    if (!TryCreateInstance(childWireType, out ProtobufNode? child))
                        return null;
                    if (!child.TryReadProtobuf(ref reader, childWireType, out _))
                        return null;
                    if (recursion != 0)
                        child = child.Expand();
                    result.Children.Add(new(index, child));
                }
                if (subStatus != ParseStatus.ExactEndOfStream)
                    return null;
                return result;
            }
            finally
            {
                reader.Dispose();
            }
        }
    }
    public static bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out ProtobufByteArray? value)
    {
        if (wireType is not WireType.LengthDelimited)
        {
            value = null;
            return false;
        }
        value = new();
        return true;
    }
    protected override bool SharedTryReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType, out ParseStatus status)
    {
        if (receivedWireType is not WireType.LengthDelimited)
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        if (reader.TryReadByteArray(out byte[] data, out status))
        {
            Data = data;
            return true;
        }
        else
        {
            if (status == ParseStatus.ExactEndOfStream)
                status = ParseStatus.EndOfStream;
            return false;
        }
    }
    protected override void SharedReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
    {
        if (receivedWireType is not WireType.LengthDelimited)
            throw new InvalidDataException();
        Data = reader.ReadByteArray();
    }
    public override void WriteProtobuf<TWriter>(ref TWriter writer, int index)
    {
        writer.WriteTag(index, WireType.LengthDelimited);
        writer.WriteLengthDelimited(Data);
    }
    public override void WriteProtobuf<TWriter>(TWriter writer, int index)
    {
        writer.WriteTag(index, WireType.LengthDelimited);
        writer.WriteLengthDelimited(Data);
    }
    public override string ToString()
    {
        return $"{{LengthDelimited, Length = {Data?.Length ?? 0}, {DisplayLimitedBytes(Data, 32)}}}";
    }
    public static string DisplayLimitedBytes(ReadOnlySpan<byte> bytes, int limit)
    {
        StringBuilder sb = new();
        sb.Append("b\"");
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i == limit)
                return sb.Append($"\" ... and {bytes.Length - limit} more").ToString();
            byte b = bytes[i];
            switch (b)
            {
                case (byte)'\t':
                    sb.Append("\\t");
                    break;
                case (byte)'\n':
                    sb.Append("\\n");
                    break;
                case (byte)'\r':
                    sb.Append("\\r");
                    break;
                case (byte)'"':
                    sb.Append("\\\"");
                    break;
                case (byte)'\\':
                    sb.Append("\\\\");
                    break;
                default:
                    if (b is > 0x1F and < 0x7F)
                        sb.Append((char)b);
                    else
                        sb.Append($"\\x{b:X2}");
                    break;
            }
        }
        return sb.Append('"').ToString();
    }
    public ProtobufString? AsString()
    {
        try
        {
            string s = ProtobufUtility.DefaultEncoding.GetString(Data.AsSpan());
            ProtobufString str = new() { Value = s };
            return str;
        }
        catch
        {
            return null;
        }
    }
    public ProtobufMessage? AsMessage(int recursion = -1)
    {
        SpanBinaryReader reader = new(Data);
        ProtobufMessage result = new();
        try
        {
            ParseStatus subStatus;
            int nextRecursion = Math.Max(recursion, 0) - 1;
            while (ProtobufUtility.TryReadTag(ref reader, out int index, out WireType childWireType, out subStatus))
            {
                if (!TryCreateInstance(childWireType, out ProtobufNode? child))
                    return null;
                if (!child.TryReadProtobuf(ref reader, childWireType, out _))
                    return null;
                if (recursion != 0 && child is ProtobufByteArray childLD)
                {
                    ProtobufMessage? childMSG = childLD.AsMessage(nextRecursion);
                    if (childMSG is not null)
                        child = childMSG;
                }
                result.Children.Add(new(index, child));
            }
            if (subStatus != ParseStatus.ExactEndOfStream)
                return null;
            return result;
        }
        finally
        {
            reader.Dispose();
        }
    }
}