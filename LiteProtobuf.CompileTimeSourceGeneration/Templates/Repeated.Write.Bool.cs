namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

public partial class Repeated
{
    public partial class Write
    {
        public class Bool : IType
        {
            public static readonly Bool Instance = new();

            public string Keyword => nameof(Bool);
            public string ReadOnlySpan => """
                int count = value.Length;
                switch (repeatedEncoding)
                {{
                    case RepeatedEncoding.Auto:
                """;
            public string IEnumerable => """
                int count = -1;
                switch (repeatedEncoding)
                {{
                    case RepeatedEncoding.Auto:
                        if (value.TryGetNonEnumeratedCount(out count))
                            goto case RepeatedEncoding.Packed;
                        else if (value is not IReadOnlyCollection<{2}> ro)
                            goto case RepeatedEncoding.NonPacked;
                        else
                        {{
                            count = ro.Count;
                            goto case RepeatedEncoding.Packed;
                        }}
                    case RepeatedEncoding.Packed when count < 0:
                        count = value.Count();
                        goto case RepeatedEncoding.Packed;
                
                """;
            public string Common => """
                    case RepeatedEncoding.Packed:
                        writer.WriteTag(number, WireType.LengthDelimited);
                        writer.WriteVarInt(count);
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