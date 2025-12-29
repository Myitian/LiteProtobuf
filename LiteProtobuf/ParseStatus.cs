namespace Myitian.LiteProtobuf;

public enum ParseStatus
{
    /// <summary>
    /// Successfully read content.
    /// </summary>
    Success,
    /// <summary>
    /// The stream ended just as reading began, with no additional content.
    /// </summary>
    ExactEndOfStream,
    /// <summary>
    /// The stream unexpectedly ended during reading.
    /// </summary>
    EndOfStream,
    /// <summary>
    /// The data format is incorrect or the value is out of range.
    /// </summary>
    InvalidData,
    /// <summary>
    /// The data is valid, but the current implementation does not support it.
    /// </summary>
    NotSupported
}