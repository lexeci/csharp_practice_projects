using System.Diagnostics;

public class Program
{

    static int BinarySearch(int[] sortedArray, int target)
    {
        int left = 0, right = sortedArray.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (sortedArray[mid] == target) return mid;
            if (sortedArray[mid] < target) left = mid + 1;
            else right = mid - 1;
        }
        return -1; // Target not found
    }

    static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivot = Partition(arr, low, high);
            QuickSort(arr, low, pivot - 1);
            QuickSort(arr, pivot + 1, high);
        }
    }
    static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }

    static void BubbleSort(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                }
            }
        }
    }

    static void MergeSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int mid = (left + right) / 2;
            MergeSort(arr, left, mid);
            MergeSort(arr, mid + 1, right);
            Merge(arr, left, mid, right);
        }
    }
    static void Merge(int[] arr, int left, int mid, int right)
    {
        int[] leftArr = arr[left..(mid + 1)];
        int[] rightArr = arr[(mid + 1)..(right + 1)];
        int i = 0, j = 0, k = left;
        while (i < leftArr.Length && j < rightArr.Length)
        {
            if (leftArr[i] <= rightArr[j])
                arr[k++] = leftArr[i++];
            else
                arr[k++] = rightArr[j++];
        }
        while (i < leftArr.Length) arr[k++] = leftArr[i++];
        while (j < rightArr.Length) arr[k++] = rightArr[j++];
    }
    static void Main()
    {
        Random rand = new Random();
        int[] numbers = Enumerable.Range(1, 50000).OrderBy(x => rand.Next()).ToArray();

        // Console.WriteLine("Original Array:");
        // Console.WriteLine(string.Join(", ", numbers));
        Stopwatch stopwatch = Stopwatch.StartNew();
        int[] quicksortArray = (int[])numbers.Clone();
        QuickSort(quicksortArray, 0, quicksortArray.Length - 1);
        stopwatch.Stop();
        Console.WriteLine("Quicksort Time: " + stopwatch.ElapsedMilliseconds + " ms");
        stopwatch.Restart();
        int[] mergeSortArray = (int[])numbers.Clone();
        MergeSort(mergeSortArray, 0, mergeSortArray.Length - 1);
        stopwatch.Stop();
        Console.WriteLine("Merge Sort Time: " + stopwatch.ElapsedMilliseconds + " ms");
        stopwatch.Restart();
        int[] bubbleSortArray = (int[])numbers.Clone();
        BubbleSort(bubbleSortArray);
        stopwatch.Stop();
        Console.WriteLine("Bubble Sort Time: " + stopwatch.ElapsedMilliseconds + " ms");


        int[] userIds = { 101, 203, 304, 405, 506, 607, 708, 809, 910 };
        int target = 506;
        int index = BinarySearch(userIds, target);
        if (index != -1)
            Console.WriteLine($"User ID {target} found at index {index}.");
        else
            Console.WriteLine($"User ID {target} not found.");
    }
}