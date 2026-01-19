using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Myitian.LiteProtobuf.SourceGeneration;

static partial class GeneratedProtobufTypeSerializer
{
    readonly record struct Step0
    {
        public readonly INamedTypeSymbol? Target;
        public readonly bool Read = true;
        public readonly bool TryRead = true;
        public readonly bool Write = true;
        public readonly bool NoSort = false;
        public readonly string? ReadingCompleteCallback = null;
        public Step0(GeneratorAttributeSyntaxContext context)
        {
            Target = context.TargetSymbol as INamedTypeSymbol;
            foreach (KeyValuePair<string, TypedConstant> kvp in context.Attributes.SelectMany(it => it.NamedArguments))
            {
                switch (kvp.Key)
                {
                    case nameof(Read) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool v
                    }:
                        Read = v;
                        break;
                    case nameof(TryRead) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool v
                    }:
                        TryRead = v;
                        break;
                    case nameof(Write) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool v
                    }:
                        Write = v;
                        break;
                    case nameof(NoSort) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: bool v
                    }:
                        NoSort = v;
                        break;
                    case nameof(ReadingCompleteCallback) when kvp.Value is
                    {
                        Kind: TypedConstantKind.Primitive,
                        Value: string v
                    }:
                        ReadingCompleteCallback = v;
                        break;
                }
            }
        }
    }
}
