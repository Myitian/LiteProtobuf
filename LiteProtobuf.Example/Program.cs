using Myitian.LiteProtobuf.Nodes;
using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;

namespace Myitian.LiteProtobuf.Example;

class Program
{
    static void Main(string[] args)
    {
        string p = Console.ReadLine().AsSpan().Trim().Trim('"').ToString();
        ReadOnlySpan<byte> buffer = File.ReadAllBytes(p);
        using FileStream fs = File.Open("test.protobuf", FileMode.Create, FileAccess.Write, FileShare.Read);
        ProtobufMessage root = new();
        using StreamBinaryWriter writer = new(fs);

        SpanBinaryReader reader = new(buffer);
        // prevent `using` and `ref` limitations. For structs, need to use `try-finally` instead of `using`.
        // Cannot use `using` variable as a ref or out value.
        try
        {
            try
            {
                // use TryCreateFulfilled or ReadProtobuf to read normal ProtobufMessage;
                // use ReadProtobufBody to read a body-only ProtobufMessage.
                root.ReadProtobufBody(ref reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            root = (ProtobufMessage)root.Expand();
            root.WriteProtobufBody(writer, null);

            // If parsed correctly, the input and output should be consistent.

            Console.WriteLine(root.ToFormattedString(null));
        }
        finally
        {
            reader.Dispose(); // Although the SpanBinaryReader doesn't need Dispose actually
        }
    }

    public static int Test(ref readonly SpanBinaryReader reader)
    {
        return reader.ReadFixed32<int>();
    }
}


[DefaultTryCreateInstance]
[DefaultCreateInstance]
[DefaultTryCreateFulfilled]
[DefaultCreateFulfilled]
[GeneratedProtobufTypeSerializer]
partial class Example : IProtobufType<Example>
{
    [ProtobufField(0, Handler = typeof(object))]
    public int TestVal;
    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }
    public bool IsFieldInfoValidForInstance(FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }

    public void ReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options) where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        throw new NotImplementedException();
    }

    public bool TryReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status) where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        throw new NotImplementedException();
    }

    public void WriteProtobuf<TWriter>(scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options) where TWriter : struct, IStructBinaryWriter<TWriter>, allows ref struct
    {
        throw new NotImplementedException();
    }

    void IReadOnlyProtobufType<Example>.ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }

    bool IReadOnlyProtobufType<Example>.TryReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        throw new NotImplementedException();
    }

    void IWriteOnlyProtobufType<Example>.WriteProtobuf<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }
}