namespace Myitian.LiteProtobuf;

public enum ParseStatus
{
    Success,
    ExactEndOfStream,
    EndOfStream,
    InvalidData,
    NotSupported
}