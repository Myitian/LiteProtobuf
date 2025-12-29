using Myitian.LiteProtobuf.CompileTimeSourceGeneration;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Myitian.LiteProtobuf;

#pragma warning disable IDE0002,IDE0003 // keep `this.` and `IBinaryReader.` to simplify the source generator
[ShadowVirtualMethodBodyTo(typeof(BinaryReaderExtension), "TReader", "reader")]
public interface IBinaryReader : IDisposable
{
    public static T ThrowIfNotSuccess<T>(T value, ParseStatus status)
        where T : allows ref struct
    {
        return status switch
        {
            ParseStatus.Success => value,
            ParseStatus.ExactEndOfStream or ParseStatus.EndOfStream => throw new EndOfStreamException(),
            ParseStatus.InvalidData => throw new InvalidDataException(),
            ParseStatus.NotSupported => throw new NotSupportedException(),
            _ => throw new Exception(status.ToString())
        };
    }
    public virtual byte ReadByte()
    {
        return this.TryReadByte(out byte value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    bool TryReadByte(out byte result, out ParseStatus status);
    public virtual T ReadFixed32<T>() where T : struct
    {
        return this.TryReadFixed32(out T value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }

    public virtual bool TryReadFixed32<T>(out T result, out ParseStatus status) where T : struct
    {
        const int bufSize = sizeof(uint);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed32", nameof(T));
        Span<byte> buffer = stackalloc byte[bufSize];
        int read = this.ReadRawBlock(buffer, false);
        if (read != bufSize)
        {
            result = default;
            status = read == 0 ? ParseStatus.ExactEndOfStream : ParseStatus.EndOfStream;
            return false;
        }
        if (!BitConverter.IsLittleEndian)
            buffer[..size].Reverse();
        result = MemoryMarshal.Read<T>(buffer);
        status = ParseStatus.Success;
        return true;
    }
    public virtual T ReadFixed64<T>() where T : struct
    {
        return this.TryReadFixed64(out T value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    public virtual bool TryReadFixed64<T>(out T result, out ParseStatus status) where T : struct
    {
        const int bufSize = sizeof(ulong);
        int size = Unsafe.SizeOf<T>();
        if (size > bufSize)
            throw new ArgumentException("T cannot fit into protobuf fixed64", nameof(T));
        Span<byte> buffer = stackalloc byte[bufSize];
        int read = this.ReadRawBlock(buffer, false);
        if (read != bufSize)
        {
            result = default;
            status = read == 0 ? ParseStatus.ExactEndOfStream : ParseStatus.EndOfStream;
            return false;
        }
        if (!BitConverter.IsLittleEndian)
            buffer[..size].Reverse();
        result = MemoryMarshal.Read<T>(buffer);
        status = ParseStatus.Success;
        return true;
    }
    public virtual T ReadVarInt<T>() where T : IBinaryInteger<T>
    {
        return this.TryReadVarInt(out T value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    public virtual bool TryReadVarInt<T>(out T result, out ParseStatus status)
        where T : IBinaryInteger<T>
    {
        result = T.Zero;
        byte byteReadJustNow;
        const int MaxBytesWithoutOverflow = 9;
        for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
        {
            if (!this.TryReadByte(out byteReadJustNow, out _))
            {
                status = shift == 0 ? ParseStatus.ExactEndOfStream : ParseStatus.EndOfStream;
                return false;
            }
            result |= T.CreateTruncating(byteReadJustNow & 0x7F) << shift;
            if (byteReadJustNow <= 0x7Fu)
            {
                status = ParseStatus.Success;
                return true;
            }
        }
        if (!this.TryReadByte(out byteReadJustNow, out _))
        {
            status = ParseStatus.EndOfStream;
            return false;
        }
        if (byteReadJustNow > 0x7Fu)
        {
            status = ParseStatus.InvalidData;
            return false;
        }
        result |= T.CreateTruncating(byteReadJustNow) << (MaxBytesWithoutOverflow * 7);
        status = ParseStatus.Success;
        return true;
    }
    public virtual T ReadVarIntZigZag<T>() where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        return this.TryReadVarIntZigZag(out T value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    public virtual bool TryReadVarIntZigZag<T>(out T result, out ParseStatus status) where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        if (this.TryReadVarInt(out long value, out status))
        {
            result = T.CreateTruncating(ProtobufUtility.DecodeZigZag(value));
            return true;
        }
        result = T.Zero;
        return false;
    }
    public virtual bool ReadBool()
    {
        return this.TryReadBool(out bool value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    public virtual bool TryReadBool(out bool result, out ParseStatus status)
    {
        bool b = this.TryReadVarInt(out ulong value, out status);
        result = value != 0;
        return b;
    }
    public virtual ReadOnlySpan<byte> ReadRawBlock(int length)
    {
        return this.TryReadRawBlock(length, out ReadOnlySpan<byte> value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    bool TryReadRawBlock(int length, scoped out ReadOnlySpan<byte> value, out ParseStatus status);
    void SkipRawBlock(long length);
    int ReadRawBlock(scoped Span<byte> buffer, bool readExactly = false);
    public virtual ReadOnlySpan<byte> ReadLengthDelimited()
    {
        return this.TryReadLengthDelimited(out ReadOnlySpan<byte> value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    bool TryReadLengthDelimited(scoped out ReadOnlySpan<byte> value, out ParseStatus status);
    public virtual void SkipLengthDelimited()
    {
        long length = this.ReadVarInt<long>();
        if (length < 0)
            throw new InvalidDataException();
        this.SkipRawBlock(length);
    }
    public virtual byte[] ReadByteArray()
    {
        return this.TryReadByteArray(out byte[] value, out ParseStatus status) ? value : IBinaryReader.ThrowIfNotSuccess(value, status);
    }
    bool TryReadByteArray(out byte[] value, out ParseStatus status);
    void ReadByteArray(List<byte> destination);
    string ReadString(Encoding? encoding = null);
}
#pragma warning restore IDE0002,IDE0003
public interface IStructBinaryReader<TReader> : IBinaryReader
    where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
{
    public static abstract TReader CreateLengthDelimitedReader(ref TReader parent);
    public static abstract bool TryCreateLengthDelimitedReader(ref TReader parent, [NotNullWhen(true)] out TReader subReader, out ParseStatus status);
}
public interface IClassBinaryReader<TReader> : IBinaryReader
    where TReader : class, IClassBinaryReader<TReader>
{
    public static abstract TReader CreateLengthDelimitedReader(TReader parent);
    public static abstract bool TryCreateLengthDelimitedReader(TReader parent, [NotNullWhen(true)] out TReader? subReader, out ParseStatus status);
}
public static partial class BinaryReaderExtension
{
}