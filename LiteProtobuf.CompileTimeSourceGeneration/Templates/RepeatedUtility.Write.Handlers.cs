namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

partial class RepeatedUtility
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
                switch (isPacked)
                {{
                    case null:
                """;
            public string IEnumerable => """
                switch (isPacked)
                {{
                    case null:
                        if (value is ICollection<{2}> or IReadOnlyCollection<{2}>)
                            goto case true;
                        else
                            goto case false;
                """;
            public string Body => """
                    case true:
                        long totalSize = ProtobufUtility.Count{1}Size(value);
                        writer.WriteTag(number, WireType.LengthDelimited);
                        writer.WriteVarInt(totalSize);
                        foreach ({2} it in value)
                            writer.Write{1}(it);
                        break;
                    case false:
                        foreach ({2} it in value)
                        {{
                            writer.WriteTag(number, WireType.{0});
                            writer.Write{1}(it);
                        }}
                        break;
                }}
                """;
        }
        sealed class Fixed : IHandler
        {
            public static readonly Fixed Instance = new();

            public string Keyword => nameof(Fixed);
            public string ReadOnlySpan => """
                switch (isPacked)
                {{
                    case null:
                    case true:
                        long totalSize = value.Length * ({4}L / 8);
                """;
            public string IEnumerable => """
                int count = -1;
                switch (isPacked)
                {{
                    case null:
                        if (value.TryGetNonEnumeratedCount(out count))
                            goto case true;
                        else if (value is not IReadOnlyCollection<{2}> ro)
                            goto case false;
                        else
                        {{
                            count = ro.Count;
                            goto case true;
                        }}
                    case true when count < 0:
                        count = value.Count();
                        goto case true;
                    case true:
                        long totalSize = count * ({4}L / 8);
                """;
            public string Body => """
                        writer.WriteTag(number, WireType.LengthDelimited);
                        writer.WriteVarInt(totalSize);
                        foreach ({2} it in value)
                            writer.Write{1}(it);
                        break;
                    case false:
                        foreach ({2} it in value)
                        {{
                            writer.WriteTag(number, WireType.{0});
                            writer.Write{1}(it);
                        }}
                        break;
                }}
                """;
        }
        sealed class Bool : IHandler
        {
            public static readonly Bool Instance = new();

            public string Keyword => nameof(Bool);
            public string ReadOnlySpan => """
                int count = value.Length;
                switch (isPacked)
                {{
                    case null:
                """;
            public string IEnumerable => """
                int count = -1;
                switch (isPacked)
                {{
                    case null:
                        if (value.TryGetNonEnumeratedCount(out count))
                            goto case true;
                        else if (value is not IReadOnlyCollection<{2}> ro)
                            goto case false;
                        else
                        {{
                            count = ro.Count;
                            goto case true;
                        }}
                    case true when count < 0:
                        count = value.Count();
                        goto case true;
                
                """;
            public string Body => """
                    case true:
                        writer.WriteTag(number, WireType.LengthDelimited);
                        writer.WriteVarInt(count);
                        foreach ({2} it in value)
                            writer.Write{1}(it);
                        break;
                    case false:
                        foreach ({2} it in value)
                        {{
                            writer.WriteTag(number, WireType.{0});
                            writer.Write{1}(it);
                        }}
                        break;
                }}
                """;
        }
    }
}