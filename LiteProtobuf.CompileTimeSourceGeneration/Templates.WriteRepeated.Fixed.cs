namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public partial class Templates
{
    public partial class WriteRepeated
    {
        public class Fixed : IType
        {
            public static readonly Fixed Instance = new();

            public string Keyword => nameof(Fixed);
            public string ReadOnlySpan => """
                    {{
                        switch (repeatedEncoding)
                        {{
                            case RepeatedEncoding.Auto:
                            case RepeatedEncoding.Packed:
                                long totalSize = value.Length * ({3}L / 8);
                
                """;
            public string IEnumerable => """
                    {{
                        int count = -1;
                        switch (repeatedEncoding)
                        {{
                            case RepeatedEncoding.Auto:
                                if (value.TryGetNonEnumeratedCount(out count))
                                    goto case RepeatedEncoding.Packed;
                                else if (value is not IReadOnlyCollection<{1}> ro)
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
                                long totalSize = count * ({3}L / 8);
                
                """;
            public string Common => """
                                writer.WriteTag(index, WireType.LengthDelimited);
                                writer.WriteVarInt(totalSize);
                                foreach ({1} it in value)
                                    writer.Write{2}(it);
                                break;
                            case RepeatedEncoding.NonPacked:
                                foreach ({1} it in value)
                                {{
                                    writer.WriteTag(index, WireType.{2});
                                    writer.Write{2}(it);
                                }}
                                break;
                            default:
                                throw new ArgumentException("Invalid repeatedEncoding", nameof(repeatedEncoding));
                        }}
                    }}
                
                """;
        }
    }
}