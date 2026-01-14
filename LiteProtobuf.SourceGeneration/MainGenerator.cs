using Microsoft.CodeAnalysis;
using System;

namespace Myitian.LiteProtobuf.SourceGeneration;

[Generator]
internal class MainGenerator : IIncrementalGenerator
{
    public const string Global = "global::";
    public const string N_DefaultTryCreateInstanceAttribute = $"Default{nameof(Defaults.TryCreateInstance)}{nameof(Attribute)}";
    public const string N_DefaultCreateInstanceAttribute = $"Default{nameof(Defaults.CreateInstance)}{nameof(Attribute)}";
    public const string N_DefaultTryCreateFulfilledAttribute = $"Default{nameof(Defaults.TryCreateFulfilled)}{nameof(Attribute)}";
    public const string N_DefaultCreateFulfilledAttribute = $"Default{nameof(Defaults.CreateFulfilled)}{nameof(Attribute)}";
    public const string N_GeneratedProtobufTypeSerializerAttribute = $"{nameof(GeneratedProtobufTypeSerializer)}{nameof(Attribute)}";
    public const string N_ProtobufFieldAttribute = $"ProtobufField{nameof(Attribute)}";

    public const string NS_Myitian = nameof(Myitian);
    public const string NS_Myitian_LiteProtobuf = $"{NS_Myitian}.{nameof(LiteProtobuf)}";
    public const string NS_Myitian_LiteProtobuf_Serialization = $"{NS_Myitian_LiteProtobuf}.Serialization";
    public const string NS_Myitian_LiteProtobuf_SourceGeneration = $"{NS_Myitian_LiteProtobuf}.{nameof(SourceGeneration)}";
    public const string NS_System = nameof(System);
    public const string NS_System_Diagnostics = $"{NS_System}.{nameof(System.Diagnostics)}";
    public const string NS_System_Diagnostics_CodeAnalysis = $"{NS_System_Diagnostics}.{nameof(System.Diagnostics.CodeAnalysis)}";

    public const string FQ_DefaultTryCreateInstanceAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_DefaultTryCreateInstanceAttribute}";
    public const string FQ_DefaultCreateInstanceAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_DefaultCreateInstanceAttribute}";
    public const string FQ_DefaultTryCreateFulfilledAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_DefaultTryCreateFulfilledAttribute}";
    public const string FQ_DefaultCreateFulfilledAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_DefaultCreateFulfilledAttribute}";
    public const string FQ_GeneratedProtobufTypeSerializerAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_GeneratedProtobufTypeSerializerAttribute}";
    public const string FQ_ProtobufFieldAttribute = $"{NS_Myitian_LiteProtobuf_SourceGeneration}.{N_ProtobufFieldAttribute}";

    public const string FQG_NotNullWhenAttribute = $"{Global}{NS_System_Diagnostics_CodeAnalysis}.NotNullWhenAttribute";
    public const string FQG_IStructBinaryReader = $"{Global}{NS_Myitian_LiteProtobuf}.IStructBinaryReader";
    public const string FQG_IClassBinaryReader = $"{Global}{NS_Myitian_LiteProtobuf}.IClassBinaryReader";
    public const string FQG_ParseStatus = $"{Global}{NS_Myitian_LiteProtobuf}.ParseStatus";
    public const string FQG_WireType = $"{Global}{NS_Myitian_LiteProtobuf}.WireType";
    public const string FQG_Defaults = $"{Global}{NS_Myitian_LiteProtobuf}.Defaults";
    public const string FQG_FieldInfo = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.FieldInfo";
    public const string FQG_SerializationOptions = $"{Global}{NS_Myitian_LiteProtobuf_Serialization}.SerializationOptions";
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // new System.Threading.Thread(() => System.Threading.Thread.Sleep(100000)) { IsBackground = false }.Start(); // Keep console not to close
        Defaults.RegisterSourceOutput(context, Defaults.TryCreateInstance.Instance);
        Defaults.RegisterSourceOutput(context, Defaults.CreateInstance.Instance);
        Defaults.RegisterSourceOutput(context, Defaults.TryCreateFulfilled.Instance);
        Defaults.RegisterSourceOutput(context, Defaults.CreateFulfilled.Instance);
        GeneratedProtobufTypeSerializer.RegisterSourceOutput(context);
    }
}