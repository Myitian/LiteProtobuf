using Myitian.LiteProtobuf.Nodes;
using Myitian.LiteProtobuf.Serialization;
using Myitian.LiteProtobuf.Serialization.DefaultHandler;
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
                root.ReadProtobufBody(ref reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            root = (ProtobufMessage)root.Expand();
            root.WriteProtobufBody(writer);

            // If parsed correctly, the input and output should be consistent.

            Console.WriteLine(root.ToFormattedString());
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


class MyBoolList : List<bool>;

[GeneratedDefaultImplementation(
    TryCreateInstance = true,
    CreateInstance = true,
    TryCreateFulfilled = true,
    CreateFulfilled = true)]
[GeneratedProtobufTypeSerializer(Read = true, NoSort = false)]
partial class ExampleMessage : IProtobufType<ExampleMessage>
{
    [field: ProtobufField(10, FieldTypeHint.Fixed32)]
    public bool Fx { get; set; } // ???

    [ProtobufField(1, FieldTypeHint.Fixed32)]
    public int TestValField;

    [ProtobufField(2, FieldTypeHint.Fixed64)]
    public int TestValProp { init { } }

    [ProtobufField(4, NoWrite = true)]
    public ProtobufString? TestValPropNW { get; set; }

    [ProtobufField(3, Handler = typeof(BoolCollectionHandler<List<bool>>))]
    public List<bool> ListBool { get; set; }

    [ProtobufField(555)]
    public IDisposable Dsp { get; set; }

    [ProtobufRemainingFields]
    private readonly ProtobufMessage remaingFields = new();

    public void ReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
        where TReader : struct, IStructBinaryReader<TReader>, allows ref struct
    {
        if (!this.IsFieldInfoValidForInstance(fieldInfo, options))
            throw new InvalidDataException();
        TReader subReader = TReader.CreateLengthDelimitedReader(ref reader);
        int field_1_v = default;
        bool field_1_e = false;
        int field_2_v = default;
        bool field_2_e = false;
        MyBoolList? field_3_v = default;
        bool field_3_e = false;
        ProtobufString? field_4_v = default;
        bool field_4_e = false;
        IDisposable? field_555_v = default;
        bool field_555_e = false;
        try
        {
            ParseStatus subStatus;
            while (subReader.TryReadTag(out FieldInfo fi, out subStatus))
            {
                switch (fi.Number)
                {
                    case 1:
                        field_1_v = subReader.ReadVarInt<int>();
                        field_1_e = true;
                        break;
                    case 2:
                        field_2_v = subReader.ReadVarInt<int>();
                        field_2_e = true;
                        break;
                    case 3:
                        fi.FieldTypeHint = (FieldTypeHint)0;
                        fi.CustomAttribute = 0;
                        SerializationHelper.StructBinaryReader<TReader>.Class<List<bool>>.ReadField<BoolCollectionHandler<List<bool>>, BoolCollectionHandler<List<bool>>>(ref subReader, field_3_v, ref field_3_e, fi, options);
                        break;
                    case 4:
                        fi.FieldTypeHint = (FieldTypeHint)0;
                        fi.CustomAttribute = 0;
                        SerializationHelper.StructBinaryReader<TReader>.ClassCreatableReadOnly<ProtobufString>.ReadField(ref subReader, field_4_v, ref field_4_e, fi, options);
                        break;
                    default:
                        fi.FieldTypeHint = (FieldTypeHint)(0);
                        fi.CustomAttribute = 0;
                        remaingFields.AddProtobufField(ref subReader, fi, options);
                        break;
                }
            }
            if (subStatus != ParseStatus.ExactEndOfStream)
            {
                // if (field_1_e)
                //     ProtobufUtility.DisposeHelper(ref field_1_v);
                // if (field_3_e)
                //     ((IDisposable)field_3_v).Dispose();
                throw new InvalidDataException();
            }
            TestValField = field_1_v;
            //TestValProp = field_2_v;
            ListBool = field_3_v!;
            TestValPropNW = field_4_v!;
        }
        catch (Exception ex)
        {
            List<Exception> exceptions = [ex];
            SerializationHelper.Dispose(field_555_v, exceptions);
            if (exceptions.Count < 2)
                throw;
            throw new AggregateException(exceptions);
        }
        finally
        {
            subReader.Dispose();
        }
    }

    public bool IsFieldInfoValidForInstance(FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }

    static bool ICreatableProtobufType<ExampleMessage>.IsFieldInfoValid(FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }

    bool IReadOnlyProtobufType.TryReadProtobuf<TReader>(scoped ref TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        throw new NotImplementedException();
    }

    bool IReadOnlyProtobufType.TryReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options, out ParseStatus status)
    {
        throw new NotImplementedException();
    }

    void IReadOnlyProtobufType.ReadProtobuf<TReader>(TReader reader, FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }

    void IWriteOnlyProtobufType.WriteProtobuf<TWriter>(scoped ref TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }

    void IWriteOnlyProtobufType.WriteProtobuf<TWriter>(TWriter writer, FieldInfo fieldInfo, SerializationOptions? options)
    {
        throw new NotImplementedException();
    }
}