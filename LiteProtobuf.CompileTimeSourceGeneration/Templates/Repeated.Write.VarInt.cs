namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

public partial class Repeated
{
    public partial class Write
    {
        public class VarInt : IType
        {
            public static readonly VarInt Instance = new();

            public string Keyword => nameof(VarInt);
            public string ReadOnlySpan => """
                switch (repeatedEncoding)
                {{
                    case RepeatedEncoding.Auto:
                """;
            public string IEnumerable => """
                switch (repeatedEncoding)
                {{
                    case RepeatedEncoding.Auto:
                        if (value is ICollection<{2}> or IReadOnlyCollection<{2}>)
                            goto case RepeatedEncoding.Packed;
                        else
                            goto case RepeatedEncoding.NonPacked;
                """;
            public string Common => """
                    case RepeatedEncoding.Packed:
                        long totalSize = Count{1}Size(value);
                        writer.WriteTag(number, WireType.LengthDelimited);
                        writer.WriteVarInt(totalSize);
                        foreach ({2} it in value)
                            writer.Write{1}(it);
                        break;
                    case RepeatedEncoding.NonPacked:
                        foreach ({2} it in value)
                        {{
                            writer.WriteTag(number, WireType.{0});
                            writer.Write{1}(it);
                        }}
                        break;
                    default:
                        throw new ArgumentException($"Invalid value: {{repeatedEncoding}}", nameof(repeatedEncoding));
                }}
                """;
        }
    }
}