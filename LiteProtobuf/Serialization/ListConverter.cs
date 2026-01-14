using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public abstract class GenericListConverter<T>
    : IClassProtobufTypeHandler<List<T>>
{
    public abstract bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options);
    public virtual bool IsFieldInfoValidForInstance(in List<T> value, FieldInfo fieldInfo, SerializationOptions? options)
    {
        return IsFieldInfoValid(fieldInfo, options);
    }
    public virtual bool TryCreateInstance(FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value)
    {
        if (!IsFieldInfoValid(fieldInfo, options))
        {
            value = null;
            return false;
        }
        value = [];
        return true;
    }
    public virtual bool TryCreateFulfilled<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
            .TryCreateFulfilled(this, ref reader, fieldInfo, options, out value, out status);
    }
    public virtual bool TryCreateFulfilled<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, [NotNullWhen(true)] out List<T>? value, out ParseStatus status)
         where TReader : class, IClassBinaryReader<TReader>
    {
        return Defaults.ReadOnlyClassProtobufTypeHandler<List<T>>
            .TryCreateFulfilled(this, reader, fieldInfo, options, out value, out status);
    }
    public abstract bool TryReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public abstract bool TryReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
         where TReader : class, IClassBinaryReader<TReader>;
    public abstract void ReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct;
    public abstract void ReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
         where TReader : class, IClassBinaryReader<TReader>;
    public abstract void WriteProtobuf<TWriter>(List<T> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct;
    public abstract void WriteProtobuf<TWriter>(List<T> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
        where TWriter : class, IClassBinaryWriter<TWriter>;
}
public class NonPackedGenericListConverter<T>
    : GenericListConverter<T>
    where T : IProtobufType<T>
{
    public override bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return T.IsFieldInfoValid(fieldInfo, options);
    }
    public override bool TryReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        if (!T.TryCreateFulfilled(ref reader, fieldInfo, options, out T? value, out status))
            return false;
        self.Add(value);
        return true;
    }
    public override bool TryReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        if (!T.TryCreateFulfilled(reader, fieldInfo, options, out T? value, out status))
            return false;
        self.Add(value);
        return true;
    }
    public override void ReadProtobuf<TReader>(List<T> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        T value = T.CreateFulfilled(ref reader, fieldInfo, options);
        self.Add(value);
    }
    public override void ReadProtobuf<TReader>(List<T> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        T value = T.CreateFulfilled(reader, fieldInfo, options);
        self.Add(value);
    }
    public override void WriteProtobuf<TWriter>(List<T> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        foreach (T item in self)
            item.WriteProtobuf(ref writer, fieldInfo, options);
    }
    public override void WriteProtobuf<TWriter>(List<T> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        foreach (T item in self)
            item.WriteProtobuf(writer, fieldInfo, options);
    }
}
public class BooleanGenericListConverter<T>
    : GenericListConverter<bool>
{
    public override bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        return fieldInfo.ReceivedWireType is WireType.LengthDelimited or WireType.VarInt;
    }
    public override bool TryReadProtobuf<TReader>(List<bool> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        return ProtobufUtility.TryReadRepeatedBool(ref reader, fieldInfo.ReceivedWireType, self, out status);
    }
    public override bool TryReadProtobuf<TReader>(List<bool> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        return ProtobufUtility.TryReadRepeatedBool(reader, fieldInfo.ReceivedWireType, self, out status);
    }
    public override void ReadProtobuf<TReader>(List<bool> self, scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
            .ReadProtobuf(this, self, ref reader, fieldInfo, options);
    }
    public override void ReadProtobuf<TReader>(List<bool> self, TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        Defaults.ReadOnlyClassProtobufTypeHandler<List<bool>>
            .ReadProtobuf(this, self, reader, fieldInfo, options);
    }
    public override void WriteProtobuf<TWriter>(List<bool> self, scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        ProtobufUtility.WriteRepeatedBool(ref writer, fieldInfo.Index, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
    }
    public override void WriteProtobuf<TWriter>(List<bool> self, TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        ProtobufUtility.WriteRepeatedBool(writer, fieldInfo.Index, self, fieldInfo.FieldTypeHint.GetRepeatedEncoding());
    }
}