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
            sb.AppendLine(ProtobufUtilityHeader).Append(' ', 4).AppendLine(model.Sign);
            string wireType = model.Mode == "VarIntZigZag" ? "VarInt" : model.Mode;
            sb.AppendFormat(Common,
                model.Mode, wireType,
                model.IsReaderValueType ? "ref " : "",
                model.IsReaderValueType ? "" : "?");
            string code = sb.Append(ProtobufUtilityFooter).ToString();
            context.AddSource($"ProtobufUtility.{model.Name}.{(model.IsReaderValueType ? 'V' : 'C')}{model.ExtraName}.g.cs", code);
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
                            if (!TReader.TryCreateLengthDelimitedReader({2}reader, out TReader{3} subReader, out status))
                            {{
                                subReader{3}.Dispose();
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
            public bool IsReaderValueType { get; } = false;
            public string ExtraName { get; } = "";

            public Model(GeneratorAttributeSyntaxContext context)
            {
                if (context.TargetNode is not MethodDeclarationSyntax m
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
                            },
                            {
                                Kind: TypedConstantKind.Primitive
                            } extraName]
                        }])
                    return;

                Mode = mode;
                Name = m.Identifier.Text;
                Sign = SemicolonRemover.Instance.Visit(m).ToString();
                ExtraName = extraName.Value as string ?? "";
                IsReaderValueType = isValueType;
                IsValid = true;
            }
        }
    }
}