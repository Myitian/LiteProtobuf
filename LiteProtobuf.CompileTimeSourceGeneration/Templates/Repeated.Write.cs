using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

public partial class Repeated
{
    public partial class Write
    {
        private static readonly IType[] types = [
            VarInt.Instance,
            Fixed.Instance,
            Bool.Instance
        ];

        public static void Apply(IncrementalGeneratorPostInitializationContext context)
        {
            IndentedTextWriter writer = new(new StringWriter());
            writer.WriteLine(ProtobufUtilityHeader);
            using (writer.CodeBlock())
            {
                foreach (Model model in Models)
                {
                    ApplyCore(writer, model, true, true);
                    ApplyCore(writer, model, false, true);
                    ApplyCore(writer, model, true, false);
                    ApplyCore(writer, model, false, false);
                }
            }
            string code = writer.InnerWriter.ToString();
            context.AddSource("ProtobufUtility.WriteRepeated.g.cs", code);
        }
        public static void ApplyCore(IndentedTextWriter writer, Model model, bool isValueType, bool isReadOnlySpan)
        {
            // <list type="bullet">
            // <item><c>{0}</c>: base mode</item>
            // <item><c>{1}</c>: mode</item>
            // <item><c>{2}</c>: type param</item>
            // <item><c>{3}</c>: type param with comma</item>
            // <item><c>{4}</c>: scoped ref</item>
            // <item><c>{5}</c>: private use (container)</item>
            // <item><c>{6}</c>: private use (fixed size)</item>
            // </list>
            using PooledArrayHandle<object> formatArgs = new(7);
            formatArgs.Array[0] = model.BaseMode;
            formatArgs.Array[1] = model.Mode;
            formatArgs.Array[2] = model.TypeParam;
            formatArgs.Array[3] = model.Constraint is null ? "" : $"{model.TypeParam}, ";
            formatArgs.Array[4] = isValueType ? "scoped ref " : "";
            formatArgs.Array[5] = isReadOnlySpan ? "ReadOnlySpan" : "IEnumerable";
            formatArgs.Array[6] = model.Mode[model.Type.Keyword.Length..];

            writer.WriteLine(Sign, formatArgs.Array);
            using (writer.IndentedBlock())
            {
                if (!string.IsNullOrEmpty(model.Constraint))
                    writer.WriteLine(model.Constraint, formatArgs.Array);
                if (isValueType)
                    writer.WriteLine("where TWriter : struct, IBinaryWriter, allows ref struct", formatArgs.Array);
                else
                    writer.WriteLine("where TWriter : class, IBinaryWriter", formatArgs.Array);
            }
            using (writer.CodeBlock())
            {
                writer.WriteLines(isReadOnlySpan ? model.Type.ReadOnlySpan : model.Type.IEnumerable, formatArgs.Array);
                writer.WriteLines(model.Type.Common, formatArgs.Array);
            }
        }

        public const string Sign = "public static void WriteRepeated{1}<{3}TWriter>({4}TWriter writer, int number, {5}<{2}> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)";

        public interface IType
        {
            string Keyword { get; }
            string ReadOnlySpan { get; }
            string IEnumerable { get; }
            string Common { get; }
        }
    }
}