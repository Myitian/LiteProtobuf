using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Myitian.LiteProtobuf;

public interface IBinaryReader : IDisposable
{
    public static T ThrowIfNotSuccess<T>(T value, ParseStatus status)
        where T : allows ref struct
    {
        return status switch
        {
            ParseStatus.Success => value,
            _ => throw GetExceptionByStatus(status)
        };
    }
    public static Exception GetExceptionByStatus(ParseStatus status)
    {
        throw status switch
        {
            ParseStatus.ExactEndOfStream or ParseStatus.EndOfStream => new EndOfStreamException(),
            ParseStatus.InvalidData => new InvalidDataException(),
            ParseStatus.NotSupported => new NotSupportedException(),
            _ => new Exception(status.ToString())
        };
    }
    public virtual byte ReadByte()
        => Defaults.ReadByte(this);
    bool TryReadByte(out byte result, out ParseStatus status);
    public virtual T ReadFixed32<T>()
        where T : struct
        => Defaults.ReadFixed32<IBinaryReader, T>(this);
    public virtual bool TryReadFixed32<T>(out T result, out ParseStatus status)
        where T : struct
        => Defaults.TryReadFixed32(this, out result, out status);
    public virtual T ReadFixed64<T>()
        where T : struct
        => Defaults.ReadFixed64<IBinaryReader, T>(this);
    public virtual bool TryReadFixed64<T>(out T result, out ParseStatus status)
        where T : struct
        => Defaults.TryReadFixed64(this, out result, out status);
    public virtual T ReadVarInt<T>()
        where T : IBinaryInteger<T>
        => Defaults.ReadVarInt<IBinaryReader, T>(this);
    public virtual bool TryReadVarInt<T>(out T result, out ParseStatus status)
        where T : IBinaryInteger<T>
        => Defaults.TryReadVarInt(this, out result, out status);
    public virtual T ReadVarIntZigZag<T>()
        where T : IBinaryInteger<T>, ISignedNumber<T>
        => Defaults.ReadVarIntZigZag<IBinaryReader, T>(this);
    public virtual bool TryReadVarIntZigZag<T>(out T result, out ParseStatus status)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        => Defaults.TryReadVarIntZigZag(this, out result, out status);
    public virtual bool ReadBool()
        => Defaults.ReadBool(this);
    public virtual bool TryReadBool(out bool result, out ParseStatus status)
        => Defaults.TryReadBool(this, out result, out status);
    public virtual ReadOnlySpan<byte> ReadRawBlock(int length)
        => Defaults.ReadRawBlock(this, length);
    bool TryReadRawBlock(int length, scoped out ReadOnlySpan<byte> value, out ParseStatus status);
    void SkipRawBlock(long length);
    int ReadRawBlock(scoped Span<byte> buffer, bool readExactly = false);
    public virtual ReadOnlySpan<byte> ReadLengthDelimited()
        => Defaults.ReadLengthDelimited(this);
    bool TryReadLengthDelimited(scoped out ReadOnlySpan<byte> value, out ParseStatus status);
    public virtual void SkipLengthDelimited()
        => Defaults.SkipLengthDelimited(this);
    public virtual byte[] ReadByteArray()
        => Defaults.ReadByteArray(this);
    bool TryReadByteArray(out byte[] value, out ParseStatus status);
    void ReadByteArray(List<byte> destination);
    string ReadString(Encoding? encoding = null);


