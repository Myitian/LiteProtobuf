# LiteProtobuf
A Protobuf serializer/deserializer that heavily utilizes source generation.

Currently, only pre-defined `ProtobufNode` types are supported:
- `ProtobufByteArray`
- `ProtobufMessage`
- `ProtobufNumber`
- `ProtobufString`

The support for serializing custom types via source generators is in development.

Support for `.proto` files will not be added in the near future. This feature may be added after the main serialization logic is completed.