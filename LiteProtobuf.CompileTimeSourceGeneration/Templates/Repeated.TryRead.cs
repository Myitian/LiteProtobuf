using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

public partial class Repeated
{
    public class TryRead
    {
        public static void Apply(IncrementalGeneratorPostInitializationContext context)
        {
            IndentedTextWriter writer = new(new StringWriter());
            writer.WriteLine(ProtobufUtilityHeader);
            using (writer.CodeBlock())
            {
                foreach (Model model in Models)
                {
                    ApplyCore(writer, model, true);
                    ApplyCore(writer, model, false);
                }
            }
            string code = writer.InnerWriter.ToString();
            context.AddSource("ProtobufUtility.TryReadRepeated.g.cs", code);
        }
        public static void ApplyCore(IndentedTextWriter writer, Model model, bool isValueType)
        {
            // <list type="bullet">
            // <item><c>{0}</c>: base mode</item>
            // <item><c>{1}</c>: mode</item>
            // <item><c>{2}</c>: type param</item>
            // <item><c>{3}</c>: type param with comma</item>
            // <item><c>{4}</c>: scoped ref</item>
            // <item><c>{5}</c>: private use (ref)</item>
            // <item><c>{6}</c>: private use (nullability mark)</item>
            // </list>
            using PooledArrayHandle<object> formatArgs = new(7);
            formatArgs.Array[0] = model.BaseMode;
            formatArgs.Array[1] = model.Mode;
            formatArgs.Array[2] = model.TypeParam;
            formatArgs.Array[3] = model.Constraint is null ? "" : $"{model.TypeParam}, ";
            formatArgs.Array[4] = isValueType ? "scoped ref " : "";
            formatArgs.Array[5] = isValueType ? "ref " : "";
            formatArgs.Array[6] = isValueType ? "" : "?";

            writer.WriteLine(Sign, formatArgs.Array);
            using (writer.IndentedBlock())
            {
                if (!string.IsNullOrEmpty(model.Constraint))
                    writer.WriteLine(model.Constraint, formatArgs);
                if (isValueType)
                    writer.WriteLine("where TReader : struct, IStructBinaryReader<TReader>, allows ref struct", formatArgs);
                else
                    writer.WriteLine("where TReader : class, IClassBinaryReader<TReader>", formatArgs);
            }
            using (writer.CodeBlock())
            {
                writer.WriteLines(Common, formatArgs.Array);
            }
        }

        public const string Sign = "public static bool TryReadRepeated{1}<{3}TReader>({4}TReader reader, WireType wireType, ICollection<{2}> destination, out ParseStatus status)";

        public const string Common = """
            switch (wireType)
            {{
                case WireType.{0}:
                    if (!reader.TryRead{1}(out {2} value, out status))
                        return false;
                    destination.Add(value);
                    return true;
                case WireType.LengthDelimited:
                    if (!TReader.TryCreateLengthDelimitedReader({5}reader, out TReader{6} subReader, out status))
                    {{
                        subReader{6}.Dispose();
                        return false;
                    }}
                    try
                    {{
                        ParseStatus subStatus;
                        while (subReader.TryRead{1}(out value, out subStatus))
                            destination.Add(value);
                        if (subStatus is ParseStatus.ExactEndOfStream)
                            return true;
                    }}
                    finally
                    {{
                        subReader.Dispose();
                    }}
                    goto default;
                default:
                    status = ParseStatus.InvalidData;
                    return false;
            }}
            """;

    }
}