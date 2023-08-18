using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap <T> where T:IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void AddItem(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);

        return firstItem;
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childLeft = item.HeapIndex * 2 + 1;
            int childRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childLeft < currentItemCount)
            {
                swapIndex = childLeft;
                if (childRight < currentItemCount)
                {
                    if (items[childRight].CompareTo(items[childLeft]) > 0)
                        swapIndex = childRight;
                }
            }
            else
            {
                return;
            }

            if(item.CompareTo(items[swapIndex])<0)
            {
                Swap(item, items[swapIndex]);
            }
            else
            {
                return;
            }
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
        SortDown(item);
    }
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while(true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem)>0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA,T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;

        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

}


public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}