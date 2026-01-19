using Microsoft.CodeAnalysis;

namespace Myitian.LiteProtobuf.SourceGeneration;

[Generator]
internal class MainGenerator : IIncrementalGenerator
{
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