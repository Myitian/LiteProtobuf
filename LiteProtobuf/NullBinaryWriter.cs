using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public class NullBinaryWriter(NullBinaryWriter? parent = null) : IClassBinaryWriter<NullBinaryWriter>
{
    public NullBinaryWriter? Parent { get; } = parent;
    public ulong Length { get; set; } = 0;
    public void WriteByte(byte value)
    {
        Length++;
    }
    public void WriteFixed32<T>(T value)
        where T : struct
    {
        const int bufSize = sizeof(uint);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed32", nameof(T));
        Length += bufSize;
    }
    public void WriteFixed64<T>(T value)
        where T : struct
    {
        const int bufSize = sizeof(ulong);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed64", nameof(T));
        Length += bufSize;
    }
    public void WriteVarInt<T>(T value)
        where T : IBinaryInteger<T>
    {
        if (value.GetByteCount() > sizeof(long))
            throw new ArgumentException("T cannot fit into protobuf varint", nameof(T));
        Length += (ulong)ProtobufUtility.CountVarIntSize(value);
    }
    public void WriteVarIntZigZag<T>(T value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        if (value.GetByteCount() > sizeof(long))
            throw new ArgumentException("T cannot fit into protobuf varint(zigzag)", nameof(T));
        Length += (ulong)ProtobufUtility.CountVarIntZigZagSize(value);
    }
    public void WriteBool(bool value)
    {
        Length++;
    }
    public void WriteRawBlock(ReadOnlySpan<byte> value)
    {
        Length += (uint)value.Length;
    }
    public void WriteLengthDelimited(ReadOnlySpan<byte> value)
    {
        Length += (uint)ProtobufUtility.CountVarIntSize(value) + (uint)value.Length;
    }
    public void WriteString(ReadOnlySpan<char> value, Encoding? encoding = null)
    {
        int length = (encoding ?? Encoding.UTF8).GetByteCount(value);
        Length += (uint)ProtobufUtility.CountVarIntSize(length) + (uint)length;
    }

    public void Dispose()
    {
        if (Parent is not null)
            Parent.Length += (ulong)ProtobufUtility.CountVarIntSize(Length) + Length;
        GC.SuppressFinalize(this);
    }

    public static NullBinaryWriter CreateLengthDelimitedWriter(NullBinaryWriter parent)
    {
        return new(parent);
    }
}