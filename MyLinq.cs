namespace MyLinq;

public static class MyLinq
{
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }

    public static IEnumerable<TResult> MySelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        foreach (var item in source)
        {
            yield return selector(item);
        }
    }

    public static IEnumerable<TSource> MyOrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var comparer = Comparer<TKey>.Default;
        var list = source.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count - 1 - i; j++)
            {
                if (comparer.Compare(keySelector(list[j]), keySelector(list[j + 1])) > 0)
                {
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
                }
            }
        }
        return list;
    }

    public static bool MyAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item)) return true;
        }
        return false;
    }

    public static bool MyAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (!predicate(item)) return false;
        }
        return true;
    }

    public static IEnumerable<TSource> MyDistinct<TSource>(this IEnumerable<TSource> source)
    {
        var seen = new HashSet<TSource>();
        foreach (var item in source)
        {
            if (seen.Add(item))
            {
                yield return item;
            }
        }
    }

    public static int MyCount<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source is ICollection<TSource> collection)
        {
            return collection.Count;
        }
        var count = 0;
        foreach (var item in source)
        {
            count++;
        }
        return count;
    }

    public static IEnumerable<TSource> MyUnion<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (first == null)  throw new ArgumentNullException(nameof(first));
        if (second == null)  throw new ArgumentNullException(nameof(second));

        var seen = new HashSet<TSource>();

        foreach(var item in first)
        {
            if (seen.Add(item))
            {
                yield return item;
            }
        }

        foreach (var item in second)
        {
            if (seen.Add(item))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<TSource> MyConcat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        foreach(var item in first)
        {
            yield return item;
        }

        foreach(var item in second)
        {
            yield return item;
        }
    }

    public static IEnumerable<TSource> MyExcept<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        var seen = new HashSet<TSource>(second);
        foreach(var item in first)
        {
            if (seen.Add(item))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<TSource> MyIntersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        var secondset = new HashSet<TSource>(second);
        var result = new HashSet<TSource>();
        foreach(var item in first)
        {
            if (secondset.Contains(item)  && result.Add(item))
            {
                yield return item;
            }
        }
    }
}