using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public static partial class ListHandlers
{
    public sealed class GenericNonPackedStructList<T, THandler>
        : IClassProtobufTypeHandler<List<T>>
        where T : struct
        where THandler : IStructProtobufTypeHandler<T>
    {
        public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
        {
            return THandler.IsFieldInfoValid(fieldInfo, options);
        }
        public static bool IsFieldInfoValidForInstance(in List<T> value, FieldInfo fieldInfo, SerializationOptions? options)
        {
            return IsFieldInfoValid(fieldInfo, options);
        }
        public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value)
        {
            return Defaults.NewProtobufTypeFactory<List<T>>
                .TryCreateInstance<GenericNonPackedStructList<T, THandler>>(fieldInfo, options, out value);
        }
        public static List<T> CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
        {
            return Defaults.NewProtobufTypeFactory<List<T>>
                .CreateInstance<GenericNonPackedStructList<T, THandler>>(fieldInfo, options);
        }
        public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .TryCreateFulfilled<GenericNonPackedStructList<T, THandler>, TReader>(ref reader, fieldInfo, options, out value, out status);
        }
        public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value, out ParseStatus status)
             where TReader : class, IClassBinaryReader<TReader>
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .TryCreateFulfilled<GenericNonPackedStructList<T, THandler>, TReader>(reader, fieldInfo, options, out value, out status);
        }
        public static List<T> CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .CreateFulfilled<GenericNonPackedStructList<T, THandler>, TReader>(ref reader, fieldInfo, options);
        }
        public static List<T> CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
             where TReader : class, IClassBinaryReader<TReader>
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .CreateFulfilled<GenericNonPackedStructList<T, THandler>, TReader>(reader, fieldInfo, options);
        }
        public static bool TryReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!THandler.TryCreateFulfilled(ref reader, fieldInfo, options, out T value, out status))
                return false;
            self.Add(value);
            return true;
        }
        public static bool TryReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!THandler.TryCreateFulfilled(reader, fieldInfo, options, out T value, out status))
                return false;
            self.Add(value);
            return true;
        }
        public static void ReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            T value = THandler.CreateFulfilled(ref reader, fieldInfo, options);
            self.Add(value);
        }
        public static void ReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : class, IClassBinaryReader<TReader>
        {
            T value = THandler.CreateFulfilled(reader, fieldInfo, options);
            self.Add(value);
        }
        public static void WriteProtobuf<TWriter>(List<T> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
        {
            foreach (T item in self)
                THandler.WriteProtobuf(in item, ref writer, fieldInfo, options);
        }
        public static void WriteProtobuf<TWriter>(List<T> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : class, IClassBinaryWriter<TWriter>
        {
            foreach (T item in self)
                THandler.WriteProtobuf(in item, writer, fieldInfo, options);
        }
    }
}