    public static partial class Defaults
    {
        public static byte ReadByte<TReader>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            return reader.TryReadByte(out byte value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static byte ReadByte<TReader>(TReader reader)
            where TReader : class, IBinaryReader
        {
            return reader.TryReadByte(out byte value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static T ReadFixed32<TReader, T>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : struct
        {
            return reader.TryReadFixed32(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static T ReadFixed32<TReader, T>(TReader reader)
            where TReader : class, IBinaryReader
            where T : struct
        {
            return reader.TryReadFixed32(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static bool TryReadFixed32<TReader, T>(scoped ref TReader reader, out T result, out ParseStatus status)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : struct
        {
            const int bufSize = sizeof(uint);
            int size = Unsafe.SizeOf<T>();
            if (size > bufSize)
                throw new ArgumentException("T cannot fit into protobuf fixed32", nameof(T));
            Span<byte> buffer = stackalloc byte[bufSize];
            int read = reader.ReadRawBlock(buffer, false);
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
        public static bool TryReadFixed32<TReader, T>(TReader reader, out T result, out ParseStatus status)
            where TReader : class, IBinaryReader
            where T : struct
        {
            const int bufSize = sizeof(uint);
            int size = Unsafe.SizeOf<T>();
            if (size > bufSize)
                throw new ArgumentException("T cannot fit into protobuf fixed32", nameof(T));
            Span<byte> buffer = stackalloc byte[bufSize];
            int read = reader.ReadRawBlock(buffer, false);
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
        public static T ReadFixed64<TReader, T>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : struct
        {
            return reader.TryReadFixed64(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static T ReadFixed64<TReader, T>(TReader reader)
            where TReader : class, IBinaryReader
            where T : struct
        {
            return reader.TryReadFixed64(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static bool TryReadFixed64<TReader, T>(scoped ref TReader reader, out T result, out ParseStatus status)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : struct
        {
            const int bufSize = sizeof(ulong);
            int size = Unsafe.SizeOf<T>();
            if (size > bufSize)
                throw new ArgumentException("T cannot fit into protobuf fixed64", nameof(T));
            Span<byte> buffer = stackalloc byte[bufSize];
            int read = reader.ReadRawBlock(buffer, false);
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
        public static bool TryReadFixed64<TReader, T>(TReader reader, out T result, out ParseStatus status)
            where TReader : class, IBinaryReader
            where T : struct
        {
            const int bufSize = sizeof(ulong);
            int size = Unsafe.SizeOf<T>();
            if (size > bufSize)
                throw new ArgumentException("T cannot fit into protobuf fixed64", nameof(T));
            Span<byte> buffer = stackalloc byte[bufSize];
            int read = reader.ReadRawBlock(buffer, false);
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
        public static T ReadVarInt<TReader, T>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : IBinaryInteger<T>
        {
            return reader.TryReadVarInt(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static T ReadVarInt<TReader, T>(TReader reader)
            where TReader : class, IBinaryReader
            where T : IBinaryInteger<T>
        {
            return reader.TryReadVarInt(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static bool TryReadVarInt<TReader, T>(scoped ref TReader reader, out T result, out ParseStatus status)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : IBinaryInteger<T>
        {
            result = T.Zero;
            byte byteReadJustNow;
            const int MaxBytesWithoutOverflow = 9;
            for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
            {
                if (!reader.TryReadByte(out byteReadJustNow, out _))
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
            if (!reader.TryReadByte(out byteReadJustNow, out _))
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
        public static bool TryReadVarInt<TReader, T>(TReader reader, out T result, out ParseStatus status)
            where TReader : class, IBinaryReader
            where T : IBinaryInteger<T>
        {
            result = T.Zero;
            byte byteReadJustNow;
            const int MaxBytesWithoutOverflow = 9;
            for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
            {
                if (!reader.TryReadByte(out byteReadJustNow, out _))
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
            if (!reader.TryReadByte(out byteReadJustNow, out _))
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
        public static T ReadVarIntZigZag<TReader, T>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : IBinaryInteger<T>, ISignedNumber<T>
        {
            return reader.TryReadVarIntZigZag(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static T ReadVarIntZigZag<TReader, T>(TReader reader)
            where TReader : class, IBinaryReader
            where T : IBinaryInteger<T>, ISignedNumber<T>
        {
            return reader.TryReadVarIntZigZag(out T value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static bool TryReadVarIntZigZag<TReader, T>(scoped ref TReader reader, out T result, out ParseStatus status)
            where TReader : struct, IBinaryReader, allows ref struct
            where T : IBinaryInteger<T>, ISignedNumber<T>
        {
            if (reader.TryReadVarInt(out long value, out status))
            {
                result = T.CreateTruncating(ProtobufUtility.DecodeZigZag(value));
                return true;
            }
            result = T.Zero;
            return false;
        }
        public static bool TryReadVarIntZigZag<TReader, T>(TReader reader, out T result, out ParseStatus status)
            where TReader : class, IBinaryReader
            where T : IBinaryInteger<T>, ISignedNumber<T>
        {
            if (reader.TryReadVarInt(out long value, out status))
            {
                result = T.CreateTruncating(ProtobufUtility.DecodeZigZag(value));
                return true;
            }
            result = T.Zero;
            return false;
        }
        public static bool ReadBool<TReader>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            return reader.TryReadBool(out bool value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static bool ReadBool<TReader>(TReader reader)
            where TReader : class, IBinaryReader
        {
            return reader.TryReadBool(out bool value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static bool TryReadBool<TReader>(scoped ref TReader reader, out bool result, out ParseStatus status)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            bool b = reader.TryReadVarInt(out ulong value, out status);
            result = value != 0;
            return b;
        }
        public static bool TryReadBool<TReader>(TReader reader, out bool result, out ParseStatus status)
            where TReader : class, IBinaryReader
        {
            bool b = reader.TryReadVarInt(out ulong value, out status);
            result = value != 0;
            return b;
        }
        public static ReadOnlySpan<byte> ReadRawBlock<TReader>(scoped ref TReader reader, int length)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            return reader.TryReadRawBlock(length, out ReadOnlySpan<byte> value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static ReadOnlySpan<byte> ReadRawBlock<TReader>(TReader reader, int length)
            where TReader : class, IBinaryReader
        {
            return reader.TryReadRawBlock(length, out ReadOnlySpan<byte> value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static ReadOnlySpan<byte> ReadLengthDelimited<TReader>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            return reader.TryReadLengthDelimited(out ReadOnlySpan<byte> value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static ReadOnlySpan<byte> ReadLengthDelimited<TReader>(TReader reader)
            where TReader : class, IBinaryReader
        {
            return reader.TryReadLengthDelimited(out ReadOnlySpan<byte> value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static void SkipLengthDelimited<TReader>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            long length = reader.ReadVarInt<long>();
            if (length < 0)
                throw new InvalidDataException();
            reader.SkipRawBlock(length);
        }
        public static void SkipLengthDelimited<TReader>(TReader reader)
            where TReader : class, IBinaryReader
        {
            long length = reader.ReadVarInt<long>();
            if (length < 0)
                throw new InvalidDataException();
            reader.SkipRawBlock(length);
        }
        public static byte[] ReadByteArray<TReader>(scoped ref TReader reader)
            where TReader : struct, IBinaryReader, allows ref struct
        {
            return reader.TryReadByteArray(out byte[] value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
        public static byte[] ReadByteArray<TReader>(TReader reader)
            where TReader : class, IBinaryReader
        {
            return reader.TryReadByteArray(out byte[] value, out ParseStatus status) ? value : ThrowIfNotSuccess(value, status);
        }
    }
}
public interface IStructBinaryReader<TReader> : IBinaryReader
    where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
{
    public static abstract TReader CreateLengthDelimitedReader(scoped ref TReader parent);
    public static abstract bool TryCreateLengthDelimitedReader(scoped ref TReader parent, [NotNullWhen(true)] out TReader subReader, out ParseStatus status);
}
public interface IClassBinaryReader<TReader> : IBinaryReader
    where TReader : class, IClassBinaryReader<TReader>
{
    public static abstract TReader CreateLengthDelimitedReader(TReader parent);
    public static abstract bool TryCreateLengthDelimitedReader(TReader parent, [NotNullWhen(true)] out TReader? subReader, out ParseStatus status);
}