using static System.Console;
using ArrayPoolz;

using var arr = new BetterArray<int>(3)
{
    [0] = 2,
    [1] = 4,
    [2] = 5
};

arr[2] = 10;

WriteLine("BetterArray: {0}", arr.Length);

using BetterArray<int> arr2 = new[] { 2, 4, 5 };

WriteLine("BetterArray implicit []: {0}", arr2.Length);

IEnumerable<int> arr3 = arr;

WriteLine("BetterArray implicit IEnumerable: {0}", arr3.Count());

using BetterArray<int> arr4 = arr.AsSpan();

WriteLine("BetterArray implicit Span: {0}", arr4.Length);