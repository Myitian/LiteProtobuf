using System;
using System.CodeDom.Compiler;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

readonly struct IndentedBlock : IDisposable
{
    public readonly IndentedTextWriter Writer;
    public IndentedBlock(IndentedTextWriter writer)
        => (Writer = writer).Indent++;
    public void Dispose()
        => Writer.Indent--;
}
