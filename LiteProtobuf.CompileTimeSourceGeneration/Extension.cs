using System.CodeDom.Compiler;
using System.IO;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

static class Extension
{
    public static void WriteLines(this IndentedTextWriter writer, string text, params object[] arg)
    {
        using StringReader sr = new(text);
        while (sr.ReadLine() is string line)
            writer.WriteLine(line, arg);
    }
}