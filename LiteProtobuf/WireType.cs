namespace Myitian.LiteProtobuf;

public enum WireType
{
    VarInt = 0,
    Fixed64 = 1,
    LengthDelimited = 2,
    // No plan to support legacy value 3 and 4
    Fixed32 = 5
}