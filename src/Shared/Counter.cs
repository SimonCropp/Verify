﻿using System.Collections.Concurrent;
using System.Threading;

abstract class Counter<T>
{
    ConcurrentDictionary<T, int> cache = new ConcurrentDictionary<T, int>();
    int current;

    protected abstract T Convert(int i);

    public T Current => Convert(current);

    public int IntOrNext(T input)
    {
        if (cache.TryGetValue(input, out var cached))
        {
            return cached;
        }

        var increment = Interlocked.Increment(ref current);
        cache[input] = increment;
        return increment;
    }

    public T Next()
    {
        var increment = Interlocked.Increment(ref current);

        var convert = Convert(increment);
        cache[convert] = increment;
        return convert;
    }
}