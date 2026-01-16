using Microsoft.CodeAnalysis;
using System;

namespace Myitian.LiteProtobuf.SourceGeneration;

[Generator]
internal class MainGenerator : IIncrementalGenerator
{
    public const string Global = "global::";
    public const string N_GeneratedDefaultImplementationAttribute = $"{nameof(GeneratedDefaultImplementation)}{nameof(Attribute)}";
    public const string N_GeneratedProtobufTypeSerializerAttribute = $"{nameof(GeneratedProtobufTypeSerializer)}{nameof(Attribute)}";
    public const string N_ProtobufFieldAttribute = $"ProtobufField{nameof(Attribute)}";

    public const string NS_Myitian = nameof(Myitian);
    public const string NS_Myitian_LiteProtobuf = $"{NS_Myitian}.{nameof(LiteProtobuf)}";
    public const string NS_Myitian_LiteProtobuf_Serialization = $"{NS_Myitian_LiteProtobuf}.Serialization";
    public const string NS_Myitian_LiteProtobuf_SourceGeneration = $"{NS_Myitian_LiteProtobuf}.{nameof(SourceGeneration)}";
    public const string NS_System = nameof(System);
    public const string NS_System_Diagnostics = $"{NS_System}.{nameof(System.Diagnostics)}";
    public const string NS_System_Diagnostics_CodeAnalysis = $"{NS_System_Diagnostics}.{nameof(System.Diagnostics.CodeAnalysis)}";

    public const string FQ_GeneratedDefaultImplementationAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_GeneratedDefaultImplementationAttribute}";
    public const string FQ_GeneratedProtobufTypeSerializerAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_GeneratedProtobufTypeSerializerAttribute}";
    public const string FQ_ProtobufFieldAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_ProtobufFieldAttribute}";

    public const string FQG_NotNullWhenAttribute = $"{Global}{NS_System_Diagnostics_CodeAnalysis}.NotNullWhenAttribute";
    public const string FQG_IStructBinaryReader = $"{Global}{NS_Myitian_LiteProtobuf}.IStructBinaryReader";
    public const string FQG_IClassBinaryReader = $"{Global}{NS_Myitian_LiteProtobuf}.IClassBinaryReader";
    public const string FQG_ParseStatus = $"{Global}{NS_Myitian_LiteProtobuf}.ParseStatus";
    public const string FQG_WireType = $"{Global}{NS_Myitian_LiteProtobuf}.WireType";
    public const string FQG_FieldInfo = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.FieldInfo";
    public const string FQG_SerializationOptions = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.SerializationOptions";
    public const string FQG_DefaultImplementation = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.DefaultImplementation";

    public static readonly SymbolDisplayFormat DeclarationFormat = new(
        SymbolDisplayGlobalNamespaceStyle.Omitted,
        SymbolDisplayTypeQualificationStyle.NameOnly,
        SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);
    public static readonly SymbolDisplayFormat NullableFullyQualifiedFormat = SymbolDisplayFormat.FullyQualifiedFormat
        .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // new System.Threading.Thread(static () => System.Threading.Thread.Sleep(100000)) { IsBackground = false }.Start(); // Keep console not to close to check the console output
        GeneratedDefaultImplementation.RegisterSourceOutput(context);
        GeneratedProtobufTypeSerializer.RegisterSourceOutput(context);
    }
}