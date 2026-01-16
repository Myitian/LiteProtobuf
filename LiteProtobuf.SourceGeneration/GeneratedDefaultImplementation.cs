using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Myitian.LiteProtobuf.SourceGeneration;

static class GeneratedDefaultImplementation
{
    public const string TryCreateInstance = $$$"""
        public static bool TryCreateInstance(
            {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
            {{{MainGenerator.FQG_SerializationOptions}}}? options,
            [{{{MainGenerator.FQG_NotNullWhenAttribute}}}(true)] out {0} value)
        {{
            return {{{MainGenerator.FQG_DefaultImplementation}}}.ProtobufType.TryCreateInstance<{1}>(fieldInfo, options, out value);
        }}
        """;
    public const string CreateInstance = $$$"""
        public static {1} CreateInstance(
            {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
            {{{MainGenerator.FQG_SerializationOptions}}}? options)
        {{
            return {{{MainGenerator.FQG_DefaultImplementation}}}.ProtobufType.CreateInstance<{1}>(fieldInfo, options);
        }}
        """;
    public const string TryCreateFulfilled = $$$"""
         public static bool TryCreateFulfilled<TReader>(
             scoped ref TReader reader,
             {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
             {{{MainGenerator.FQG_SerializationOptions}}}? options,
             [{{{MainGenerator.FQG_NotNullWhenAttribute}}}(true)] out {0} value,
             out {{{MainGenerator.FQG_ParseStatus}}} status)
             where TReader : struct, {{{MainGenerator.FQG_IStructBinaryReader}}}<TReader>, allows ref struct
         {{
             return {{{MainGenerator.FQG_DefaultImplementation}}}.NoRefStructProtobufType.TryCreateFulfilled<{1}, TReader>(ref reader, fieldInfo, options, out value, out status);
         }}
         public static bool TryCreateFulfilled<TReader>(
             TReader reader,
             {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
             {{{MainGenerator.FQG_SerializationOptions}}}? options,
             [{{{MainGenerator.FQG_NotNullWhenAttribute}}}(true)] out {0} value,
             out {{{MainGenerator.FQG_ParseStatus}}} status)
             where TReader : class, {{{MainGenerator.FQG_IClassBinaryReader}}}<TReader>
         {{
             return {{{MainGenerator.FQG_DefaultImplementation}}}.ProtobufType.TryCreateFulfilled<{1}, TReader>(reader, fieldInfo, options, out value, out status);
         }}
         """;
    public const string CreateFulfilled = $$$"""
         public static {1} CreateFulfilled<TReader>(
             scoped ref TReader reader,
             {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
             {{{MainGenerator.FQG_SerializationOptions}}}? options)
             where TReader : struct, {{{MainGenerator.FQG_IStructBinaryReader}}}<TReader>, allows ref struct
         {{
             return {{{MainGenerator.FQG_DefaultImplementation}}}.ProtobufType.CreateFulfilled<{1}, TReader>(ref reader, fieldInfo, options);
         }}
         public static {1} CreateFulfilled<TReader>(
             TReader reader,
             {{{MainGenerator.FQG_FieldInfo}}} fieldInfo,
             {{{MainGenerator.FQG_SerializationOptions}}}? options)
             where TReader : class, {{{MainGenerator.FQG_IClassBinaryReader}}}<TReader>
         {{
             return {{{MainGenerator.FQG_DefaultImplementation}}}.ProtobufType.CreateFulfilled<{1}, TReader>(reader, fieldInfo, options);
         }}
         """;
    public static void RegisterSourceOutput(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: MainGenerator.FQ_GeneratedDefaultImplementationAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is BaseTypeDeclarationSyntax,
            transform: static (context, _) => new Model(context))
            .Where(static it => it.Target is not null),
            Apply);
    }
    static void Apply(SourceProductionContext context, Model model)
    {
        bool isValueType = model.Target.IsValueType;
        ITypeSymbol symbol = isValueType ? model.Target : model.Target.WithNullableAnnotation(NullableAnnotation.Annotated);

        using PooledArrayHandle<object> formatArgs = new(2);
        formatArgs.Array[0] = symbol.ToDisplayString(MainGenerator.NullableFullyQualifiedFormat);
        formatArgs.Array[1] = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        StringBuilder sb = new();
        IndentedTextWriter writer = new(new StringWriter(sb));
        writer.WriteCSharpHeader("#pragma warning disable CS0108");
        using (writer.CSharpTypeBlock(model.Target))
        {
            if ((model.EnabledMethods & EnabledMethod.TryCreateInstance) != EnabledMethod.None)
                writer.WriteLines(TryCreateInstance, formatArgs.Array);
            if ((model.EnabledMethods & EnabledMethod.CreateInstance) != EnabledMethod.None)
                writer.WriteLines(CreateInstance, formatArgs.Array);
            if ((model.EnabledMethods & EnabledMethod.TryCreateFulfilled) != EnabledMethod.None)
                writer.WriteLines(TryCreateFulfilled, formatArgs.Array);
            if ((model.EnabledMethods & EnabledMethod.CreateFulfilled) != EnabledMethod.None)
                writer.WriteLines(CreateFulfilled, formatArgs.Array);
        }
        string code = writer.InnerWriter.ToString();
        context.AddSource(sb.Clear().Append($"{nameof(GeneratedDefaultImplementation)}/").AppendClrName(model.Target).Append(".g.cs").ToString(), code);
    }
    [Flags]
    enum EnabledMethod
    {
        None,
        TryCreateInstance = 0b0001,
        CreateInstance = 0b0010,
        TryCreateFulfilled = 0b0100,
        CreateFulfilled = 0b1000
    }
    readonly struct Model
    {
        public readonly INamedTypeSymbol Target = null!;
        public readonly EnabledMethod EnabledMethods = EnabledMethod.None;
        public Model(GeneratorAttributeSyntaxContext context)
        {
            Target = (context.TargetSymbol as INamedTypeSymbol)!;
            foreach (KeyValuePair<string, TypedConstant> kvp in context.Attributes.SelectMany(it => it.NamedArguments))
            {
                if (kvp.Value is not
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool v
                    })
                    continue;
                switch (kvp.Key)
                {
                    case nameof(TryCreateInstance) when v:
                        EnabledMethods |= EnabledMethod.TryCreateInstance;
                        break;
                    case nameof(TryCreateInstance):
                        EnabledMethods &= ~EnabledMethod.TryCreateInstance;
                        break;
                    case nameof(CreateInstance) when v:
                        EnabledMethods |= EnabledMethod.CreateInstance;
                        break;
                    case nameof(CreateInstance):
                        EnabledMethods &= ~EnabledMethod.CreateInstance;
                        break;
                    case nameof(TryCreateFulfilled) when v:
                        EnabledMethods |= EnabledMethod.TryCreateFulfilled;
                        break;
                    case nameof(TryCreateFulfilled):
                        EnabledMethods &= ~EnabledMethod.TryCreateFulfilled;
                        break;
                    case nameof(CreateFulfilled) when v:
                        EnabledMethods |= EnabledMethod.CreateFulfilled;
                        break;
                    case nameof(CreateFulfilled):
                        EnabledMethods &= ~EnabledMethod.CreateFulfilled;
                        break;
                }
            }
            if (EnabledMethods is EnabledMethod.None)
                Target = null!;
        }
    }
}