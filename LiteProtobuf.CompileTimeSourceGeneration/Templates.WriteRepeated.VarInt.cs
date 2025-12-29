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
                                if (value is ICollection<{1}>)
                                    goto case RepeatedEncoding.Packed;
                                else
                                    goto case RepeatedEncoding.NonPacked;
                
                """;
            public string Common => """
                            case RepeatedEncoding.Packed:
                                long totalSize = Count{2}Size(value);
                                WriteTag({0}writer, index, WireType.LengthDelimited);
                                writer.WriteVarInt(totalSize);
                                foreach ({1} it in value)
                                    writer.Write{2}(it);
                                break;
                            case RepeatedEncoding.NonPacked:
                                foreach ({1} it in value)
                                {{
                                    WriteTag({0}writer, index, WireType.VarInt);
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