namespace Myitian.LiteProtobuf.Serialization;

public class GenericListConverter<T, TSerializer> : IProtobufSerializable<List<T>>
    where TSerializer : IProtobufSerializable<T>
{
    public static bool Serialize<TWriter>(ref TWriter reader, in List<T> value, WireType type, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto) where TWriter : IBinaryReader, allows ref struct
    {
        throw new NotImplementedException();
    }
    public static List<T> Deserialize<TReader>(ref TReader reader, WireType readedType, WireType realType) where TReader : IBinaryReader, allows ref struct
    {
        throw new NotImplementedException();
    }
    public static bool TryDeserialize<TReader>(ref TReader reader, out List<T> result, WireType readedType, WireType realType) where TReader : IBinaryReader, allows ref struct
    {
        throw new NotImplementedException();
    }
}

public interface IProtobufSerializable<T>
    where T : allows ref struct
{
    public static abstract bool Serialize<TWriter>(ref TWriter reader, in T value, WireType type, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where TWriter : IBinaryReader, allows ref struct;
    public static abstract T Deserialize<TReader>(ref TReader reader, WireType readedType, WireType realType)
        where TReader : IBinaryReader, allows ref struct;
    public static abstract bool TryDeserialize<TReader>(ref TReader reader, out T result, WireType readedType, WireType realType)
        where TReader : IBinaryReader, allows ref struct;
}