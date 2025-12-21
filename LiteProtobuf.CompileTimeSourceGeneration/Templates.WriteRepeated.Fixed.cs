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
                                long totalSize = value.Length * ({2}L / 8);
                
                """;
            public string IEnumerable => """
                    {{
                        int count = -1;
                        switch (repeatedEncoding)
                        {{
                            case RepeatedEncoding.Auto:
                                if (value.TryGetNonEnumeratedCount(out count))
                                    goto case RepeatedEncoding.Packed;
                                else
                                    goto case RepeatedEncoding.NonPacked;
                            case RepeatedEncoding.Packed when count < 0:
                                count = value.Count();
                                goto case RepeatedEncoding.Packed;
                            case RepeatedEncoding.Packed:
                                long totalSize = count * ({2}L / 8);
                
                """;
            public string Common => """
                                WriteTag(ref writer, index, WireType.LengthDelimited);
                                writer.WriteVarInt(totalSize);
                                foreach ({0} it in value)
                                    writer.Write{1}(it);
                                break;
                            case RepeatedEncoding.NonPacked:
                                foreach ({0} it in value)
                                {{
                                    WriteTag(ref writer, index, WireType.{1});
                                    writer.Write{1}(it);
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