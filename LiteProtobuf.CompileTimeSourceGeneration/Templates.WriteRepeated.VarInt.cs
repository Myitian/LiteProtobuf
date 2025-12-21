namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public partial class Templates
{
    public partial class WriteRepeated
    {
        public class VarInt : IType
        {
            public static readonly VarInt Instance = new();

            public string Keyword => nameof(VarInt);
            public string ReadOnlySpan => """
                    {{
                        switch (repeatedEncoding)
                        {{
                            case RepeatedEncoding.Auto:
                
                """;
            public string IEnumerable => """
                    {{
                        switch (repeatedEncoding)
                        {{
                            case Myitian.LiteProtobuf.RepeatedEncoding.Auto:
                                if (value is ICollection<{0}>)
                                    goto case RepeatedEncoding.Packed;
                                else
                                    goto case RepeatedEncoding.NonPacked;
                
                """;
            public string Common => """
                            case RepeatedEncoding.Packed:
                                long totalSize = Count{1}Size(value);
                                WriteTag(ref writer, index, WireType.LengthDelimited);
                                writer.WriteVarInt(totalSize);
                                foreach ({0} it in value)
                                    writer.Write{1}(it);
                                break;
                            case RepeatedEncoding.NonPacked:
                                foreach ({0} it in value)
                                {{
                                    WriteTag(ref writer, index, WireType.VarInt);
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