namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public partial class Templates
{
    public partial class WriteRepeated
    {
        public class Bool : IType
        {
            public static readonly Bool Instance = new();

            public string Keyword => nameof(Bool);
            public string ReadOnlySpan => """
                    {{
                        switch (repeatedEncoding)
                        {{
                            case RepeatedEncoding.Auto:
                            case RepeatedEncoding.Packed:
                                writer.WriteTag(number, WireType.LengthDelimited);
                                writer.WriteVarInt(value.Length);
                
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
                                writer.WriteTag(number, WireType.LengthDelimited);
                                writer.WriteVarInt(count);
                
                """;
            public string Common => """
                                foreach ({1} it in value)
                                    writer.Write{2}(it);
                                break;
                            case RepeatedEncoding.NonPacked:
                                foreach ({1} it in value)
                                {{
                                    writer.WriteTag(number, WireType.VarInt);
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