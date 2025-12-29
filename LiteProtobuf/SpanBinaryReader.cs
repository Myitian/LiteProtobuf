using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace Myitian.LiteProtobuf;

public ref struct SpanBinaryReader(ReadOnlySpan<byte> bytes) : IStructBinaryReader<SpanBinaryReader>
{
    public ReadOnlySpan<byte> Span { get; } = bytes;
    public int Position { get; set; }

    public byte ReadByte()
        => BinaryReaderExtension.ReadByte(ref this);
    public bool TryReadByte(out byte result, out ParseStatus status)
    {
        long nextPos = Position + (long)sizeof(byte);
        if (nextPos > Span.Length)
        {
            result = default;
            status = ParseStatus.ExactEndOfStream;
            return false;
        }
        result = Span[Position];
        Position = (int)nextPos;
        status = ParseStatus.Success;
        return true;
    }
    public T ReadFixed32<T>()
        where T : struct
        => BinaryReaderExtension.ReadFixed32<SpanBinaryReader, T>(ref this);
    public bool TryReadFixed32<T>(out T result, out ParseStatus status)
        where T : struct
        => BinaryReaderExtension.TryReadFixed32(ref this, out result, out status);
    public T ReadFixed64<T>()
        where T : struct
        => BinaryReaderExtension.ReadFixed64<SpanBinaryReader, T>(ref this);
    public bool TryReadFixed64<T>(out T result, out ParseStatus status)
        where T : struct
        => BinaryReaderExtension.TryReadFixed64(ref this, out result, out status);
    public T ReadVarInt<T>()
        where T : IBinaryInteger<T>
        => BinaryReaderExtension.ReadVarInt<SpanBinaryReader, T>(ref this);
    public bool TryReadVarInt<T>(out T result, out ParseStatus status)
        where T : IBinaryInteger<T>
        => BinaryReaderExtension.TryReadVarInt(ref this, out result, out status);
    public T ReadVarIntZigZag<T>()
        where T : IBinaryInteger<T>, ISignedNumber<T>
        => BinaryReaderExtension.ReadVarIntZigZag<SpanBinaryReader, T>(ref this);
    public bool TryReadVarIntZigZag<T>(out T result, out ParseStatus status)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        => BinaryReaderExtension.TryReadVarIntZigZag(ref this, out result, out status);
    public bool ReadBool()
        => BinaryReaderExtension.ReadBool(ref this);
    public bool TryReadBool(out bool result, out ParseStatus status)
        => BinaryReaderExtension.TryReadBool(ref this, out result, out status);
    public ReadOnlySpan<byte> ReadRawBlock(int length)
        => BinaryReaderExtension.ReadRawBlock(ref this, length);
    public bool TryReadRawBlock(int length, out ReadOnlySpan<byte> block, out ParseStatus status)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        long nextPos = Position + (long)length;
        long diff = nextPos - Span.Length;
        if (diff > 0)
        {
            status = Position == Span.Length ? ParseStatus.ExactEndOfStream : ParseStatus.EndOfStream;
            block = default;
            return false;
        }
        ReadOnlySpan<byte> result = Span.Slice(Position, length);
        Position = (int)nextPos;
        status = ParseStatus.Success;
        block = result;
        return true;
    }
    public void SkipRawBlock(long length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        long nextPos = Position + length;
        if (nextPos > Span.Length)
            throw new EndOfStreamException();
        Position = (int)nextPos;
    }
    public int ReadRawBlock(scoped Span<byte> buffer, bool readExactly = false)
    {
        int read = Math.Min(Span.Length - Position, buffer.Length);
        if (readExactly && read < buffer.Length)
            throw new EndOfStreamException();
        Span.Slice(Position, read).CopyTo(buffer);
        Position += read;
        return read;
    }
    public ReadOnlySpan<byte> ReadLengthDelimited()
        => BinaryReaderExtension.ReadLengthDelimited(ref this);
    public bool TryReadLengthDelimited(out ReadOnlySpan<byte> result, out ParseStatus status)
    {
        if (TryReadVarInt(out long length, out status))
        {
            if (length < 0)
                status = ParseStatus.InvalidData;
            else if (length > int.MaxValue)
                status = ParseStatus.NotSupported;
            else if (TryReadRawBlock((int)length, out result, out status))
                return true;
            else if (status == ParseStatus.ExactEndOfStream)
                status = ParseStatus.EndOfStream;
        }
        result = [];
        return false;
    }
    public void SkipLengthDelimited()
        => BinaryReaderExtension.SkipLengthDelimited(ref this);
    public bool TryReadByteArray(out byte[] value, out ParseStatus status)
    {
        if (TryReadVarInt(out long length, out status))
        {
            if (length < 0)
                status = ParseStatus.InvalidData;
            else if (length > Array.MaxLength)
                status = ParseStatus.NotSupported;
            else
            {
                byte[] buffer = GC.AllocateUninitializedArray<byte>((int)length);
                int read = ReadRawBlock(buffer, false);
                value = buffer;
                if (read == length)
                {
                    status = ParseStatus.Success;
                    return true;
                }
                else
                {
                    status = ParseStatus.EndOfStream;
                    return false;
                }
            }
        }
        value = [];
        return false;
    }
    public byte[] ReadByteArray()
        => BinaryReaderExtension.ReadByteArray(ref this);
    public void ReadByteArray(List<byte> destination)
    {
        destination.AddRange(ReadLengthDelimited());
    }
    public string ReadString(Encoding? encoding = null)
    {
        return (encoding ?? ProtobufUtility.DefaultEncoding).GetString(ReadLengthDelimited());
    }
    public readonly void Dispose()
    {
    }

    public static SpanBinaryReader CreateLengthDelimitedReader(ref SpanBinaryReader parent)
    {
        return new(parent.ReadLengthDelimited());
    }
    public static bool TryCreateLengthDelimitedReader(ref SpanBinaryReader parent, [NotNullWhen(true)] out SpanBinaryReader subReader, out ParseStatus status)
    {
        if (parent.TryReadLengthDelimited(out ReadOnlySpan<byte> span, out status))
        {
            subReader = new(span);
            return true;
        }
        subReader = default;
        return false;
    }
}