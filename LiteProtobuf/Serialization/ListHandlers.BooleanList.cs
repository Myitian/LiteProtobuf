using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public static partial class ListHandlers
{
    public sealed class BooleanList
        : IClassProtobufTypeHandler<List<bool>>
    {
        public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
        {
            return fieldInfo.ReceivedWireType is WireType.LengthDelimited or WireType.VarInt;
        }
        public static bool IsFieldInfoValidForInstance(in List<bool> value, FieldInfo fieldInfo, SerializationOptions? options)
        {
            return IsFieldInfoValid(fieldInfo, options);
        }
        public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<bool>? value)
        {
            return Defaults.NewProtobufTypeFactory<List<bool>>
                .TryCreateInstance<BooleanList>(fieldInfo, options, out value);
        }
        public static List<bool> CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
        {
            return Defaults.NewProtobufTypeFactory<List<bool>>
                .CreateInstance<BooleanList>(fieldInfo, options);
        }
        public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<bool>? value, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
                .TryCreateFulfilled<BooleanList, TReader>(ref reader, fieldInfo, options, out value, out status);
        }
        public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<bool>? value, out ParseStatus status)
             where TReader : class, IClassBinaryReader<TReader>
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
                .TryCreateFulfilled<BooleanList, TReader>(reader, fieldInfo, options, out value, out status);
        }
        public static List<bool> CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
                .CreateFulfilled<BooleanList, TReader>(ref reader, fieldInfo, options);
        }
        public static List<bool> CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
             where TReader : class, IClassBinaryReader<TReader>
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
                .CreateFulfilled<BooleanList, TReader>(reader, fieldInfo, options);
        }
        public static bool TryReadProtobuf<TReader>(List<bool> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return ProtobufUtility.TryReadRepeatedBool(ref reader, fieldInfo.ReceivedWireType, self, out status);
        }
        public static bool TryReadProtobuf<TReader>(List<bool> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : class, IClassBinaryReader<TReader>
        {
            return ProtobufUtility.TryReadRepeatedBool(reader, fieldInfo.ReceivedWireType, self, out status);
        }
        public static void ReadProtobuf<TReader>(List<bool> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
                .ReadProtobuf<BooleanList, TReader>(self, ref reader, fieldInfo, options);
        }
        public static void ReadProtobuf<TReader>(List<bool> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : class, IClassBinaryReader<TReader>
        {
            ProtobufUtility.ReadRepeatedBool(reader, fieldInfo.ReceivedWireType, self);
        }
        public static void WriteProtobuf<TWriter>(List<bool> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
        {
            ProtobufUtility.WriteRepeatedBool(ref writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
        }
        public static void WriteProtobuf<TWriter>(List<bool> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : class, IClassBinaryWriter<TWriter>
        {
            ProtobufUtility.WriteRepeatedBool(writer, fieldInfo.Number, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
        }
    }
}