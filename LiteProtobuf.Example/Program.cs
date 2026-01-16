using Myitian.LiteProtobuf.Nodes;
using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.SourceGeneration;

namespace Myitian.LiteProtobuf.Example;

class Program
{
    static void Main()
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
                root.ReadProtobufBody(ref reader, null);
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


// *** The following code is testing serialization prototypes and has no practical function! ***


[GeneratedDefaultImplementation(
    TryCreateInstance = true,
    CreateInstance = true,
    TryCreateFulfilled = true,
    CreateFulfilled = true)]
[GeneratedProtobufTypeSerializer(Read = true, NoSort = false)]
partial class ExampleMessage : IProtobufType<ExampleMessage>
{
    [ProtobufField(1, FieldType.Fixed32)]
    public int TestValField;

    [ProtobufField(2, FieldType.Fixed64)]
    public int TestValProp { get; set; }

    [ProtobufField(4, NoWrite = true)]
    public ProtobufString? TestValPropNW { get; set; }

    [ProtobufField(3, Handler = typeof(Serialization.DefaultHandler.BoolCollectionHandler<List<bool>>))]
    public List<bool>? ListBool { get; set; }

    private readonly ProtobufMessage remaingFields = new();

    public void ReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!IsFieldInfoValidForInstance(fieldInfo, options))
            throw new InvalidDataException();
        TReader subReader = TReader.CreateLengthDelimitedReader(ref reader);
        try
        {
            ParseStatus subStatus;
            while (subReader.TryReadTag(out FieldInfo fi, out subStatus))
            {
                switch (fi.Number)
                {
                    case 1:
                        fi.FieldTypeHint = (FieldType)0;
                        fi.CustomAttribute = 0;
                        break;
                    case 2:
                        fi.FieldTypeHint = (FieldType)0;
                        fi.CustomAttribute = 0;
                        break;
                    case 3:
                        fi.FieldTypeHint = (FieldType)0;
                        fi.CustomAttribute = 0;
                        break;
                    case 4:
                        fi.FieldTypeHint = (FieldType)0;
                        fi.CustomAttribute = 0;
                        break;

                }
            }
            if (subStatus != ParseStatus.ExactEndOfStream)
                throw new InvalidDataException();
        }
        finally
        {
            subReader.Dispose();
        }
        remaingFields.ReadProtobufBody(ref reader, options);
        throw new NotImplementedException();
    }

    public void ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : class, IClassBinaryReader<TReader>
    {
        throw new NotImplementedException();
    }





    public static bool IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }
    public bool IsFieldInfoValidForInstance(FieldInfo fieldInfo, SerializationOptions? options)
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

    bool IReadOnlyProtobufType<ExampleMessage>.TryReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        throw new NotImplementedException();
    }

    void IWriteOnlyProtobufType<ExampleMessage>.WriteProtobuf<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }
}