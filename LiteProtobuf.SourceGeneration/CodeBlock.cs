using System;
using System.CodeDom.Compiler;

namespace Myitian.LiteProtobuf.SourceGeneration;

readonly struct CodeBlock : IDisposable
{
    public readonly IndentedTextWriter Writer;
    public CodeBlock(IndentedTextWriter writer)
        => (Writer = writer).BeginCodeBlock();
    public void Dispose()
        => Writer.EndCodeBlock();
}