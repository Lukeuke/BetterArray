// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Global

using System.Buffers;
using System.Collections;

namespace ArrayPoolz;

public sealed class BetterArray<T> : IEnumerable<T>, IDisposable
{
    private readonly ArrayPool<T> _pool;
    private readonly T[] _array;
    private readonly int _length;
    private readonly bool _clearOnDispose;

    public int Length => _length;

    private BetterArray(bool clearOnDispose)
    {
        _clearOnDispose = clearOnDispose;
    }

    public BetterArray(int size, bool clearOnDispose = false) : this(clearOnDispose)
    {
        _pool = ArrayPool<T>.Shared;
        _array = _pool.Rent(size);
        _length = size;
    }

    public BetterArray(T[] source, bool clearOnDispose = false) : this(clearOnDispose)
    {
        _pool = ArrayPool<T>.Shared;
        _array = _pool.Rent(source.Length);
        _length = source.Length;
        Array.Copy(source, _array, _length);
    }

    public BetterArray(Span<T> source, bool clearOnDispose = false) : this(clearOnDispose)
    {
        _pool = ArrayPool<T>.Shared;
        _array = _pool.Rent(source.Length);
        _length = source.Length;
        source.CopyTo(_array.AsSpan(0, _length));
    }

    public T this[int index]
    {
        get => _array[index];
        set => _array[index] = value;
    }

    public static implicit operator BetterArray<T>(T[] source) => new(source);
    public static implicit operator BetterArray<T>(Span<T> source) => new(source);

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Length; i++)
        {
            yield return _array[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Span<T> AsSpan() => _array.AsSpan(0, _length);
    public ReadOnlySpan<T> AsReadOnlySpan() => _array.AsSpan(0, _length);
    public Span<T> Slice(int start, int length) => AsSpan().Slice(start, length);

    public T[] ToArray()
    {
        var result = new T[_length];
        Array.Copy(_array, result, _length);
        return result;
    }

    public void Dispose()
    {
        _pool.Return(_array, _clearOnDispose);
        // Console.WriteLine("Disposed");
    }
}