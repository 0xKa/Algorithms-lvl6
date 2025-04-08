using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Algorithms
{
    // Bubble Sort Time Complexity = O(n2) 👎
    public static void BubbleSort(int[] arr)
    {
        int arrLength = arr.Length;

        for (int i = 0; i < arrLength - 1; i++)
            for (int j = 0; j < arrLength - i - 1; j++)
                if (arr[j] > arr[j + 1])
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]); //swap using tuple


    }

    // Selection Sort Time Complexity = O(n2) 👎
    public static void SelectionSort(int[] arr, bool Ascending = true)
    {
        if (Ascending) SelectionSortAsc(arr); else SelectionSortDesc(arr);
    }
    private static void SelectionSortAsc(int[] arr)
    {
        int arrLength = arr.Length;

        // One by one move boundary of unsorted subarray
        for (int i = 0; i < arrLength - 1; i++)
        {
            // Find the minimum element in unsorted array
            int minIndex = i;
            for (int j = i + 1; j < arrLength; j++)
                if (arr[j] < arr[minIndex])
                    minIndex = j;

            (arr[minIndex], arr[i]) = (arr[i], arr[minIndex]); //swap using tuple

        }

    }
    private static void SelectionSortDesc(int[] arr)
    {
        int arrLength = arr.Length;

        for (int i = 0; i < arrLength - 1; i++)
        {
            int maxIndex = i;
            for (int j = i + 1; j < arrLength; j++)
                if (arr[j] > arr[maxIndex])
                    maxIndex = j;

            (arr[maxIndex], arr[i]) = (arr[i], arr[maxIndex]); //swap using tuple

        }
    }

    // Selection Sort Time Complexity = O(n2) 👎 note: it's still better than Bubble and Selection sorts
    public static void InsertionSort(int[] arr)
    {
        int arrLength = arr.Length;


        for (int i = 0; i < arrLength; i++)
        {
            int key = arr[i];
            int j = i - 1;

            // Move elements of arr[0..i-1], that are greater than key, to one position ahead of their current position
            while (j >= 0 && arr[j] >= key)
            {
                arr[j + 1] = arr[j];
                j = j - 1;
            }
            arr[j + 1] = key;
        }

    }

}

internal class Program
{
    public static void PrintArray(int[] arr)
    {
        Console.WriteLine(string.Join(", ", arr));
    }
    static void Main(string[] args)
    {

        int[] numbers = { -29, 66, 32, 46, 64, 10, -99, 21, 10, 12, 80 };
        Console.WriteLine("Array Elements:");
        PrintArray(numbers);
        
        Console.WriteLine("\nBubble Sort:");
        Algorithms.BubbleSort(numbers);
        PrintArray(numbers);


        Console.WriteLine("\nSelection Sort Ascending:");
        Algorithms.SelectionSort(numbers, Ascending: true);
        PrintArray(numbers);


        Console.WriteLine("\nSelection Sort Descending:");
        Algorithms.SelectionSort(numbers, Ascending: false);
        PrintArray(numbers);


        Console.WriteLine("\nInsertion Sort:");
        Algorithms.InsertionSort(numbers);
        PrintArray(numbers);



    }
}