using Myitian.LiteProtobuf.CompileTimeSourceGeneration;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public static partial class ProtobufUtility
{
    public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

    public static void WriteTag<T>(scoped ref T writer, int index, WireType wireType)
        where T : IBinaryWriter, allows ref struct
    {
        writer.WriteVarInt(((long)index << 3) | (long)wireType);
    }
    public static void ReadTag<T>(scoped ref T reader, out int index, out WireType wireType)
        where T : IBinaryReader, allows ref struct
    {
        long id = reader.ReadVarInt<long>();
        index = (int)(id >> 3);
        wireType = (WireType)(id & 0b111);
    }
    public static bool TryReadTag<T>(scoped ref T reader, out int index, out WireType wireType, out ParseStatus status)
        where T : IBinaryReader, allows ref struct
    {
        if (reader.TryReadVarInt(out long id, out status))
        {
            index = (int)(id >> 3);
            wireType = (WireType)(id & 0b111);
            return true;
        }
        index = default;
        wireType = default;
        return false;
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
    public static long CountVarIntZigZagSize<T>(ReadOnlySpan<T> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        long result = 0;
        foreach (T it in value)
            result += CountVarIntSize(EncodeZigZag(it));
        return result;
    }
    public static long CountVarIntZigZagSize<T>(IEnumerable<T> value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        long result = 0;
        foreach (T it in value)
            result += CountVarIntSize(EncodeZigZag(it));
        return result;
    }

    [WriteRepeated(nameof(RepeatedItemType.VarInt))]
    public static partial void WriteRepeatedVarInt<T, TWriter>(scoped ref TWriter writer, int index, ReadOnlySpan<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : IBinaryInteger<T>
        where TWriter : IBinaryWriter, allows ref struct;
    [WriteRepeated(nameof(RepeatedItemType.VarInt))]
    public static partial void WriteRepeatedVarInt<T, TWriter>(scoped ref TWriter writer, int index, IEnumerable<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : IBinaryInteger<T>
        where TWriter : IBinaryWriter, allows ref struct;

    [WriteRepeated(nameof(RepeatedItemType.VarIntZigZag))]
    public static partial void WriteRepeatedVarIntZigZag<T, TWriter>(scoped ref TWriter writer, int index, ReadOnlySpan<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where TWriter : IBinaryWriter, allows ref struct;
    [WriteRepeated(nameof(RepeatedItemType.VarIntZigZag))]
    public static partial void WriteRepeatedVarIntZigZag<T, TWriter>(scoped ref TWriter writer, int index, IEnumerable<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where TWriter : IBinaryWriter, allows ref struct;

    [WriteRepeated(nameof(RepeatedItemType.Fixed32))]
    public static partial void WriteRepeatedFixed32<T, TWriter>(scoped ref TWriter writer, int index, ReadOnlySpan<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : struct
        where TWriter : IBinaryWriter, allows ref struct;
    [WriteRepeated(nameof(RepeatedItemType.Fixed32))]
    public static partial void WriteRepeatedFixed32<T, TWriter>(scoped ref TWriter writer, int index, IEnumerable<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : struct
        where TWriter : IBinaryWriter, allows ref struct;

    [WriteRepeated(nameof(RepeatedItemType.Fixed64))]
    public static partial void WriteRepeatedFixed64<T, TWriter>(scoped ref TWriter writer, int index, ReadOnlySpan<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : struct
        where TWriter : IBinaryWriter, allows ref struct;
    [WriteRepeated(nameof(RepeatedItemType.Fixed64))]
    public static partial void WriteRepeatedFixed64<T, TWriter>(scoped ref TWriter writer, int index, IEnumerable<T> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where T : struct
        where TWriter : IBinaryWriter, allows ref struct;

    [WriteRepeated(nameof(RepeatedItemType.Bool))]
    public static partial void WriteRepeatedBool<TWriter>(scoped ref TWriter writer, int index, ReadOnlySpan<bool> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where TWriter : IBinaryWriter, allows ref struct;
    [WriteRepeated(nameof(RepeatedItemType.Bool))]
    public static partial void WriteRepeatedBool<TWriter>(scoped ref TWriter writer, int index, IEnumerable<bool> value, RepeatedEncoding repeatedEncoding = RepeatedEncoding.Auto)
        where TWriter : IBinaryWriter, allows ref struct;

    [ReadRepeated(nameof(RepeatedItemType.VarInt))]
    public static partial bool ReadRepeatedVarInt<T, TCollection, TReader>(scoped ref TReader reader, WireType wireType, TCollection destination, out ParseStatus status)
        where T : IBinaryInteger<T>
        where TCollection : ICollection<T>, allows ref struct
        where TReader : IBinaryReader<TReader>, allows ref struct;
    [ReadRepeated(nameof(RepeatedItemType.VarIntZigZag))]
    public static partial bool ReadRepeatedVarIntZigZag<T, TCollection, TReader>(scoped ref TReader reader, WireType wireType, TCollection destination, out ParseStatus status)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        where TCollection : ICollection<T>, allows ref struct
        where TReader : IBinaryReader<TReader>, allows ref struct;
    [ReadRepeated(nameof(RepeatedItemType.Fixed32))]
    public static partial bool ReadRepeatedFixed32<T, TCollection, TReader>(scoped ref TReader reader, WireType wireType, TCollection destination, out ParseStatus status)
        where T : struct
        where TCollection : ICollection<T>, allows ref struct
        where TReader : IBinaryReader<TReader>, allows ref struct;
    [ReadRepeated(nameof(RepeatedItemType.Fixed64))]
    public static partial bool ReadRepeatedFixed64<T, TCollection, TReader>(scoped ref TReader reader, WireType wireType, TCollection destination, out ParseStatus status)
        where T : struct
        where TCollection : ICollection<T>, allows ref struct
        where TReader : IBinaryReader<TReader>, allows ref struct;
    public static bool ReadRepeatedBool<TCollection, TReader>(scoped ref TReader reader, WireType wireType, TCollection destination, out ParseStatus status)
        where TCollection : ICollection<bool>, allows ref struct
        where TReader : IBinaryReader<TReader>, allows ref struct
    {
        switch (wireType)
        {
            case WireType.VarInt:
                if (!reader.TryReadBool(out bool value, out status))
                    return false;
                destination.Add(value);
                return true;
            case WireType.LengthDelimited:
                if (!TReader.TryCreateLengthDelimitedReader(ref reader, out TReader? subReader, out status))
                {
                    subReader?.Dispose();
                    return false;
                }
                using (subReader)
                {
                    ParseStatus subStatus;
                    while (subReader.TryReadBool(out value, out subStatus))
                        destination.Add(value);
                    if (subStatus is ParseStatus.ExactEndOfStream)
                        return true;
                }
                goto default;
            default:
                status = ParseStatus.InvalidData;
                return false;
        }
    }
}
