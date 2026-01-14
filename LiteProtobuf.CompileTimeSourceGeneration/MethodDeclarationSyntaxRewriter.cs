using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

public class SemicolonRemover : CSharpSyntaxRewriter
{
    public static SemicolonRemover Instance = new();
    public override SyntaxNode? VisitParameter(ParameterSyntax node)
    {
        return base.VisitParameter(CleanupLeadingTrivia(node
            .WithAttributeLists([])
            .WithDefault(null)));
    }
    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        return base.VisitMethodDeclaration(node
            .WithAttributeLists([])
            .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)));
    }
    private ParameterSyntax CleanupLeadingTrivia(ParameterSyntax parameter)
    {
        var token = parameter.GetLastToken();
        if (token.HasTrailingTrivia)
            return parameter.ReplaceToken(token, token.WithTrailingTrivia());
        return parameter;
    }
}

public static class Extension
{
    extension(in SyntaxToken token)
    {
        public SyntaxToken WithWhitespaceTrailingTrivia(int count = 1, bool newLine = false)
        {
            return token.WithTrailingTrivia(WhitespaceTrivia(count, newLine));
        }
        public SyntaxToken WithWhitespaceLeadingTrivia(int count = 1, bool newLine = false)
        {
            return token.WithLeadingTrivia(WhitespaceTrivia(count, newLine));
        }
    }
    extension<TNode>(TNode node) where TNode : SyntaxNode
    {
        public TNode WithWhitespaceTrailingTrivia(int count = 1, bool newLine = false)
        {
            return node.WithTrailingTrivia(WhitespaceTrivia(count, newLine));
        }
        public TNode WithWhitespaceLeadingTrivia(int count = 1, bool newLine = false)
        {
            return node.WithLeadingTrivia(WhitespaceTrivia(count, newLine));
        }
    }
    extension<TNode>(IEnumerable<TNode> nodes) where TNode : SyntaxNode
    {
        public SeparatedSyntaxList<TNode> WithWhitespaceTrailingTrivia(int count = 1)
        {
            return [.. nodes.Select(it => it.WithWhitespaceLeadingTrivia(count))];
        }
    }

    private static SyntaxTrivia WhitespaceTrivia(int count = 1, bool newLine = false)
    {
        return SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, (newLine ? @"
" : "") + new string(' ', count));
    }
}