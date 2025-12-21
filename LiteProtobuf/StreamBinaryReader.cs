using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Myitian.LiteProtobuf;

public class StreamBinaryReader(Stream stream, bool leaveOpen = false) : IBinaryReader<StreamBinaryReader>
{
    protected readonly StreamBinaryReader? _parent;
    protected long _length = long.MaxValue;
    public Stream BaseStream { get; } = stream;
    public StreamBinaryReader(StreamBinaryReader parent, long length)
        : this(new LengthLimitedStream(parent.BaseStream, length, true), false)
    {
        _parent = parent;
        _length = length;
    }

    public bool TryReadByte(out byte result, out ParseStatus status)
    {
        int read = BaseStream.ReadByte();
        if (read < 0)
        {
            result = default;
            status = ParseStatus.ExactEndOfStream;
            return false;
        }
        _length--;
        result = (byte)read;
        status = ParseStatus.Success;
        return true;
    }
    public bool TryReadRawBlock(int length, out ReadOnlySpan<byte> value, out ParseStatus status)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, Array.MaxLength);
        byte[] buffer = GC.AllocateUninitializedArray<byte>(length);
        int read = ReadRawBlock(buffer, false);
        value = buffer;
        if (read == length)
        {
            status = ParseStatus.Success;
            return true;
        }
        else if (read == 0)
        {
            status = ParseStatus.ExactEndOfStream;
            return false;
        }
        else
        {
            status = ParseStatus.EndOfStream;
            return false;
        }
    }
    public ReadOnlySpan<byte> ReadRawBlock(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, Array.MaxLength);
        byte[] buffer = GC.AllocateUninitializedArray<byte>(length);
        ReadRawBlock(buffer, true);
        return buffer;
    }
    public void SkipRawBlock(long length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        const int BufferSize = 128;
        Span<byte> buffer = stackalloc byte[BufferSize];
        while (length >= BufferSize)
        {
            ReadRawBlock(buffer, true);
            length -= BufferSize;
        }
        ReadRawBlock(buffer[..(int)length], true);
    }
    public int ReadRawBlock(Span<byte> buffer, bool readExactly = false)
    {
        int read = BaseStream.ReadAtLeast(buffer, buffer.Length, readExactly);
        _length -= read;
        return read;
    }
    public bool TryReadLengthDelimited(out ReadOnlySpan<byte> value, out ParseStatus status)
    {
        if (((IBinaryReader)this).TryReadVarInt(out long length, out status))
        {
            if (length < 0)
                status = ParseStatus.InvalidData;
            else if (length > Array.MaxLength)
                status = ParseStatus.NotSupported;
            else if (TryReadRawBlock((int)length, out value, out status))
                return true;
            else if (status == ParseStatus.ExactEndOfStream)
                status = ParseStatus.EndOfStream;
        }
        value = [];
        return false;
    }
    public bool TryReadByteArray(out byte[] value, out ParseStatus status)
    {
        if (((IBinaryReader)this).TryReadVarInt(out long length, out status))
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
    public void ReadByteArray(List<byte> destination)
    {
        long length = ((IBinaryReader)this).ReadVarInt<long>();
        if (length < 0)
            throw new InvalidDataException();
        const int BufferSize = 128;
        Span<byte> buffer = stackalloc byte[BufferSize];
        while (length >= BufferSize)
        {
            ReadRawBlock(buffer, true);
            destination.AddRange(buffer);
            length -= BufferSize;
        }
        buffer = buffer[..(int)length];
        ReadRawBlock(buffer, true);
        destination.AddRange(buffer);
    }
    public string ReadString(Encoding? encoding = null)
    {
        StreamBinaryReader self = this;
        using StreamBinaryReader subReader = CreateLengthDelimitedReader(ref self);
        using StreamReader reader = new(subReader.BaseStream, encoding ?? ProtobufUtility.DefaultEncoding);
        return reader.ReadToEnd();
    }

    public void Dispose()
    {
        _parent?.SkipRawBlock(_length);
        if (!leaveOpen)
            BaseStream.Dispose();
        GC.SuppressFinalize(this);
    }

    public static StreamBinaryReader CreateLengthDelimitedReader(ref StreamBinaryReader parent)
    {
        long length = ((IBinaryReader)parent).ReadVarInt<long>();
        LengthLimitedStream limitedStream = new(parent.BaseStream, length, true);
        return new(limitedStream, false);
    }
    public static bool TryCreateLengthDelimitedReader(ref StreamBinaryReader parent, [NotNullWhen(true)] out StreamBinaryReader? subReader, out ParseStatus status)
    {
        if (((IBinaryReader)parent).TryReadVarInt(out long length, out status))
        {
            LengthLimitedStream limitedStream = new(parent.BaseStream, length, true);
            subReader = new(limitedStream, false);
            return true;
        }
        subReader = default;
        return false;
    }
}