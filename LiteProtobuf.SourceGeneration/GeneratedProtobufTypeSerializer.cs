using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace Myitian.LiteProtobuf.SourceGeneration;

static class GeneratedProtobufTypeSerializer
{
    public static void RegisterSourceOutput(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: MainGenerator.FQ_GeneratedProtobufTypeSerializerAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is BaseTypeDeclarationSyntax,
            transform: static (context, _) => context.TargetSymbol as INamedTypeSymbol),
            Apply);
    }
    public static void Apply(SourceProductionContext context, INamedTypeSymbol? self)
    {
        if (self is null)
            return;
        // TODO: in dev
        var m = self.GetMembers();
        var a = m[0].GetAttributes();
        StringBuilder sb = new();
    }
}