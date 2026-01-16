namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

partial class Repeated
{
    partial class Write
    {
        interface IHandler
        {
            string Keyword { get; }
            string ReadOnlySpan { get; }
            string IEnumerable { get; }
            string Body { get; }
        }
        sealed class VarInt : IHandler
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
            public string Body => """
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
        sealed class Fixed : IHandler
        {
            public static readonly Fixed Instance = new();

            public string Keyword => nameof(Fixed);
            public string ReadOnlySpan => """
                switch (repeatedEncoding)
                {{
                    case RepeatedEncoding.Auto:
                    case RepeatedEncoding.Packed:
                        long totalSize = value.Length * ({4}L / 8);
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
                        long totalSize = count * ({4}L / 8);
                """;
            public string Body => """
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
        sealed class Bool : IHandler
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
            public string Body => """
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