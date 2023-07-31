using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Util;

public static class CollectionExtensions
{
    public static IEnumerable<IList<T>> Combinations<T>(this IEnumerable<T> source, int size)
    {
        bool Increment(int[] indexes, int max, int ix)
        {
            if (indexes[ix] < max) {
                indexes[ix]++;
                return true;
            }
            else if (ix > 0) {
                if (Increment(indexes, max - 1, ix - 1))
                {
                    indexes[ix] = indexes[ix - 1] + 1;
                    return true;
                }
            }

            return false;
        }

        if (size <= 0) {
            yield break;
        }

        var items = source.ToList();
        if (size > items.Count) {
            yield break;
        }

        var indexes = new int[size];
        for (var ix = 0; ix < size; ix++) {
            indexes[ix] = ix;
        }

        do {
            yield return indexes.Select(ix => items[ix]).ToList();
        } while (Increment(indexes, items.Count - 1, size - 1));
    }
}
