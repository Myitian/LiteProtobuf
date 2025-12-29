using System.Numerics;
using System.Text;

namespace Myitian.LiteProtobuf;

public interface IBinaryWriter : IDisposable
{
    void WriteByte(byte value);
    void WriteFixed32<T>(T value) where T : struct;
    void WriteFixed64<T>(T value) where T : struct;
    void WriteVarInt<T>(T value) where T : IBinaryInteger<T>;
    void WriteVarIntZigZag<T>(T value) where T : IBinaryInteger<T>, ISignedNumber<T>;
    void WriteBool(bool value);
    void WriteRawBlock(ReadOnlySpan<byte> value);
    void WriteLengthDelimited(ReadOnlySpan<byte> value);
    void WriteString(ReadOnlySpan<char> value, Encoding? encoding = null);
}
public interface IStructBinaryWriter<TWriter> : IBinaryWriter where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
{
    public static abstract TWriter CreateLengthDelimitedWriter(ref TWriter parent);
}
public interface IClassBinaryWriter<TWriter> : IBinaryWriter where TWriter : class, IClassBinaryWriter<TWriter>
{
    public static abstract TWriter CreateLengthDelimitedWriter(TWriter parent);
}