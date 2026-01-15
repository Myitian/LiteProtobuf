using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public static partial class ListHandlers
{
    public sealed class GenericNonPackedList<T>
        : IClassProtobufTypeHandler<List<T>>
        where T : IProtobufType<T>
    {
        public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
        {
            return T.IsFieldInfoValid(fieldInfo, options);
        }
        public static bool IsFieldInfoValidForInstance(in List<T> value, FieldInfo fieldInfo, SerializationOptions? options)
        {
            return IsFieldInfoValid(fieldInfo, options);
        }
        public static bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value)
        {
            return Defaults.NewProtobufTypeFactory<List<T>>
                .TryCreateInstance<GenericNonPackedList<T>>(fieldInfo, options, out value);
        }
        public static List<T> CreateInstance(FieldInfo fieldInfo, SerializationOptions? options)
        {
            return Defaults.NewProtobufTypeFactory<List<T>>
                .CreateInstance<GenericNonPackedList<T>>(fieldInfo, options);
        }
        public static bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .TryCreateFulfilled<GenericNonPackedList<T>, TReader>(ref reader, fieldInfo, options, out value, out status);
        }
        public static bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value, out ParseStatus status)
             where TReader : class, IClassBinaryReader<TReader>
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .TryCreateFulfilled<GenericNonPackedList<T>, TReader>(reader, fieldInfo, options, out value, out status);
        }
        public static List<T> CreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .CreateFulfilled<GenericNonPackedList<T>, TReader>(ref reader, fieldInfo, options);
        }
        public static List<T> CreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
             where TReader : class, IClassBinaryReader<TReader>
        {
            return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
                .CreateFulfilled<GenericNonPackedList<T>, TReader>(reader, fieldInfo, options);
        }
        public static bool TryReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            if (!T.TryCreateFulfilled(ref reader, fieldInfo, options, out T? value, out status))
                return false;
            self.Add(value);
            return true;
        }
        public static bool TryReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            where TReader : class, IClassBinaryReader<TReader>
        {
            if (!T.TryCreateFulfilled(reader, fieldInfo, options, out T? value, out status))
                return false;
            self.Add(value);
            return true;
        }
        public static void ReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
        {
            T value = T.CreateFulfilled(ref reader, fieldInfo, options);
            self.Add(value);
        }
        public static void ReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
            where TReader : class, IClassBinaryReader<TReader>
        {
            T value = T.CreateFulfilled(reader, fieldInfo, options);
            self.Add(value);
        }
        public static void WriteProtobuf<TWriter>(List<T> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
        {
            foreach (T item in self)
                item.WriteProtobuf(ref writer, fieldInfo, options);
        }
        public static void WriteProtobuf<TWriter>(List<T> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
            where TWriter : class, IClassBinaryWriter<TWriter>
        {
            foreach (T item in self)
                item.WriteProtobuf(writer, fieldInfo, options);
        }
    }
}