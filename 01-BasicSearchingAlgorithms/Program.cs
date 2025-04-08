using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
    // Linear Search Time Complexity = O(n) 
    public static int LinearSearch<T>(T[] arr, T x)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(arr[i], x)) //to compare T types instead of ==
                return i; //return index
        }
        return -1;
    }

    // Linear Search Time Complexity = O(log n) 
    public static int BinarySearch(int[] arr, int x)
    {
        int start = 0, end = arr.Length - 1;

        while (start <= end)
        {
            int middle = start + (end - start) / 2;
            
            if (arr[middle] == x)
                return middle;

            if (arr[middle] < x)
                start = middle + 1;
            else
                end = middle - 1;

        }

        return -1;
    }

    static void Main(string[] args)
    {
        /// Linear Search ///
        string[] names = { "Tariq", "Ahmed", "Salim", "Hilal", "Mustafa", "Reda", "Marwan" };

        Console.WriteLine("Array Elements:");
        Console.WriteLine(string.Join(", ", names));

        Console.WriteLine("\nSearching for 'Reda' index...");
        Console.WriteLine("Result: Index " + LinearSearch(names, "Reda"));

        Console.WriteLine("\nSearching for 'Malik' index...");
        Console.WriteLine("Result: Index " + LinearSearch(names, "Malik"));

        /// Binary Search ///
        int[] numbers = { 100, 12, 43, 44, 15, -38, 22, 99, 3201, 10, -5, 55, -32, 0, 420 };
        Array.Sort(numbers); //array Must be sorted for binary search

        Console.WriteLine("Array Elements:");
        Console.WriteLine(string.Join(", ", numbers));

        Console.WriteLine("\nSearching for '99' index...");
        Console.WriteLine("Result: Index " + BinarySearch(numbers, 99));

        Console.WriteLine("\nSearching for '-99' index...");
        Console.WriteLine("Result: Index " + BinarySearch(numbers, -99));


    }
}

/* Linear Search 
The simplest search algorithm; a basic demonstration of searching.
    Linear Search, also known as Sequential Search, is one of the simplest searching algorithms used to find a particular element's position in a list. It works by sequentially checking each element of the list until the desired element is found or the list ends.

Introduction to Linear Search
    Linear Search is a straightforward algorithm that checks every element in a list or array until it finds the target value. If the element is found, the algorithm returns its index. If the list does not contain the element, it indicates that as well. This method does not require the list to be sorted and is very intuitive. However, its simplicity comes at the cost of efficiency, especially with large datasets, as it has a time complexity of O(n)
    O(n), where n is the number of elements in the list.

Conclusion
    Linear Search is a simple and intuitive searching algorithm perfect for small datasets or situations where simplicity is more valued than efficiency. Its implementation in C# showcases basic programming concepts such as loops and conditional statements. Despite its inefficiency for large datasets, understanding Linear Search is crucial for beginners as it lays the foundation for learning more complex searching algorithms.
 */

/* Binary Search
 A fast algorithm for finding a target value within a sorted array.

Binary Search in C# involves explaining the algorithm's concept, providing a detailed program example, and discussing the step-by-step process of how binary search efficiently locates an element within a sorted array. Binary Search is a powerful algorithm that reduces the search space by half at each step, making it significantly faster than linear search for large datasets.

Introduction to Binary Search
    Binary Search is a divide-and-conquer search algorithm that finds the position of a target value within a sorted array. Binary Search compares the target value to the middle element of the array; if they are not equal, the half in which the target cannot lie is eliminated, and the search continues on the remaining half until the target is found or the search space is empty. This method requires the array to be sorted beforehand and has a time complexity of O(log⁡ n)

    O (log n), where n is the number of elements in the array.

Conclusion
    Binary Search is an efficient algorithm for finding an element in a sorted array. Its O(log⁡ n)

    O(log n) time complexity makes it significantly faster than linear search O(n) for large datasets. The key to binary search is its method of eliminating half of the remaining elements from consideration at each step, which is a classic example of the divide-and-conquer strategy. Understanding binary search is crucial for grasping more complex algorithms and concepts in computer science.
 */

