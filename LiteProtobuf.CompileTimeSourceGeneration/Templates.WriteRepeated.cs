using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public partial class Templates
{
    public partial class WriteRepeated
    {
        private static readonly IType[] types = [
            VarInt.Instance,
            Fixed.Instance,
            Bool.Instance
        ];

        public static void Apply(SourceProductionContext context, Model model)
        {
            StringBuilder sb = new();
            sb.AppendLine(ProtobufUtilityHeader).Append(' ', 4).AppendLine(model.Sign);
            foreach (IType type in types)
            {
                if (model.Mode.StartsWith(type.Keyword))
                {
                    object[] args = [
                        model.IsValueType ? "ref " : "",
                        model.ElementType,
                        model.Mode,
                        model.Mode[type.Keyword.Length..]];
                    switch (model.ContainerType)
                    {
                        case nameof(ReadOnlySpan<>):
                            sb.AppendFormat(type.ReadOnlySpan, args);
                            break;
                        case nameof(IEnumerable<>):
                            sb.AppendFormat(type.IEnumerable, args);
                            break;
                        default:
                            return;
                    }
                    sb.AppendFormat(type.Common, args);
                    string code = sb.Append(ProtobufUtilityFooter).ToString();
                    context.AddSource($"ProtobufUtility.{model.Name}.{model.ContainerType}-{model.ElementType}.{(model.IsValueType ? 'V' : 'C')}.g.cs", code);
                    return;
                }
            }
            return;
        }

        public interface IType
        {
            string Keyword { get; }
            string ReadOnlySpan { get; }
            string IEnumerable { get; }
            string Common { get; }
        }

        public readonly record struct Model
        {
            public bool IsValid { get; } = false;
            public string ContainerType { get; } = "";
            public string ElementType { get; } = "";
            public string Mode { get; } = "";
            public string Name { get; } = "";
            public string Sign { get; } = "";
            public bool IsValueType { get; } = false;

            public Model(GeneratorAttributeSyntaxContext context)
            {
                if (context.TargetNode is not MethodDeclarationSyntax m
                    || m?.ParameterList
                        .Parameters
                        .FirstOrDefault(it => it.Identifier.Text == "value")
                        ?.Type is not GenericNameSyntax
                        {
                            Identifier.Text: string containerType,
                            TypeArgumentList.Arguments: [TypeSyntax elementType]
                        }
                    || context.Attributes is not [
                        {
                            ConstructorArguments: [
                            {
                                Kind: TypedConstantKind.Primitive,
                                Value: string mode
                            },
                            {
                                Kind: TypedConstantKind.Primitive,
                                Value: bool isValueType
                            }]
                        }])
                    return;

                ContainerType = containerType;
                ElementType = elementType.ToString();
                Mode = mode;
                Name = m.Identifier.Text;
                Sign = SemicolonRemover.Instance.Visit(m).ToString();
                IsValueType = isValueType;
                IsValid = true;
            }
        }
    }
}