using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public class StreamBinaryWriter(Stream stream, bool leaveOpen = false) : IBinaryWriter<StreamBinaryWriter>
{
    protected readonly StreamBinaryWriter? _parent;
    public Stream BaseStream { get; } = stream;
    public StreamBinaryWriter(StreamBinaryWriter parent) : this(new MemoryStream(), false)
    {
        _parent = parent;
    }

    public void WriteByte(byte value)
    {
        BaseStream.WriteByte(value);
    }
    public void WriteFixed32<T>(T value)
        where T : struct
    {
        const int bufSize = sizeof(uint);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed32", nameof(T));
        Span<byte> buffer = stackalloc byte[bufSize];
        buffer.Clear();
        MemoryMarshal.Write(buffer, in value);
        if (!BitConverter.IsLittleEndian)
            buffer[..size].Reverse();
        WriteRawBlock(buffer);
    }
    public void WriteFixed64<T>(T value)
        where T : struct
    {
        const int bufSize = sizeof(ulong);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed64", nameof(T));
        Span<byte> buffer = stackalloc byte[bufSize];
        buffer.Clear();
        MemoryMarshal.Write(buffer, in value);
        if (!BitConverter.IsLittleEndian)
            buffer[..size].Reverse();
        WriteRawBlock(buffer);
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
    public void WriteRawBlock(ReadOnlySpan<byte> value)
    {
        BaseStream.Write(value);
    }
    public void WriteLengthDelimited(ReadOnlySpan<byte> value)
    {
        WriteVarInt(value.Length);
        WriteRawBlock(value);
    }
    public void WriteString(ReadOnlySpan<char> value, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        int length = encoding.GetByteCount(value);
        WriteVarInt(length);
        using StreamWriter sw = new(BaseStream, encoding, leaveOpen: true);
        sw.Write(value);
    }

    public void Dispose()
    {
        if (_parent is not null)
        {
            _parent.WriteVarInt(BaseStream.Length);
            BaseStream.Position = 0;
            BaseStream.CopyTo(_parent.BaseStream);
        }
        if (!leaveOpen)
            BaseStream.Dispose();
        GC.SuppressFinalize(this);
    }

    public static StreamBinaryWriter CreateLengthDelimitedWriter(ref StreamBinaryWriter parent)
    {
        return new(parent);
    }
}