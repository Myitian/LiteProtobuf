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
            {{{SR.FQG_FieldInfo}}} fieldInfo,
            {{{SR.FQG_SerializationOptions}}}? options,
            [{{{SR.FQG_NotNullWhenAttribute}}}(true)] out {0} value)
        {{
            return {{{SR.FQG_DefaultImplementation}}}.ProtobufType.TryCreateInstance<{1}>(fieldInfo, options, out value);
        }}
        """;
    public const string CreateInstance = $$$"""
        public static {1} CreateInstance(
            {{{SR.FQG_FieldInfo}}} fieldInfo,
            {{{SR.FQG_SerializationOptions}}}? options)
        {{
            return {{{SR.FQG_DefaultImplementation}}}.ProtobufType.CreateInstance<{1}>(fieldInfo, options);
        }}
        """;
    public const string TryCreateFulfilled = $$$"""
         public static bool TryCreateFulfilled<TReader>(
             scoped ref TReader reader,
             {{{SR.FQG_FieldInfo}}} fieldInfo,
             {{{SR.FQG_SerializationOptions}}}? options,
             [{{{SR.FQG_NotNullWhenAttribute}}}(true)] out {0} value,
             out {{{SR.FQG_ParseStatus}}} status)
             where TReader : struct, {{{SR.FQG_IStructBinaryReader}}}<TReader>, allows ref struct
         {{
             return {{{SR.FQG_DefaultImplementation}}}.NoRefStructProtobufType.TryCreateFulfilled<TReader, {1}>(ref reader, fieldInfo, options, out value, out status);
         }}
         public static bool TryCreateFulfilled<TReader>(
             TReader reader,
             {{{SR.FQG_FieldInfo}}} fieldInfo,
             {{{SR.FQG_SerializationOptions}}}? options,
             [{{{SR.FQG_NotNullWhenAttribute}}}(true)] out {0} value,
             out {{{SR.FQG_ParseStatus}}} status)
             where TReader : class, {{{SR.FQG_IClassBinaryReader}}}<TReader>
         {{
             return {{{SR.FQG_DefaultImplementation}}}.ProtobufType.TryCreateFulfilled<TReader, {1}>(reader, fieldInfo, options, out value, out status);
         }}
         """;
    public const string CreateFulfilled = $$$"""
         public static {1} CreateFulfilled<TReader>(
             scoped ref TReader reader,
             {{{SR.FQG_FieldInfo}}} fieldInfo,
             {{{SR.FQG_SerializationOptions}}}? options)
             where TReader : struct, {{{SR.FQG_IStructBinaryReader}}}<TReader>, allows ref struct
         {{
             return {{{SR.FQG_DefaultImplementation}}}.ProtobufType.CreateFulfilled<TReader, {1}>(ref reader, fieldInfo, options);
         }}
         public static {1} CreateFulfilled<TReader>(
             TReader reader,
             {{{SR.FQG_FieldInfo}}} fieldInfo,
             {{{SR.FQG_SerializationOptions}}}? options)
             where TReader : class, {{{SR.FQG_IClassBinaryReader}}}<TReader>
         {{
             return {{{SR.FQG_DefaultImplementation}}}.ProtobufType.CreateFulfilled<TReader, {1}>(reader, fieldInfo, options);
         }}
         """;
    public static void RegisterSourceOutput(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: SR.FQ_GeneratedDefaultImplementationAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is BaseTypeDeclarationSyntax,
            transform: static (context, _) => new Model(context))
            .Where(static it => it.Target is not null),
            Apply);
    }
    static void Apply(SourceProductionContext context, Model model)
    {
        bool isValueType = model.Target.IsValueType;
        ITypeSymbol nullableTarget = isValueType ? model.Target
            : model.Target.WithNullableAnnotation(NullableAnnotation.Annotated);
        StringBuilder sb = new();
        IndentedTextWriter writer = new(new StringWriter(sb));
        writer.WriteCSharpHeader("#pragma warning disable CS0108");
        using (writer.CSharpTypeBlock(model.Target))
        {
            object[] formatArgs = [
                nullableTarget.ToDisplayString(MainGenerator.NullableFullyQualifiedFormat),
                nullableTarget.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)];
            if ((model.EnabledMethods & EnabledMethod.TryCreateInstance) != EnabledMethod.None)
                writer.WriteLines(TryCreateInstance, formatArgs);
            if ((model.EnabledMethods & EnabledMethod.CreateInstance) != EnabledMethod.None)
                writer.WriteLines(CreateInstance, formatArgs);
            if ((model.EnabledMethods & EnabledMethod.TryCreateFulfilled) != EnabledMethod.None)
                writer.WriteLines(TryCreateFulfilled, formatArgs);
            if ((model.EnabledMethods & EnabledMethod.CreateFulfilled) != EnabledMethod.None)
                writer.WriteLines(CreateFulfilled, formatArgs);
        }
        string code = sb.ToString();
        string file = sb.Clear().AppendGeneratedFileName(nameof(GeneratedDefaultImplementation), model.Target).ToString();
        context.AddSource(file, code);
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
    readonly record struct Model
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