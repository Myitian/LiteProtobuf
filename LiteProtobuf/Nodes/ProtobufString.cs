using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;
using System.Text;

namespace Myitian.LiteProtobuf.Nodes;

[GeneratedDefaultImplementation(
    TryCreateInstance = true,
    CreateInstance = true,
    TryCreateFulfilled = true,
    CreateFulfilled = true)]
public sealed partial class ProtobufString()
    : ProtobufNode(WireType.LengthDelimited), IProtobufType<ProtobufString>
{
    public string? Value { get; set; }

    public static new bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return fieldInfo.ReceivedWireType is WireType.LengthDelimited;
    }
    protected override bool SharedTryReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        if (!IsFieldInfoValid(fieldInfo, options))
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
    protected override void SharedReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        if (!IsFieldInfoValid(fieldInfo, options))
            throw new InvalidDataException();
        Value = reader.ReadString();
    }
    protected override void SharedWriteProtobuf<TWriter>(scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        writer.WriteString(Value);
    }
    public override string ToString()
    {
        return $"{{String, Length = {Value?.Length ?? 0}, {DisplayLimitedChars(Value, 50)}}}";
    }
    public static string DisplayLimitedChars(ReadOnlySpan<char> chars, int limit)
    {
        StringBuilder sb = new();
        sb.Append('"');
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == limit)
                return sb.Append($"\" ... and {chars.Length - limit} more").ToString();
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