using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Myitian.LiteProtobuf.Serialization;

public class SerializationOptions : IEnumerable<KeyValuePair<string, object?>>
{
    protected Dictionary<string, object?>? _customOptions = null;
    public virtual object? this[string key]
    {
        get => _customOptions?.TryGetValue(key, out object? value) is true ? value : throw new KeyNotFoundException();
        set => (_customOptions ??= []).Add(key, value);
    }
    public virtual ICollection<string> Keys => _customOptions?.Keys ?? (ICollection<string>)[];
    public virtual int Count => _customOptions?.Count ?? 0;
    public virtual void Add(string key, object? value)
    {
        (_customOptions ??= []).Add(key, value);
    }
    public virtual void Clear(bool releaseUnderlyingDictionary = true)
    {
        if (releaseUnderlyingDictionary)
            _customOptions = null;
        else
            _customOptions?.Clear();
    }
    public virtual bool ContainsKey(string key)
    {
        return _customOptions?.ContainsKey(key) ?? false;
    }
    public virtual void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
    {
        (_customOptions as ICollection<KeyValuePair<string, object?>>)?.CopyTo(array, arrayIndex);
    }
    public virtual IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return _customOptions?.GetEnumerator() ?? Enumerable.Empty<KeyValuePair<string, object?>>().GetEnumerator();
    }
    public virtual bool Remove(string key)
    {
        return _customOptions?.Remove(key) ?? false;
    }
    public virtual bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        if (_customOptions is null)
        {
            value = null;
            return false;
        }
        return _customOptions.TryGetValue(key, out value);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}