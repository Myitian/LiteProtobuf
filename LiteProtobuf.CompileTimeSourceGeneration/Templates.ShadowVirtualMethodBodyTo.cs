using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public partial class Templates
{
    public class ShadowVirtualMethodBodyTo
    {
        public static void Apply(SourceProductionContext context, Model model)
        {
            if (model.Self is null || model.Target is null)
                return;
            StringBuilder sb = new();
            string us = model.Usings;
            if (!SymbolEqualityComparer.Default.Equals(model.Self.ContainingNamespace, model.Target.ContainingNamespace))
            {
                string? u = model.Self.ContainingNamespace?.ToDisplayString();
                if (!string.IsNullOrEmpty(u))
                    us = $@"using {u};
{us}";
            }
            int depth = UtilityGenerator.CreateCSharpCode(sb, us, model.Target);
            foreach (IMethodSymbol m in model.Self
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(it => it.IsVirtual))
            {
                sb.Append(' ', depth * 4).AppendLine(new VirtualMethodToStaticConverter(
                    model.TypeParameter,
                    model.Parameter,
                    model.Self.Name,
                    depth)
                    .Visit(m.DeclaringSyntaxReferences[0].GetSyntax())
                    .ToString());
            }
            while (depth > 0)
            {
                depth--;
                sb.Append(' ', depth * 4)
                    .AppendLine("}");
            }
            string code = sb.ToString();
            context.AddSource($"{model.Self.Name}.{model.Target.Name}.g.cs", code);
        }

        public readonly record struct Model
        {
            public bool IsValid { get; } = false;
            public INamedTypeSymbol? Target { get; } = null;
            public INamedTypeSymbol? Self { get; } = null;
            public string TypeParameter { get; } = "";
            public string Parameter { get; } = "";
            public string Usings { get; } = "";

            public Model(GeneratorAttributeSyntaxContext context)
            {
                if (context.TargetSymbol is not INamedTypeSymbol i
                    || context.Attributes is not [
                        {
                            ConstructorArguments: [
                            {
                                Kind: TypedConstantKind.Type,
                                Value: INamedTypeSymbol target
                            },
                            {
                                Kind: TypedConstantKind.Primitive,
                                Value: string typeParameter
                            },
                            {
                                Kind: TypedConstantKind.Primitive,
                                Value: string parameter
                            }]
                        }])
                    return;

                for (SyntaxNode? node = context.TargetNode; node is not null; node = node.Parent)
                {
                    if (node is CompilationUnitSyntax cus)
                    {
                        Usings = cus.Usings.ToString();
                        break;
                    }
                }
                TypeParameter = typeParameter;
                Parameter = parameter;
                Self = i;
                Target = target;
                IsValid = true;
            }
        }
    }
}