using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Myitian.LiteProtobuf.SourceGeneration;

partial class Defaults
{
    public interface IType
    {
        string Prefix { get; }
        string Template { get; }
        string FullyQualifiedMetadataName { get; }
    }
    public class TryCreateInstance : IType
    {
        public static readonly TryCreateInstance Instance = new();
        public string Prefix => nameof(TryCreateInstance);
        public string FullyQualifiedMetadataName => MainGenerator.FQ_DefaultTryCreateInstanceAttribute;
        public string Template => $$$"""
            public static bool TryCreateInstance(
                {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
                {{{MainGenerator.FQG_SerializationOptions}}}? options,
                [{{{MainGenerator.FQG_NotNullWhenAttribute}}}(true)] out {0} value)
            {{
                return {{{MainGenerator.FQG_Defaults}}}.NewReadOnlyProtobufType<{1}>.TryCreateInstance(fieldInfo, options, out value);
            }}
            """;
    }
    public class CreateInstance : IType
    {
        public static readonly CreateInstance Instance = new();
        public string Prefix => nameof(CreateInstance);
        public string FullyQualifiedMetadataName => MainGenerator.FQ_DefaultCreateInstanceAttribute;
        public string Template => $$$"""
            public static {1} CreateInstance(
                {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
                {{{MainGenerator.FQG_SerializationOptions}}}? options)
            {{
                return {{{MainGenerator.FQG_Defaults}}}.ReadOnlyProtobufType<{1}>.CreateInstance(fieldInfo, options);
            }}
            """;
    }
    public class TryCreateFulfilled : IType
    {
        public static readonly TryCreateFulfilled Instance = new();
        public string Prefix => nameof(TryCreateFulfilled);
        public string FullyQualifiedMetadataName => MainGenerator.FQ_DefaultTryCreateFulfilledAttribute;
        public string Template => $$$"""
            public static bool TryCreateFulfilled<TReader>(
                scoped ref TReader reader,
                {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
                {{{MainGenerator.FQG_SerializationOptions}}}? options,
                [{{{MainGenerator.FQG_NotNullWhenAttribute}}}(true)] out {0} value,
                out {{{MainGenerator.FQG_ParseStatus}}} status)
                where TReader : struct, {{{MainGenerator.FQG_IStructBinaryReader}}}<TReader>, allows ref struct
            {{
                return {{{MainGenerator.FQG_Defaults}}}.ReadOnlyProtobufType<{1}>.TryCreateFulfilled(ref reader, fieldInfo, options, out value, out status);
            }}
            public static bool TryCreateFulfilled<TReader>(
                TReader reader,
                {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
                {{{MainGenerator.FQG_SerializationOptions}}}? options,
                [{{{MainGenerator.FQG_NotNullWhenAttribute}}}(true)] out {0} value,
                out {{{MainGenerator.FQG_ParseStatus}}} status)
                where TReader : class, {{{MainGenerator.FQG_IClassBinaryReader}}}<TReader>
            {{
                return {{{MainGenerator.FQG_Defaults}}}.ReadOnlyProtobufType<{1}>.TryCreateFulfilled(reader, fieldInfo, options, out value, out status);
            }}
            """;
    }
    public class CreateFulfilled : IType
    {
        public static readonly CreateFulfilled Instance = new();
        public string Prefix => nameof(CreateFulfilled);
        public string FullyQualifiedMetadataName => MainGenerator.FQ_DefaultCreateFulfilledAttribute;
        public string Template => $$$"""
            public static {1} CreateFulfilled<TReader>(
                scoped ref TReader reader,
                {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
                {{{MainGenerator.FQG_SerializationOptions}}}? options)
                where TReader : struct, {{{MainGenerator.FQG_IStructBinaryReader}}}<TReader>, allows ref struct
            {{
                return {{{MainGenerator.FQG_Defaults}}}.ReadOnlyProtobufType<{1}>.CreateFulfilled(ref reader, fieldInfo, options);
            }}
            public static {1} CreateFulfilled<TReader>(
                TReader reader,
                {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
                {{{MainGenerator.FQG_SerializationOptions}}}? options)
                where TReader : class, {{{MainGenerator.FQG_IClassBinaryReader}}}<TReader>
            {{
                return {{{MainGenerator.FQG_Defaults}}}.ReadOnlyProtobufType<{1}>.CreateFulfilled(reader, fieldInfo, options);
            }}
            """;
    }
    public static void RegisterSourceOutput(IncrementalGeneratorInitializationContext context, IType type)
    {
        context.RegisterSourceOutput(context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: type.FullyQualifiedMetadataName,
            predicate: static (syntaxNode, _) => syntaxNode is BaseTypeDeclarationSyntax,
            transform: static (context, _) => new Model(context))
            .Where(it => it.IsValid)
            .Combine(context.CompilationProvider), (context, model) => Apply(context, type.Prefix, type.Template, model));
    }
    public static void Apply(SourceProductionContext context, string prefix, string template, (Model Model, Compilation Compilation) model)
    {
        if (model.Model is not { Self: INamedTypeSymbol self })
            return;
        StringBuilder sb = new();
        int depth = sb.AppendCSharpCode("#pragma warning disable CS0108", self);
        foreach (INamedTypeSymbol arg in model.Model.Args)
        {
            ITypeSymbol symbol = arg.IsValueType ?
                arg : arg.WithNullableAnnotation(NullableAnnotation.Annotated);
            string c0 = symbol.ToDisplayString(Utils.NullableFullyQualifiedFormat);
            string c1 = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            sb.AppendIndented(depth, string.Format(template, c0, c1));
        }
        while (depth > 0)
        {
            depth--;
            sb.Append(' ', depth * 4)
                .AppendLine("}");
        }
        string code = sb.ToString();
        context.AddSource($"{new StringBuilder().AppendClrName(self)}-{prefix}.g.cs", code);
    }
    public readonly record struct Model
    {
        public bool IsValid => Self is not null && !Args.IsDefaultOrEmpty;
        public INamedTypeSymbol? Self { get; } = null;
        public ImmutableArray<INamedTypeSymbol> Args { get; }

        public Model(GeneratorAttributeSyntaxContext context)
        {
            if (context.TargetSymbol is not INamedTypeSymbol self)
                return;
            List<INamedTypeSymbol> args = [];
            List<string> omitWarningList = [];
            foreach (AttributeData attr in context.Attributes)
            {
                if (attr is not
                    {
                        ConstructorArguments: [
                        {
                            Kind: TypedConstantKind.Type,
                            Value: INamedTypeSymbol arg
                        }]
                    })
                    return;
                args.Add(arg);
            }
            Self = self;
            Args = [.. args];
        }
    }
}