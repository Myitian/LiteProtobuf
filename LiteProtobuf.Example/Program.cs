using Myitian.LiteProtobuf.Nodes;

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
            root.WriteProtobufBody(writer);

            // If parsed correctly, the input and output should be consistent.

            Console.WriteLine(root.ToFormattedString(null));
        }
        finally
        {
            reader.Dispose(); // Although the SpanBinaryReader doesn't need Dispose
        }
    }

    public static int Test(ref readonly SpanBinaryReader reader)
    {
        return reader.ReadFixed32<int>();
    }
}