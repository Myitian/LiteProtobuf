using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Myitian.LiteProtobuf.SourceGeneration;

static partial class GeneratedProtobufTypeSerializer
{
    public static void RegisterSourceOutput(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: SR.FQ_GeneratedProtobufTypeSerializerAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is BaseTypeDeclarationSyntax,
            transform: static (context, _) => new Step0(context))
            .Where(static it => it.Target is not null)
            .Combine(context.CompilationProvider.Select(static (compilation, _) => new RequiredSymbols(compilation)))
            .Select(static (it, _) => new Step1(it.Left, it.Right)),
            Apply);
    }
    static void Apply(SourceProductionContext context, Step1 step1)
    {
        if (!step1.Diagnostics.IsDefaultOrEmpty)
        {
            foreach (Diagnostic diagnostic in step1.Diagnostics)
                context.ReportDiagnostic(diagnostic);
        }
        bool isValueType = step1.Target.IsValueType;
        ITypeSymbol nullableTarget = isValueType ? step1.Target
            : step1.Target.WithNullableAnnotation(NullableAnnotation.Annotated);
        StringBuilder sb = new();
        IndentedTextWriter writer = new(new StringWriter(sb));
        writer.WriteCSharpHeader();
        using (writer.CSharpTypeBlock(step1.Target))
        {

        }
        string code = sb.ToString();
        string file = sb.Clear().AppendGeneratedFileName(nameof(GeneratedProtobufTypeSerializer), step1.Target).ToString();
        context.AddSource(file, code);
    }
    struct RawProtobufFieldInfo
    {
        public readonly int CustomAttribute = 0;
        public bool NoRead = false;
        public bool NoWrite = false;
        public readonly ITypeSymbol? Factory = null;
        public readonly ITypeSymbol? ReadHandler = null;
        public readonly ITypeSymbol? WriteHandler = null;
        public RawProtobufFieldInfo(AttributeData attribute)
        {
            ITypeSymbol? Handler = null;
            foreach (KeyValuePair<string, TypedConstant> kvp in attribute.NamedArguments)
            {
                switch (kvp.Key)
                {
                    case nameof(CustomAttribute) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: int value
                    }:
                        CustomAttribute = value;
                        break;
                    case nameof(NoRead) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool value
                    }:
                        NoRead = value;
                        break;
                    case nameof(NoWrite) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool value
                    }:
                        NoWrite = value;
                        break;
                    case nameof(Handler) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Type,
                        Value: ITypeSymbol value
                    }:
                        Handler = value;
                        break;
                    case nameof(Factory) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Type,
                        Value: ITypeSymbol value
                    }:
                        Factory = value;
                        break;
                    case nameof(ReadHandler) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Type,
                        Value: ITypeSymbol value
                    }:
                        ReadHandler = value;
                        break;
                    case nameof(WriteHandler) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Type,
                        Value: ITypeSymbol value
                    }:
                        WriteHandler = value;
                        break;
                }
            }
            Factory ??= Handler;
            ReadHandler ??= Handler;
            WriteHandler ??= Handler;
        }
    }
    readonly struct RawProtobufRemeaningFieldsInfo
    {
        public readonly int FieldType = 0;
        public readonly int CustomAttribute = 0;
        public readonly bool NoRead = false;
        public readonly bool NoWrite = false;
        public RawProtobufRemeaningFieldsInfo(AttributeData attribute)
        {
            foreach (KeyValuePair<string, TypedConstant> kvp in attribute.NamedArguments)
            {
                switch (kvp.Key)
                {
                    case nameof(FieldType) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Type,
                        Value: int value
                    }:
                        FieldType = value;
                        break;
                    case nameof(CustomAttribute) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: int value
                    }:
                        CustomAttribute = value;
                        break;
                    case nameof(NoRead) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool value
                    }:
                        NoRead = value;
                        break;
                    case nameof(NoWrite) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool value
                    }:
                        NoWrite = value;
                        break;
                }
            }
        }
    }
    readonly struct ProtobufField
    {
        public readonly ITypeSymbol? Type;
        public readonly string Name;
        public readonly int Number;
        public readonly int FieldType;
        public readonly int CustomAttribute;
        public readonly object? Factory;
        public readonly object? ReadHandler;
        public readonly object? WriteHandler;

        public ProtobufField(
            ITypeSymbol type,
            string name,
            int number,
            int fieldType,
            RawProtobufFieldInfo info)
        {
            Type = type;
            Name = name;
            Number = number;
            FieldType = fieldType;
            CustomAttribute = info.CustomAttribute;
        }
        [MemberNotNullWhen(true, nameof(Type))]
        public bool IsValid => Type is not null;
        public abstract class Collection : ICollection<ProtobufField>
        {
            public abstract int Count { get; }
            public bool IsReadOnly => false;
            public abstract ProtobufField Add(ProtobufField field);
            public abstract void Clear();
            public abstract bool ContainsNumber(int number);
            public abstract void CopyTo(ProtobufField[] array, int arrayIndex);
            public abstract IEnumerator<ProtobufField> GetEnumerator();
            protected abstract bool RemoveByNumber(int number);
            void ICollection<ProtobufField>.Add(ProtobufField item) => Add(item);
            bool ICollection<ProtobufField>.Contains(ProtobufField field) => ContainsNumber(field.Number);
            bool ICollection<ProtobufField>.Remove(ProtobufField item) => RemoveByNumber(item.Number);
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public static Collection Create(bool sorted) => sorted ? new SortedCollection() : new OrderedCollection();
        }
        public sealed class SortedCollection : Collection
        {
            private readonly SortedDictionary<int, ProtobufField> _map = [];
            public override int Count => _map.Count;
            public override ProtobufField Add(ProtobufField field)
            {
                if (_map.TryGetValue(field.Number, out ProtobufField old))
                    return old;
                _map.Add(field.Number, field);
                return default;
            }
            public override void Clear() => _map.Clear();
            public override bool ContainsNumber(int number) => _map.ContainsKey(number);
            public override void CopyTo(ProtobufField[] array, int arrayIndex) => _map.Values.CopyTo(array, arrayIndex);
            public override IEnumerator<ProtobufField> GetEnumerator() => _map.Values.GetEnumerator();
            protected override bool RemoveByNumber(int number) => _map.Remove(number);
        }
        public sealed class OrderedCollection : Collection
        {
            private readonly List<ProtobufField> _values = [];
            private readonly Dictionary<int, ProtobufField> _map = [];
            public override int Count => _values.Count;
            public override ProtobufField Add(ProtobufField field)
            {
                if (_map.TryGetValue(field.Number, out ProtobufField old))
                    return old;
                _map.Add(field.Number, field);
                _values.Add(field);
                return default;
            }
            public override void Clear()
            {
                _map.Clear();
                _values.Clear();
            }
            public override bool ContainsNumber(int number) => _map.ContainsKey(number);
            public override void CopyTo(ProtobufField[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);
            public override IEnumerator<ProtobufField> GetEnumerator() => _values.GetEnumerator();
            protected override bool RemoveByNumber(int number)
            {
                if (_map.Remove(number))
                {
                    _values.RemoveAll(it => it.Number == number);
                    return true;
                }
                return false;
            }
        }
    }
    readonly struct ProtobufRemeaningFields
    {
        public readonly ITypeSymbol? Type;
        public readonly string Name;
        public readonly int FieldType;
        public readonly int CustomAttribute;
        public readonly bool NoRead;
        public readonly bool NoWrite;
        [MemberNotNullWhen(true, nameof(Type))]
        public bool IsValid => Type is not null;
    }
}
