using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

partial class RepeatedUtility
{
    static class TryRead
    {
        const string Signature = """
            public static bool TryReadRepeated{1}<TReader{3}>({6}TReader reader, WireType wireType, ICollection<{2}> destination, out ParseStatus status)
            """;
        const string Body = """
            switch (wireType)
            {{
                case WireType.{0}:
                    if (!reader.TryRead{1}(out {2} value, out status))
                        return false;
                    destination.Add(value);
                    return true;
                case WireType.LengthDelimited:
                    if (!TReader.TryCreateLengthDelimitedReader({5}reader, out TReader{4} subReader, out status))
                    {{
                        subReader{4}.Dispose();
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
        public static void Generate(IncrementalGeneratorPostInitializationContext context)
        {
            IndentedTextWriter writer = new(new StringWriter());
            writer.WriteLine(SharedHeader);
            using (writer.CodeBlock())
            {
                // <list type="bullet">
                // <item><c>{0}</c>: base mode</item>
                // <item><c>{1}</c>: mode</item>
                // <item><c>{2}</c>: type param</item>
                // <item><c>{3}</c>: type param with comma</item>
                // <item><c>{4}</c>: private use (<c>?</c>)</item>
                // <item><c>{5}</c>: private use (<see langword="ref"/>)</item>
                // <item><c>{6}</c>: private use (<see langword="scoped ref"/>)</item>
                // </list>
                object[] formatArgs = new object[7];
                foreach (InternalGenerator.Model model in InternalGenerator.Models)
                {
                    formatArgs[0] = model.BaseMode;
                    formatArgs[1] = model.Mode;
                    formatArgs[2] = model.TypeParam;
                    formatArgs[3] = model.Constraint is null ? "" : $", {model.TypeParam}";
                    GenerateCore(writer, in model, formatArgs, true);
                    GenerateCore(writer, in model, formatArgs, false);
                }
            }
            string code = writer.InnerWriter.ToString();
            context.AddSource("RepeatedUtility.TryReadRepeated.g.cs", code);
        }
        public static void GenerateCore(
            IndentedTextWriter writer,
            in InternalGenerator.Model model,
            object[] formatArgs,
            bool isValueType)
        {
            formatArgs[4] = isValueType ? "" : "?";
            formatArgs[5] = isValueType ? "ref " : "";
            formatArgs[6] = isValueType ? "scoped ref " : "";
            writer.WriteLine(Signature, formatArgs);
            using (writer.IndentedBlock())
            {
                if (isValueType)
                    writer.WriteLine("where TReader : struct, IStructBinaryReader<TReader>, allows ref struct", formatArgs);
                else
                    writer.WriteLine("where TReader : class, IClassBinaryReader<TReader>", formatArgs);
                if (!string.IsNullOrEmpty(model.Constraint))
                    writer.WriteLine(model.Constraint, formatArgs);
            }
            using (writer.CodeBlock())
            {
                writer.WriteLines(Body, formatArgs);
            }
        }
    }
}