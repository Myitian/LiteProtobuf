using Myitian.LiteProtobuf.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Myitian.LiteProtobuf.Nodes;

public sealed class ProtobufString()
    : ProtobufNode(WireType.LengthDelimited), IProtobufType<ProtobufString>
{
    public string? Value { get; set; }
    public static bool TryCreateInstance(WireType wireType, [NotNullWhen(true)] out ProtobufString? value)
    {
        if (wireType is not WireType.LengthDelimited)
        {
            value = null;
            return false;
        }
        value = new();
        return true;
    }
    public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, WireType wireType, [NotNullWhen(true)] out ProtobufString? value, out ParseStatus status)
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
        try
        {
            Value = reader.ReadString();
            status = ParseStatus.Success;
            return true;
        }
        catch (EndOfStreamException)
        {
            status = ParseStatus.EndOfStream;
            return false;
        }
        catch
        {
            status = ParseStatus.InvalidData;
            return false;
        }
    }
    public override void ReadProtobuf<TReader>(ref TReader reader, WireType receivedWireType)
    {
        if (receivedWireType is not WireType.LengthDelimited)
            throw new InvalidDataException();
        Value = reader.ReadString();
    }
    public override void WriteProtobuf<TWriter>(ref TWriter writer, int index)
    {
        ProtobufUtility.WriteTag(ref writer, index, WireType.LengthDelimited);
        writer.WriteString(Value);
    }
    public override string ToString()
    {
        return $"{{String, Length = {Value?.Length ?? 0}, {LimitedString(Value, 32)}}}";
    }
    public static string LimitedString(ReadOnlySpan<char> chars, int limit)
    {
        StringBuilder sb = new();
        sb.Append("b\"");
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == limit)
            {
                return sb.Append($"\" ... and {chars.Length - limit} more").ToString();
            }
            char c = chars[i];
            switch (c)
            {
                case '\0':
                    sb.Append("\\0");
                    break;
                case '\a':
                    sb.Append("\\a");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\e':
                    sb.Append("\\e");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\v':
                    sb.Append("\\v");
                    break;
                case '"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                default:
                    if (char.IsControl(c))
                        sb.Append($"\\u{(int)c:X4}");
                    else
                        sb.Append(c);
                    break;
            }
        }
        return sb.Append('"').ToString();
    }
}