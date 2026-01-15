using System.CodeDom.Compiler;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

static class Utils
{
    public static void BeginCodeBlock(this IndentedTextWriter writer)
    {
        writer.WriteLine('{');
        writer.Indent++;
    }
    public static void EndCodeBlock(this IndentedTextWriter writer)
    {
        writer.WriteLine('}');
        writer.Indent--;
    }
    public static void WriteLines(this IndentedTextWriter writer, string text, params object[] arg)
    {
        using StringReader sr = new(text);
        while (sr.ReadLine() is string line)
            writer.WriteLine(line, arg);
    }
    public static IndentedBlock IndentedBlock(this IndentedTextWriter writer)
    {
        return new(writer);
    }
    public static CodeBlock CodeBlock(this IndentedTextWriter writer)
    {
        return new(writer);
    }
}