using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

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