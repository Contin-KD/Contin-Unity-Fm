
using System;
using System.Collections.Generic;

public static class ListExt
{
    /// <summary>
    /// 无GC版本的AddRange
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="collection"></param>
    public static void AddRangeNonAlloc<T>(this IList<T> list, IList<T> collection)
    {
        if (collection == null)
            return;

        for (int i = 0; i < collection.Count; i++)
        {
            list.Add(collection[i]);
        }
    }

    /// <summary>
    /// 有序插入,将一个元素插入到已经排序好的list中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="element"></param>
    public static int OrderedInsert<T>(this IList<T> list, T element) where T : IComparable<T>
    {
        if (list.Count == 0)
        {
            list.Add(element);
            return 0;
        }

        int start = 0;
        int end = list.Count - 1;
        int index = list.Count;
        while (start < end)
        {
            index = (start + end) / 2;
            int curr = list[index].CompareTo(element);
            int next = list[index + 1].CompareTo(element);
            if (curr > 0)
            {
                end = index - 1;
                continue;
            }
            if (next <= 0)
            {
                start = index + 1;
                continue;
            }

            //找到位置了
            list.Insert(index + 1, element);
            return index + 1;
        }

        if (start == end)
        {
            index = list[start].CompareTo(element) <= 0 ? start + 1 : start;
        }

        list.Insert(index, element);
        return index;
    }

    public static T Dequeue<T>(this IList<T> list)
    {
        T element = list[0];
        list.RemoveAt(0);
        return element;
    }
}