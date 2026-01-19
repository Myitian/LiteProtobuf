using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Frozen;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

partial class RepeatedUtility
{
    static partial class Write
    {
        const string Signature = """
            public static void WriteRepeated{1}<TWriter{3}>({6}TWriter writer, int number, {5}<{2}> value, bool? isPacked = null)
            """;
        static readonly FrozenDictionary<string, IHandler> HandlerMap = FrozenDictionary.ToFrozenDictionary<string, IHandler>([
            new("VarInt",       VarInt.Instance),
            new("VarIntZigZag", VarInt.Instance),
            new("Fixed32",      Fixed.Instance ),
            new("Fixed64",      Fixed.Instance ),
            new("Bool",         Bool.Instance  )]);
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
                // <item><c>{4}</c>: private use (fixed size)</item>
                // <item><c>{5}</c>: private use (container type)</item>
                // <item><c>{6}</c>: private use (<see langword="scoped ref"/>)</item>
                // </list>
                object[] formatArgs = new object[7];
                foreach (InternalGenerator.Model model in InternalGenerator.Models)
                {
                    IHandler handler = HandlerMap[model.Mode];
                    formatArgs[0] = model.BaseMode;
                    formatArgs[1] = model.Mode;
                    formatArgs[2] = model.TypeParam;
                    formatArgs[3] = model.Constraint is null ? "" : $", {model.TypeParam}";
                    formatArgs[4] = model.Mode[handler.Keyword.Length..];
                    GenerateCore(writer, in model, handler, formatArgs, true, true);
                    GenerateCore(writer, in model, handler, formatArgs, false, true);
                    GenerateCore(writer, in model, handler, formatArgs, true, false);
                    GenerateCore(writer, in model, handler, formatArgs, false, false);
                }
            }
            string code = writer.InnerWriter.ToString();
            context.AddSource("RepeatedUtility.WriteRepeated.g.cs", code);
        }
        static void GenerateCore(
            IndentedTextWriter writer,
            in InternalGenerator.Model model,
            IHandler handler,
            object[] formatArgs,
            bool isValueType,
            bool isReadOnlySpan)
        {
            formatArgs[5] = isReadOnlySpan ? "ReadOnlySpan" : "IEnumerable";
            formatArgs[6] = isValueType ? "scoped ref " : "";
            writer.WriteLine(Signature, formatArgs);
            using (writer.IndentedBlock())
            {
                if (isValueType)
                    writer.WriteLine("where TWriter : struct, IBinaryWriter, allows ref struct", formatArgs);
                else
                    writer.WriteLine("where TWriter : class, IBinaryWriter", formatArgs);
                if (!string.IsNullOrEmpty(model.Constraint))
                    writer.WriteLine(model.Constraint, formatArgs);
            }
            using (writer.CodeBlock())
            {
                writer.WriteLines(isReadOnlySpan ? handler.ReadOnlySpan : handler.IEnumerable, formatArgs);
                writer.WriteLines(handler.Body, formatArgs);
            }
        }
    }
}