using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NSudoku.Util
{
    public static class CollectionExtensions
    {
        public static IEnumerable<IList<T>> Combinations<T>(this IEnumerable<T> source, int size)
        {
            var items = source.ToList();
            if (size > items.Count) {
                yield break;
            }

            int[] result = new int[size];
            Stack<int> stack = new Stack<int>(size);
            stack.Push(0);
            while (stack.Count > 0) {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < size) {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != size) continue;
                    yield return result.Select(ix => items[ix]).ToList();
                    break;
                }
            }
        }
    }
}
