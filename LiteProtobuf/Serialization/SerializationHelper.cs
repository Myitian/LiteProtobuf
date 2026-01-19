namespace Myitian.LiteProtobuf.Serialization;

public static class SerializationHelper
{
    public static void Dispose<T>(ref T obj, List<Exception> exceptions)
        where T : IDisposable, allows ref struct
    {
        try
        {
            obj.Dispose();
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }
    }
    public static void Dispose<T>(T? obj, List<Exception> exceptions)
        where T : class, IDisposable
    {
        try
        {
            obj?.Dispose();
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }
    }
    public static class StructBinaryReader<TReader>
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        public static class StructCollection<T>
            where T : struct, IProtobufFieldCollection, allows ref struct
        {
            public static void ReadField(scoped ref TReader reader, scoped ref T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.AddProtobufField(ref reader, fieldInfo, options);
            }
            public static bool TryReadField(scoped ref TReader reader, scoped ref T collection, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                return collection.TryAddProtobufField(ref reader, fieldInfo, options, out status);
            }
        }
        public static class ClassCollection<T>
            where T : class, IProtobufFieldCollection
        {
            public static void ReadField(scoped ref TReader reader, T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.AddProtobufField(ref reader, fieldInfo, options);
            }
            public static bool TryReadField(scoped ref TReader reader, T collection, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                return collection.TryAddProtobufField(ref reader, fieldInfo, options, out status);
            }
        }
        public static class StructCreatableReadOnly<T>
            where T : struct, ICreatableProtobufType<T>, IReadOnlyProtobufType, allows ref struct
        {
            public static void ReadField(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value!.ReadProtobuf(ref reader, fieldInfo, options);
            }
            public static bool TryReadField(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value!.TryReadProtobuf(ref reader, fieldInfo, options, out status);
            }
        }
        public static class ClassCreatableReadOnly<T>
            where T : class, ICreatableProtobufType<T>, IReadOnlyProtobufType
        {
            public static void ReadField(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value!.ReadProtobuf(ref reader, fieldInfo, options);
            }
            public static bool TryReadField(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value!.TryReadProtobuf(ref reader, fieldInfo, options, out status);
            }
        }
        public static class StructCreatable<T>
            where T : struct, ICreatableProtobufType<T>, allows ref struct
        {
            public static void ReadField<THandler>(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(ref value, ref reader, fieldInfo, options);
            }
            public static bool TryReadField<THandler>(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
            }
        }
        public static class ClassCreatable<T>
            where T : class, ICreatableProtobufType<T>
        {
            public static void ReadField<THandler>(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(value!, ref reader, fieldInfo, options);
            }
            public static bool TryReadField<THandler>(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(value!, ref reader, fieldInfo, options, out status);
            }
        }
        public static class StructReadOnly<T>
            where T : struct, IReadOnlyProtobufType, allows ref struct
        {
            public static void ReadField<TFactory>(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value.ReadProtobuf(ref reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory>(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value.TryReadProtobuf(ref reader, fieldInfo, options, out status);
            }
        }
        public static class ClassReadOnly<T>
            where T : class, IReadOnlyProtobufType
        {
            public static void ReadField<TFactory>(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value!.ReadProtobuf(ref reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory>(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value!.TryReadProtobuf(ref reader, fieldInfo, options, out status);
            }
        }
        public static class Struct<T>
            where T : struct, allows ref struct
        {
            public static void ReadField<TFactory, THandler>(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(ref value, ref reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory, THandler>(scoped ref TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(ref value, ref reader, fieldInfo, options, out status);
            }
        }
        public static class Class<T>
            where T : class
        {
            public static void ReadField<TFactory, THandler>(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(value!, ref reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory, THandler>(scoped ref TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(value!, ref reader, fieldInfo, options, out status);
            }
        }
    }
    public static class ClassBinaryReader<TReader>
        where TReader : class, IClassBinaryReader<TReader>
    {
        public static class StructCollection<T>
            where T : struct, IProtobufFieldCollection, allows ref struct
        {
            public static void ReadField(TReader reader, scoped ref T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.AddProtobufField(reader, fieldInfo, options);
            }
            public static bool TryReadField(TReader reader, scoped ref T collection, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                return collection.TryAddProtobufField(reader, fieldInfo, options, out status);
            }
        }
        public static class ClassCollection<T>
            where T : class, IProtobufFieldCollection
        {
            public static void ReadField(TReader reader, T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.AddProtobufField(reader, fieldInfo, options);
            }
            public static bool TryReadField(TReader reader, T collection, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                return collection.TryAddProtobufField(reader, fieldInfo, options, out status);
            }
        }
        public static class StructCreatableReadOnly<T>
            where T : struct, ICreatableProtobufType<T>, IReadOnlyProtobufType, allows ref struct
        {
            public static void ReadField(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value!.ReadProtobuf(reader, fieldInfo, options);
            }
            public static bool TryReadField(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value!.TryReadProtobuf(reader, fieldInfo, options, out status);
            }
        }
        public static class ClassCreatableReadOnly<T>
            where T : class, ICreatableProtobufType<T>, IReadOnlyProtobufType
        {
            public static void ReadField(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value!.ReadProtobuf(reader, fieldInfo, options);
            }
            public static bool TryReadField(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value!.TryReadProtobuf(reader, fieldInfo, options, out status);
            }
        }
        public static class StructCreatable<T>
            where T : struct, ICreatableProtobufType<T>, allows ref struct
        {
            public static void ReadField<THandler>(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(ref value, reader, fieldInfo, options);
            }
            public static bool TryReadField<THandler>(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(ref value, reader, fieldInfo, options, out status);
            }
        }
        public static class ClassCreatable<T>
            where T : class, ICreatableProtobufType<T>
        {
            public static void ReadField<THandler>(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = T.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(value!, reader, fieldInfo, options);
            }
            public static bool TryReadField<THandler>(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!T.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(value!, reader, fieldInfo, options, out status);
            }
        }
        public static class StructReadOnly<T>
            where T : struct, IReadOnlyProtobufType, allows ref struct
        {
            public static void ReadField<TFactory>(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value.ReadProtobuf(reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory>(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value.TryReadProtobuf(reader, fieldInfo, options, out status);
            }
        }
        public static class ClassReadOnly<T>
            where T : class, IReadOnlyProtobufType
        {
            public static void ReadField<TFactory>(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                value!.ReadProtobuf(reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory>(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return value!.TryReadProtobuf(reader, fieldInfo, options, out status);
            }
        }
        public static class Struct<T>
            where T : struct, allows ref struct
        {
            public static void ReadField<TFactory, THandler>(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(ref value, reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory, THandler>(TReader reader, scoped ref T value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IStructProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(ref value, reader, fieldInfo, options, out status);
            }
        }
        public static class Class<T>
            where T : class
        {
            public static void ReadField<TFactory, THandler>(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    value = TFactory.CreateInstance(fieldInfo, options);
                    existed = true;
                }
                THandler.ReadProtobuf(value!, reader, fieldInfo, options);
            }
            public static bool TryReadField<TFactory, THandler>(TReader reader, T? value, scoped ref bool existed, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
                where TFactory : IProtobufTypeFactory<T>, allows ref struct
                where THandler : IClassProtobufTypeReadOnlyHandler<T>, allows ref struct
            {
                if (!existed)
                {
                    if (!TFactory.TryCreateInstance(fieldInfo, options, out value))
                    {
                        status = ParseStatus.InvalidData;
                        return false;
                    }
                    existed = true;
                }
                return THandler.TryReadProtobuf(value!, reader, fieldInfo, options, out status);
            }
        }
    }
    public static class StructBinaryWriter<TWriter>
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        public static class StructCollection<T>
            where T : struct, IProtobufFieldCollection, allows ref struct
        {
            public static void WriteField(scoped ref TWriter writer, scoped ref T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.WriteProtobufBody(ref writer, fieldInfo, options);
            }
        }
        public static class ClassCollection<T>
            where T : class, IProtobufFieldCollection
        {
            public static void WriteField(scoped ref TWriter writer, T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.WriteProtobufBody(ref writer, fieldInfo, options);
            }
        }
        public static class StructWriteOnly<T>
            where T : struct, IWriteOnlyProtobufType, allows ref struct
        {
            public static void WriteField(scoped ref TWriter writer, scoped ref T value, FieldInfo fieldInfo, SerializationOptions? options)
            {
                value.WriteProtobuf(ref writer, fieldInfo, options);
            }
        }
        public static class ClassWriteOnly<T>
            where T : class, IWriteOnlyProtobufType
        {
            public static void WriteField(scoped ref TWriter writer, T? value, FieldInfo fieldInfo, SerializationOptions? options)
            {
                value?.WriteProtobuf(ref writer, fieldInfo, options);
            }
        }
        public static class Struct<T>
            where T : struct, allows ref struct
        {
            public static void WriteField<THandler>(scoped ref TWriter writer, scoped ref T value, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IStructProtobufTypeWriteOnlyHandler<T>, allows ref struct
            {
                THandler.WriteProtobuf(in value, ref writer, fieldInfo, options);
            }
        }
        public static class Class<T>
                where T : class
        {
            public static void WriteField<THandler>(scoped ref TWriter writer, T? value, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IClassProtobufTypeWriteOnlyHandler<T>, allows ref struct
            {
                THandler.WriteProtobuf(value!, ref writer, fieldInfo, options);
            }
        }
    }
    public static class ClassBinaryWriter<TWriter>
        where TWriter : class, IClassBinaryWriter<TWriter>
    {
        public static class StructCollection<T>
            where T : struct, IProtobufFieldCollection, allows ref struct
        {
            public static void WriteField(TWriter writer, scoped ref T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.WriteProtobufBody(writer, fieldInfo, options);
            }
        }
        public static class ClassCollection<T>
            where T : class, IProtobufFieldCollection
        {
            public static void WriteField(TWriter writer, T collection, FieldInfo fieldInfo, SerializationOptions? options)
            {
                collection.WriteProtobufBody(writer, fieldInfo, options);
            }
        }
        public static class StructWriteOnly<T>
            where T : struct, IWriteOnlyProtobufType, allows ref struct
        {
            public static void WriteField(TWriter writer, scoped ref T value, FieldInfo fieldInfo, SerializationOptions? options)
            {
                value.WriteProtobuf(writer, fieldInfo, options);
            }
        }
        public static class ClassWriteOnly<T>
            where T : class, IWriteOnlyProtobufType
        {
            public static void WriteField(TWriter writer, T? value, FieldInfo fieldInfo, SerializationOptions? options)
            {
                value?.WriteProtobuf(writer, fieldInfo, options);
            }
        }
        public static class Struct<T>
            where T : struct, allows ref struct
        {
            public static void WriteField<THandler>(TWriter writer, scoped ref T value, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IStructProtobufTypeWriteOnlyHandler<T>, allows ref struct
            {
                THandler.WriteProtobuf(in value, writer, fieldInfo, options);
            }
        }
        public static class Class<T>
            where T : class
        {
            public static void WriteField<THandler>(TWriter writer, T? value, FieldInfo fieldInfo, SerializationOptions? options)
                where THandler : IClassProtobufTypeWriteOnlyHandler<T>, allows ref struct
            {
                THandler.WriteProtobuf(value!, writer, fieldInfo, options);
            }
        }
    }
}