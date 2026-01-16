namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

static partial class DefaultHandler
{
    interface IHandler
    {
        string Signature { get; }
        string Body { get; }
    }
    sealed class CollectionHandler : IHandler
    {
        public static readonly CollectionHandler Instance = new();

        public string Signature => """
            public sealed class {1}CollectionHandler<{3}TCollection> : IClassProtobufTypeHandler<TCollection>
                where TCollection : class, ICollection<{2}>, new()
            """;
        public string Body => """
            public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return fieldInfo.ReceivedWireType is WireType.{0} or WireType.LengthDelimited;
            }}
            public static bool IsFieldInfoValidForInstance(in TCollection value, FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return IsFieldInfoValid(fieldInfo, options);
            }}
            public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value)
            {{
                return ProtobufTypeFactory.TryCreateInstance<TCollection, {1}CollectionHandler<{3}TCollection>>(fieldInfo, options, out value);
            }}
            public static TCollection CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return ProtobufTypeFactory.CreateInstance<TCollection, {1}CollectionHandler<{3}TCollection>>(fieldInfo, options);
            }}
            public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value, out ParseStatus status)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return ClassProtobufTypeHandler.TryCreateFulfilled<TCollection, {1}CollectionHandler<{3}TCollection>, TReader>(ref reader, fieldInfo, options, out value, out status);
            }}
            public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value, out ParseStatus status)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return ClassProtobufTypeHandler.TryCreateFulfilled<TCollection, {1}CollectionHandler<{3}TCollection>, TReader>(reader, fieldInfo, options, out value, out status);
            }}
            public static TCollection CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return ClassProtobufTypeHandler.CreateFulfilled<TCollection, {1}CollectionHandler<{3}TCollection>, TReader>(ref reader, fieldInfo, options);
            }}
            public static TCollection CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return ClassProtobufTypeHandler.CreateFulfilled<TCollection, {1}CollectionHandler<{3}TCollection>, TReader>(reader, fieldInfo, options);
            }}
            public static bool TryReadProtobuf<TReader>(TCollection self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return ProtobufUtility.TryReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static bool TryReadProtobuf<TReader>(TCollection self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return ProtobufUtility.TryReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static void ReadProtobuf<TReader>(TCollection self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                ProtobufUtility.ReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self);
            }}
            public static void ReadProtobuf<TReader>(TCollection self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                ProtobufUtility.ReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self);
            }}
            public static void WriteProtobuf<TWriter>(TCollection self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
            {{
                ProtobufUtility.WriteRepeated{1}(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
            }}
            public static void WriteProtobuf<TWriter>(TCollection self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : class, IClassBinaryWriter<TWriter>
            {{
                ProtobufUtility.WriteRepeated{1}(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
            }}
            """;
    }
    sealed class CollectionReadOnlyHandler : IHandler
    {
        public static readonly CollectionReadOnlyHandler Instance = new();

        public string Signature => """
            public sealed class {1}CollectionReadOnlyHandler{4} : IReadOnlyClassProtobufTypeHandler<ICollection<{2}>>
            """;
        public string Body => """
            public static bool TryReadProtobuf<TReader>(ICollection<{2}> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return ProtobufUtility.TryReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static bool TryReadProtobuf<TReader>(ICollection<{2}> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return ProtobufUtility.TryReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static void ReadProtobuf<TReader>(ICollection<{2}> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                ProtobufUtility.ReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self);
            }}
            public static void ReadProtobuf<TReader>(ICollection<{2}> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                ProtobufUtility.ReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self);
            }}
            """;
    }
    sealed class EnumerableWriteOnlyHandler : IHandler
    {
        public static readonly EnumerableWriteOnlyHandler Instance = new();

        public string Signature => """
            public sealed class {1}EnumerableWriteOnlyHandler{4} : IWriteOnlyClassProtobufTypeHandler<IEnumerable<{2}>>
            """;
        public string Body => """
            public static void WriteProtobuf<TWriter>(IEnumerable<{2}> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
            {{
                ProtobufUtility.WriteRepeated{1}(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
            }}
            public static void WriteProtobuf<TWriter>(IEnumerable<{2}> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : class, IClassBinaryWriter<TWriter>
            {{
                ProtobufUtility.WriteRepeated{1}(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
            }}
            """;
    }
    sealed class ReadOnlySpanWriteOnlyHandler : IHandler
    {
        public static readonly ReadOnlySpanWriteOnlyHandler Instance = new();

        public string Signature => """
            public sealed class {1}ReadOnlySpanWriteOnlyHandler{4} : IWriteOnlyStructProtobufTypeHandler<ReadOnlySpan<{2}>>
            """;
        public string Body => """
            public static void WriteProtobuf<TWriter>(in ReadOnlySpan<{2}> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
            {{
                ProtobufUtility.WriteRepeated{1}(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
            }}
            public static void WriteProtobuf<TWriter>(in ReadOnlySpan<{2}> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : class, IClassBinaryWriter<TWriter>
            {{
                ProtobufUtility.WriteRepeated{1}(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
            }}
            """;
    }
}