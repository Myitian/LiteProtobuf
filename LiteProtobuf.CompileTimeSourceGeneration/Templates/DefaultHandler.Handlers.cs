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
            public sealed class {1}CollectionHandler<TCollection{3}> : IClassProtobufTypeHandler<TCollection>
                where TCollection : class, ICollection<{2}>, new()
            """;
        public string Body => """
            public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return fieldInfo.ReceivedWireType is WireType.{0} or WireType.LengthDelimited;
            }}
            public static bool IsFieldInfoValidForInstance(TCollection value, FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return IsFieldInfoValid(fieldInfo, options);
            }}
            public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value)
            {{
                return ProtobufTypeFactory.TryCreateInstance<TCollection, {1}CollectionHandler<TCollection{3}>>(fieldInfo, options, out value);
            }}
            public static TCollection CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return ProtobufTypeFactory.CreateInstance<TCollection, {1}CollectionHandler<TCollection{3}>>(fieldInfo, options);
            }}
            public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value, out ParseStatus status)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return ClassProtobufTypeHandler.TryCreateFulfilled<TReader, TCollection, {1}CollectionHandler<TCollection{3}>>(ref reader, fieldInfo, options, out value, out status);
            }}
            public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out TCollection? value, out ParseStatus status)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return ClassProtobufTypeHandler.TryCreateFulfilled<TReader, TCollection, {1}CollectionHandler<TCollection{3}>>(reader, fieldInfo, options, out value, out status);
            }}
            public static TCollection CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return ClassProtobufTypeHandler.CreateFulfilled<TReader, TCollection, {1}CollectionHandler<TCollection{3}>>(ref reader, fieldInfo, options);
            }}
            public static TCollection CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return ClassProtobufTypeHandler.CreateFulfilled<TReader, TCollection, {1}CollectionHandler<TCollection{3}>>(reader, fieldInfo, options);
            }}
            public static bool TryReadProtobuf<TReader>(TCollection self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return RepeatedUtility.TryReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static bool TryReadProtobuf<TReader>(TCollection self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return RepeatedUtility.TryReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static void ReadProtobuf<TReader>(TCollection self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                RepeatedUtility.ReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self);
            }}
            public static void ReadProtobuf<TReader>(TCollection self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                RepeatedUtility.ReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self);
            }}
            public static void WriteProtobuf<TWriter>(TCollection self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
            {{
                RepeatedUtility.WriteRepeated{1}(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.IsPacked);
            }}
            public static void WriteProtobuf<TWriter>(TCollection self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : class, IClassBinaryWriter<TWriter>
            {{
                RepeatedUtility.WriteRepeated{1}(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.IsPacked);
            }}
            """;
    }
    sealed class CollectionReadOnlyHandler : IHandler
    {
        public static readonly CollectionReadOnlyHandler Instance = new();

        public string Signature => """
            public sealed class {1}CollectionReadOnlyHandler{4} : IClassProtobufTypeReadOnlyHandler<ICollection<{2}>>
            """;
        public string Body => """
            public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return fieldInfo.ReceivedWireType is WireType.{0} or WireType.LengthDelimited;
            }}
            public static bool IsFieldInfoValidForInstance(ICollection<{2}> value, FieldInfo fieldInfo, SerializationOptions? options)
            {{
                return IsFieldInfoValid(fieldInfo, options);
            }}
            public static bool TryReadProtobuf<TReader>(ICollection<{2}> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                return RepeatedUtility.TryReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static bool TryReadProtobuf<TReader>(ICollection<{2}> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                return RepeatedUtility.TryReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self, out status);
            }}
            public static void ReadProtobuf<TReader>(ICollection<{2}> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
            {{
                RepeatedUtility.ReadRepeated{1}(ref reader, fieldInfo.ReceivedWireType, self);
            }}
            public static void ReadProtobuf<TReader>(ICollection<{2}> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
                where TReader : class, IClassBinaryReader<TReader>
            {{
                RepeatedUtility.ReadRepeated{1}(reader, fieldInfo.ReceivedWireType, self);
            }}
            """;
    }
    sealed class EnumerableWriteOnlyHandler : IHandler
    {
        public static readonly EnumerableWriteOnlyHandler Instance = new();

        public string Signature => """
            public sealed class {1}EnumerableWriteOnlyHandler{4} : IClassProtobufTypeWriteOnlyHandler<IEnumerable<{2}>>
            """;
        public string Body => """
            public static void WriteProtobuf<TWriter>(IEnumerable<{2}> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
            {{
                RepeatedUtility.WriteRepeated{1}(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.IsPacked);
            }}
            public static void WriteProtobuf<TWriter>(IEnumerable<{2}> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : class, IClassBinaryWriter<TWriter>
            {{
                RepeatedUtility.WriteRepeated{1}(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.IsPacked);
            }}
            """;
    }
    sealed class ReadOnlySpanWriteOnlyHandler : IHandler
    {
        public static readonly ReadOnlySpanWriteOnlyHandler Instance = new();

        public string Signature => """
            public sealed class {1}ReadOnlySpanWriteOnlyHandler{4} : IStructProtobufTypeWriteOnlyHandler<ReadOnlySpan<{2}>>
            """;
        public string Body => """
            public static void WriteProtobuf<TWriter>(in ReadOnlySpan<{2}> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
            {{
                RepeatedUtility.WriteRepeated{1}(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.IsPacked);
            }}
            public static void WriteProtobuf<TWriter>(in ReadOnlySpan<{2}> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
                where TWriter : class, IClassBinaryWriter<TWriter>
            {{
                RepeatedUtility.WriteRepeated{1}(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.IsPacked);
            }}
            """;
    }
}