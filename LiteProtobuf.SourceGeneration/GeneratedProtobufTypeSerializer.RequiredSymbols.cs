using Microsoft.CodeAnalysis;

namespace Myitian.LiteProtobuf.SourceGeneration;

static partial class GeneratedProtobufTypeSerializer
{
    readonly record struct RequiredSymbols
    {
        public readonly Compilation Compilation;
        public readonly INamedTypeSymbol? ProtobufFieldAttribute;
        public readonly INamedTypeSymbol? ProtobufRemainingFieldsAttribute;
        public RequiredSymbols(Compilation compilation)
        {
            Compilation = compilation;
            ProtobufFieldAttribute = compilation.GetTypeByMetadataAndAssemblyName(
                SR.FQ_ProtobufFieldAttribute,
                SR.NS_Myitian_LiteProtobuf);
            ProtobufRemainingFieldsAttribute = compilation.GetTypeByMetadataAndAssemblyName(
                SR.FQ_ProtobufRemainingFieldsAttribute,
                SR.NS_Myitian_LiteProtobuf);
        }
        public bool IsProtobufFieldAttribute(AttributeData attribute)
        {
            return SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, ProtobufFieldAttribute);
        }
        public bool IsProtobufRemainingFieldsAttribute(AttributeData attribute)
        {
            return SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, ProtobufRemainingFieldsAttribute);
        }
    }
}
