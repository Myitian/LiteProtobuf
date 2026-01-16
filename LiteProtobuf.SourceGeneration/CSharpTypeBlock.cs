using Microsoft.CodeAnalysis;
using System;
using System.CodeDom.Compiler;

namespace Myitian.LiteProtobuf.SourceGeneration;

readonly struct CSharpTypeBlock : IDisposable
{
    public readonly IndentedTextWriter Writer;
    public CSharpTypeBlock(IndentedTextWriter writer, INamedTypeSymbol type)
        => (Writer = writer).BeginCSharpTypeBlock(type);
    public void Dispose()
        => Writer.EndCSharpTypeBlock();
}