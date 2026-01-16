using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

partial class Repeated
{
    static class Read
    {
        const string Signature = """
            public static void ReadRepeated{1}<{3}TReader>({6}TReader reader, WireType wireType, ICollection<{2}> destination)
            """;
        const string Body = """
            switch (wireType)
            {{
                case WireType.{0}:
                    {2} value = reader.Read{1}{4}();
                    destination.Add(value);
                    return;
                case WireType.LengthDelimited:
                    using (TReader subReader = TReader.CreateLengthDelimitedReader({5}reader))
                    {{
                        ParseStatus subStatus;
                        while (subReader.TryRead{1}(out value, out subStatus))
                            destination.Add(value);
                        if (subStatus is ParseStatus.ExactEndOfStream)
                            return;
                        else
                            throw IBinaryReader.GetExceptionByStatus(subStatus);
                    }}
                default:
                    throw IBinaryReader.GetExceptionByStatus(ParseStatus.InvalidData);
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
                // <item><c>{4}</c>: private use (<c>&lt;T&gt;</c>)</item>
                // <item><c>{5}</c>: private use (<see langword="ref"/>)</item>
                // <item><c>{6}</c>: private use (<see langword="scoped ref"/>)</item>
                // </list>
                using PooledArrayHandle<object> formatArgs = new(7);
                foreach (InternalGenerator.Model model in InternalGenerator.Models)
                {
                    formatArgs.Array[0] = model.BaseMode;
                    formatArgs.Array[1] = model.Mode;
                    formatArgs.Array[2] = model.TypeParam;
                    formatArgs.Array[3] = model.Constraint is null ? "" : $"{model.TypeParam}, ";
                    formatArgs.Array[4] = model.Constraint is null ? "" : $"<{model.TypeParam}>";
                    GenerateCore(writer, in model, formatArgs.Array, true);
                    GenerateCore(writer, in model, formatArgs.Array, false);
                }
            }
            string code = writer.InnerWriter.ToString();
            context.AddSource("ProtobufUtility.ReadRepeated.g.cs", code);
        }
        static void GenerateCore(
            IndentedTextWriter writer,
            in InternalGenerator.Model model,
            object[] formatArgs,
            bool isValueType)
        {
            formatArgs[5] = isValueType ? "ref " : "";
            formatArgs[6] = isValueType ? "scoped ref " : "";
            writer.WriteLine(Signature, formatArgs);
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
                writer.WriteLines(Body, formatArgs);
            }
        }
    }
}