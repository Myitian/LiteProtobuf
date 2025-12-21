using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public partial class Templates
{
    public class ReadRepeated
    {
        public static void Apply(SourceProductionContext context, Model model)
        {
            StringBuilder sb = new();
            sb.AppendLine(Header).Append(' ', 4).AppendLine(model.Sign);
            string wireType = model.Mode == "VarIntZigZag" ? "VarInt" : model.Mode;
            sb.AppendFormat(Common, model.Mode, wireType);
            string code = sb.Append(Footer).ToString();
            context.AddSource($"ProtobufUtility.{model.Name}.g.cs", code);
        }

        public const string Common = """
                {{
                    switch (wireType)
                    {{
                        case WireType.{1}:
                            if (!reader.TryRead{0}(out T value, out status))
                                return false;
                            destination.Add(value);
                            return true;
                        case WireType.LengthDelimited:
                            if (!TReader.TryCreateLengthDelimitedReader(ref reader, out TReader? subReader, out status))
                            {{
                                subReader?.Dispose();
                                return false;
                            }}
                            using (subReader)
                            {{
                                ParseStatus subStatus;
                                while (subReader.TryRead{0}(out value, out subStatus))
                                    destination.Add(value);
                                if (subStatus is ParseStatus.ExactEndOfStream)
                                    return true;
                            }}
                            goto default;
                        default:
                            status = ParseStatus.InvalidData;
                            return false;
                    }}
                }}
                
                """;

        public readonly record struct Model
        {
            public bool IsValid { get; } = false;
            public string Mode { get; } = "";
            public string Name { get; } = "";
            public string Sign { get; } = "";

            public Model(GeneratorAttributeSyntaxContext context)
            {
                if (context.TargetNode is not MethodDeclarationSyntax m
                    || context.Attributes is not [
                        {
                            ConstructorArguments: [
                                {
                                    Kind: not TypedConstantKind.Array,
                                    Value: string mode
                                }]
                        }])
                    return;

                Mode = mode;
                Name = m.Identifier.Text;
                Sign = SemicolonRemover.Instance.Visit(m).ToString();
                IsValid = true;
            }
        }
    }
}