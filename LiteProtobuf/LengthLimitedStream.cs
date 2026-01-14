namespace Myitian.LiteProtobuf;

public sealed class LengthLimitedStream(Stream stream, ulong length, bool leaveOpen = true) : Stream
{
    private ulong _remaining = length;
    private readonly bool _leaveOpen = leaveOpen;
    public Stream BaseStream { get; } = stream;
    public override bool CanRead => BaseStream.CanRead;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotSupportedException();
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Close()
    {
        if (!_leaveOpen)
            BaseStream.Close();
    }
    public override void Flush()
    {
        BaseStream.Flush();
    }
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return BaseStream.FlushAsync(cancellationToken);
    }
    public override int Read(byte[] buffer, int offset, int count)
    {
        ValidateBufferArguments(buffer, offset, count);
        ulong c = Math.Min((ulong)count, _remaining);
        int result = BaseStream.Read(buffer, offset, (int)c);
        _remaining -= (uint)result;
        return result;
    }
    public override int Read(Span<byte> buffer)
    {
        ulong c = Math.Min((ulong)buffer.Length, _remaining);
        if (c == 0)
            return 0;
        int result = BaseStream.Read(buffer[..(int)c]);
        _remaining = Math.Min(_remaining - (uint)result, 0);
        return result;
    }
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<int>(cancellationToken);
        try
        {
            ValidateBufferArguments(buffer, offset, count);
            ulong c = Math.Min((ulong)count, _remaining);
            if (c == 0)
                return Task.FromResult(0);
            return BaseStream.ReadAsync(buffer, offset, (int)c, cancellationToken).ContinueWith(task =>
            {
                int result = task.Result;
                _remaining = Math.Min(_remaining - (uint)result, 0);
                return result;
            });
        }
        catch (Exception ex)
        {
            return Task.FromException<int>(ex);
        }
    }
    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ulong c = Math.Min((ulong)buffer.Length, _remaining);
        if (c == 0)
            return 0;
        int result = await BaseStream.ReadAsync(buffer[..(int)c], cancellationToken);
        _remaining = Math.Min(_remaining - (uint)result, 0);
        return result;
    }
    public override int ReadByte()
    {
        if (_remaining > 0)
        {
            int result = BaseStream.ReadByte();
            _remaining--;
            return result;
        }
        return -1;
    }
    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }
    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}