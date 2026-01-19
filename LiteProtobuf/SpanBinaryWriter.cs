using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public ref struct SpanBinaryWriter(Span<byte> bytes) : IStructBinaryWriter<SpanBinaryWriter>
{
    private readonly ref int _parentPosition = ref Unsafe.NullRef<int>();
    private int _position = 0;
    public readonly Span<byte> Span { get; } = bytes;
    public readonly int Position => _position;
    public SpanBinaryWriter(ref SpanBinaryWriter parent) : this(parent.Span[parent._position..])
    {
        _parentPosition = ref parent._position;
    }

    public void WriteByte(byte value)
    {
        long nextPos = _position + (long)sizeof(byte);
        if (nextPos > Span.Length)
            throw new EndOfStreamException();
        Span[_position] = value;
        _position = (int)nextPos;
    }
    public void WriteFixed32<T>(T value)
        where T : struct
    {
        const int bufSize = sizeof(int);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed32", nameof(T));
        long nextPos = _position + (long)bufSize;
        if (nextPos > Span.Length)
            throw new EndOfStreamException();
        Span<byte> buffer = Span.Slice(_position, bufSize);
        buffer.Clear();
        MemoryMarshal.Write(buffer, in value);
        if (!BitConverter.IsLittleEndian)
            buffer[..size].Reverse();
        _position = (int)nextPos;
    }
    public void WriteFixed64<T>(T value)
        where T : struct
    {
        const int bufSize = sizeof(long);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed64", nameof(T));
        long nextPos = _position + (long)bufSize;
        if (nextPos > Span.Length)
            throw new EndOfStreamException();
        Span<byte> buffer = Span.Slice(_position, bufSize);
        buffer.Clear();
        MemoryMarshal.Write(buffer, in value);
        if (!BitConverter.IsLittleEndian)
            buffer[..size].Reverse();
        _position = (int)nextPos;
    }
    public void WriteVarInt<T>(T value)
        where T : IBinaryInteger<T>
    {
        if (value.GetByteCount() > sizeof(long))
            throw new ArgumentException("T cannot fit into protobuf varint", nameof(T));
        T v0x80 = T.CreateChecked(0x80);
        while (value >= v0x80)
        {
            WriteByte(byte.CreateTruncating(value | v0x80));
            value >>>= 7;
        }
        WriteByte(byte.CreateTruncating(value));
    }
    public void WriteVarIntZigZag<T>(T value)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        if (value.GetByteCount() > sizeof(long))
            throw new ArgumentException("T cannot fit into protobuf varint(zigzag)", nameof(T));
        WriteVarInt(ProtobufUtility.EncodeZigZag(value));
    }
    public void WriteBool(bool value)
    {
        WriteByte((byte)(value ? 0 : 1));
    }
    public void WriteRawBlock(scoped ReadOnlySpan<byte> value)
    {
        long nextPos = _position + (long)value.Length;
        if (nextPos > Span.Length)
            throw new EndOfStreamException();
        value.CopyTo(Span[_position..]);
        _position = (int)nextPos;
    }
    public void WriteLengthDelimited(scoped ReadOnlySpan<byte> value)
    {
        WriteVarInt(value.Length);
        WriteRawBlock(value);
    }
    public void WriteString(scoped ReadOnlySpan<char> value, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        int length = encoding.GetByteCount(value);
        WriteVarInt(value.Length);
        long nextPos = _position + length;
        if (nextPos > Span.Length)
            throw new EndOfStreamException();
        encoding.GetBytes(value, Span[_position..]);
        _position = (int)nextPos;
    }

    public void Dispose()
    {
        if (!Unsafe.IsNullRef(ref _parentPosition))
        {
            // flush to parent writer (insert the length)
            int lengthSize = ProtobufUtility.CountVarIntSize(_position);
            _parentPosition += lengthSize + _position;
            Span[.._position].CopyTo(Span[lengthSize..]);
            _position = 0;
            WriteVarInt(lengthSize);
        }
    }

    public static SpanBinaryWriter CreateLengthDelimitedWriter(ref SpanBinaryWriter parent)
    {
        return new(ref parent);
    }
}