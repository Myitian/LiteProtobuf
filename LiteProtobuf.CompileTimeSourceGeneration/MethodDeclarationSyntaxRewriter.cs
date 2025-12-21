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

public class VirtualMethodToStaticConverter(string typeParameter, string parameter, string type, int depth) : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        int indent = (depth + 1) * 4;
        return base.VisitMethodDeclaration(node
            .WithModifiers(SyntaxFactory.TokenList()
                .Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithWhitespaceTrailingTrivia())
                .Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword).WithWhitespaceTrailingTrivia()))
            .WithAttributeLists([])
            .WithTypeParameterList(SyntaxFactory.TypeParameterList(SeparatedSyntaxList.Create([
                SyntaxFactory.TypeParameter(typeParameter),
                .. (node.TypeParameterList?.Parameters ?? []).WithWhitespaceTrailingTrivia()])))
            .WithParameterList(SyntaxFactory.ParameterList([
                SyntaxFactory.Parameter(
                    default,
                    [
                        SyntaxFactory.Token(SyntaxKind.ScopedKeyword).WithWhitespaceTrailingTrivia(),
                        SyntaxFactory.Token(SyntaxKind.RefKeyword).WithWhitespaceTrailingTrivia()],
                    SyntaxFactory.IdentifierName(typeParameter).WithWhitespaceTrailingTrivia(),
                    SyntaxFactory.Identifier(parameter),
                    default),
                .. node.ParameterList.Parameters.WithWhitespaceTrailingTrivia()
                ]))
            .WithConstraintClauses([
                SyntaxFactory.TypeParameterConstraintClause(
                    SyntaxFactory.Token(SyntaxKind.WhereKeyword).WithWhitespaceTrailingTrivia(),
                    SyntaxFactory.IdentifierName(typeParameter),
                    SyntaxFactory.Token(SyntaxKind.ColonToken).WithWhitespaceLeadingTrivia().WithWhitespaceTrailingTrivia(),
                    default)
                    .WithConstraints([
                        SyntaxFactory.TypeConstraint(SyntaxFactory.IdentifierName(type)),
                        SyntaxFactory.AllowsConstraintClause(
                            SyntaxFactory.Token(SyntaxKind.AllowsKeyword).WithWhitespaceLeadingTrivia(),
                            [SyntaxFactory.RefStructConstraint(
                                SyntaxFactory.Token(SyntaxKind.RefKeyword).WithWhitespaceLeadingTrivia(),
                                SyntaxFactory.Token(SyntaxKind.StructKeyword).WithWhitespaceLeadingTrivia())])])
                    .WithWhitespaceLeadingTrivia(indent, true)
                    .WithWhitespaceTrailingTrivia(0, true),
                .. node.ConstraintClauses.Select(it => it.WithWhereKeyword(it.WhereKeyword.WithWhitespaceLeadingTrivia(indent)))]));
    }
    public override SyntaxNode? VisitThisExpression(ThisExpressionSyntax node)
    {
        return SyntaxFactory.IdentifierName(parameter)
            .WithLeadingTrivia(node.GetLeadingTrivia())
            .WithTrailingTrivia(node.GetTrailingTrivia());
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
    extension<TNode>(SeparatedSyntaxList<TNode> nodes) where TNode : SyntaxNode
    {
        public SeparatedSyntaxList<TNode> WithWhitespaceTrailingTrivia(int count = 1)
        {
            if (nodes.Count > 0)
            {
                TNode node = nodes[0];
                nodes = nodes.Replace(node, node.WithWhitespaceLeadingTrivia(count));
            }
            return nodes;
        }
    }

    private static SyntaxTrivia WhitespaceTrivia(int count = 1, bool newLine = false)
    {
        return SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, (newLine ? @"
" : "") + new string(' ', count));
    }
}