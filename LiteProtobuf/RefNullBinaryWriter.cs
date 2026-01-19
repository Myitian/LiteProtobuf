using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public ref struct RefNullBinaryWriter : IStructBinaryWriter<RefNullBinaryWriter>
{
    private readonly ref ulong _parentLength = ref Unsafe.NullRef<ulong>();
    public ulong Length = 0;
    public RefNullBinaryWriter(ref RefNullBinaryWriter parent)
    {
        _parentLength = ref parent.Length;
    }
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

    public readonly void Dispose()
    {
        if (!Unsafe.IsNullRef(ref _parentLength))
            _parentLength += (ulong)ProtobufUtility.CountVarIntSize(Length) + Length;
    }

    public static RefNullBinaryWriter CreateLengthDelimitedWriter(ref RefNullBinaryWriter parent)
    {
        return new(ref parent);
    }
}