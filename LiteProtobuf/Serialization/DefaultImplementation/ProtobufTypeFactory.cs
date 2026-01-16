using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization.DefaultImplementation;

public static class ProtobufTypeFactory
{
    public static bool TryCreateInstance<T, THandler>(
        FieldInfo fieldInfo,
        SerializationOptions? options,
        [NotNullWhen(true)] out T? value)
        where T : new(), allows ref struct
        where THandler : IProtobufTypeFactory<T>, allows ref struct
    {
        if (!THandler.IsFieldInfoValid(fieldInfo, options))
        {
            value = default;
            return false;
        }
        value = new();
        return true;
    }
    public static T CreateInstance<T, THandler>(
        FieldInfo fieldInfo,
        SerializationOptions? options)
        where T : new(), allows ref struct
        where THandler : IProtobufTypeFactory<T>, allows ref struct
    {
        if (!THandler.IsFieldInfoValid(fieldInfo, options))
            throw new InvalidDataException($"Invalid data: {fieldInfo}");
        return new();
    }
}