namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

public partial class Repeated
{
    public partial class Write
    {
        public class Fixed : IType
        {
            public static readonly Fixed Instance = new();

            public string Keyword => nameof(Fixed);
            public string ReadOnlySpan => """
                switch (repeatedEncoding)
                {{
                    case RepeatedEncoding.Auto:
                    case RepeatedEncoding.Packed:
                        long totalSize = value.Length * ({6}L / 8);
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
                    case RepeatedEncoding.Packed:
                        long totalSize = count * ({6}L / 8);
                """;
            public string Common => """
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