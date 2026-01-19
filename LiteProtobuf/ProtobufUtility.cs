using Myitian.LiteProtobuf.Serialization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public static partial class ProtobufUtility
{
    public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

    extension<TReader>(scoped ref TReader reader)
        where TReader : struct, IBinaryReader, allows ref struct
    {
        public void ReadTag(out int number, out WireType wireType)
        {
            long id = reader.ReadVarInt<long>();
            number = (int)(id >> 3);
            wireType = (WireType)(id & 0b111);
        }
        public bool TryReadTag(out int number, out WireType wireType, out ParseStatus status)
        {
            if (reader.TryReadVarInt(out long id, out status))
            {
                number = (int)(id >> 3);
                wireType = (WireType)(id & 0b111);
                return true;
            }
            number = default;
            wireType = default;
            return false;
        }
        public bool TryReadTag(out FieldInfo fieldInfo, out ParseStatus status)
        {
            if (reader.TryReadVarInt(out long id, out status))
            {
                fieldInfo = new()
                {
                    Number = (int)(id >> 3),
                    ReceivedWireType = (WireType)(id & 0b111)
                };
                return true;
            }
            fieldInfo = default;
            return false;
        }
    }
    extension<TReader>(TReader reader)
        where TReader : class, IBinaryReader
    {
        public void ReadTag(out int number, out WireType wireType)
        {
            long id = reader.ReadVarInt<long>();
            number = (int)(id >> 3);
            wireType = (WireType)(id & 0b111);
        }
        public bool TryReadTag(out int number, out WireType wireType, out ParseStatus status)
        {
            if (reader.TryReadVarInt(out long id, out status))
            {
                number = (int)(id >> 3);
                wireType = (WireType)(id & 0b111);
                return true;
            }
            number = default;
            wireType = default;
            return false;
        }
        public bool TryReadTag(out FieldInfo fieldInfo, out ParseStatus status)
        {
            if (reader.TryReadVarInt(out long id, out status))
            {
                fieldInfo = new()
                {
                    Number = (int)(id >> 3),
                    ReceivedWireType = (WireType)(id & 0b111)
                };
                return true;
            }
            fieldInfo = default;
            return false;
        }
    }
    extension<TWriter>(scoped ref TWriter writer)
        where TWriter : struct, IBinaryWriter, allows ref struct
    {
        public void WriteTag(int number, WireType wireType)
        {
            writer.WriteVarInt(((long)number << 3) | (long)wireType);
        }
    }
    extension<TWriter>(TWriter writer)
        where TWriter : class, IBinaryWriter
    {
        public void WriteTag(int number, WireType wireType)
        {
            writer.WriteVarInt(((long)number << 3) | (long)wireType);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T EncodeZigZag<T>(T value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        return (value << 1) ^ (value >> (int.CreateChecked(T.PopCount(T.AllBitsSet)) - 1));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T DecodeZigZag<T>(T value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        return (value >> 1) ^ -(value & T.One);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountVarIntSize<T>(T value)
        where T : IBinaryInteger<T>
    {
        if (T.IsZero(value))
            return 1;
        return (6 + int.CreateChecked(T.PopCount(T.AllBitsSet)) - int.CreateChecked(T.LeadingZeroCount(value))) / 7;
    }
    public static long CountVarIntSize<T>(ReadOnlySpan<T> value)
        where T : IBinaryInteger<T>
    {
        long result = 0;
        foreach (T it in value)
            result += CountVarIntSize(it);
        return result;
    }
    public static long CountVarIntSize<T>(IEnumerable<T> value)
        where T : IBinaryInteger<T>
    {
        long result = 0;
        foreach (T it in value)
            result += CountVarIntSize(it);
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountVarIntZigZagSize<T>(T value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        if (T.IsZero(value))
            return 1;
        return CountVarIntSize(EncodeZigZag(value));
    }
    public static long CountVarIntZigZagSize<T>(ReadOnlySpan<T> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        long result = 0;
        foreach (T it in value)
            result += CountVarIntZigZagSize(it);
        return result;
    }
    public static long CountVarIntZigZagSize<T>(IEnumerable<T> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        long result = 0;
        foreach (T it in value)
            result += CountVarIntZigZagSize(it);
        return result;
    }
}