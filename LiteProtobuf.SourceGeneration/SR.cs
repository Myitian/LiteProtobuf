using System;

namespace Myitian.LiteProtobuf.SourceGeneration
{
    internal static class SR
    {
        public const string Global = "global::";

        public const string NS_Myitian = nameof(Myitian);
        public const string NS_Myitian_LiteProtobuf = $"{NS_Myitian}.{nameof(LiteProtobuf)}";
        public const string NS_Myitian_LiteProtobuf_Serialization = $"{NS_Myitian_LiteProtobuf}.Serialization";
        public const string NS_Myitian_LiteProtobuf_SourceGeneration = $"{NS_Myitian_LiteProtobuf}.{nameof(SourceGeneration)}";
        public const string NS_System = nameof(System);
        public const string NS_System_Diagnostics = $"{NS_System}.{nameof(System.Diagnostics)}";
        public const string NS_System_Diagnostics_CodeAnalysis = $"{NS_System_Diagnostics}.{nameof(System.Diagnostics.CodeAnalysis)}";
        public const string NS_System_Runtime = $"{NS_System}.{nameof(System.Runtime)}";

        public const string FQ_GeneratedDefaultImplementationAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{nameof(GeneratedDefaultImplementation)}Attribute";
        public const string FQ_GeneratedProtobufTypeSerializerAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{nameof(GeneratedProtobufTypeSerializer)}Attribute";
        public const string FQ_ProtobufFieldAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.ProtobufFieldAttribute";
        public const string FQ_ProtobufRemainingFieldsAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.ProtobufRemainingFieldsAttribute";
        public const string FQ_ICreatableProtobufType = $"{NS_Myitian_LiteProtobuf_Serialization}.ICreatableProtobufType`1";
        public const string FQ_IReadOnlyProtobufType = $"{NS_Myitian_LiteProtobuf_Serialization}.IReadOnlyProtobufType";
        public const string FQ_IWriteOnlyProtobufType = $"{NS_Myitian_LiteProtobuf_Serialization}.IWriteOnlyProtobufType";
        public const string FQ_NullableT = $"{NS_System}.{nameof(Nullable<>)}`1";

        public const string FQG_NotNullWhenAttribute = $"{Global}{NS_System_Diagnostics_CodeAnalysis}.NotNullWhenAttribute";
        public const string FQG_IStructBinaryReader = $"{Global}{NS_Myitian_LiteProtobuf}.IStructBinaryReader";
        public const string FQG_IClassBinaryReader = $"{Global}{NS_Myitian_LiteProtobuf}.IClassBinaryReader";
        public const string FQG_ParseStatus = $"{Global}{NS_Myitian_LiteProtobuf}.ParseStatus";
        public const string FQG_WireType = $"{Global}{NS_Myitian_LiteProtobuf}.WireType";
        public const string FQG_FieldInfo = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.FieldInfo";
        public const string FQG_SerializationOptions = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.SerializationOptions";
        public const string FQG_DefaultImplementation = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.DefaultImplementation";

        public const string Arg_UnreachableException = "The program executed an instruction that was thought to be unreachable.";
    }
}