using DSA;
using NutritionManagement;

public static class SortingAlgorithms
{
    // Member 1: Bubble Sort (Calories)
    public static void BubbleSort(DynamicArray<Food> arr, string criteria)
    {
        for (int i = 0; i < arr.Count - 1; i++)
            for (int j = 0; j < arr.Count - i - 1; j++)
                if (GetValue(arr.At(j), criteria) > GetValue(arr.At(j + 1), criteria))
                    Swap(arr, j, j + 1);
    }

    // Member 2: Merge Sort (Protein)
    public static void MergeSort(DynamicArray<Food> arr, string criteria)
    {
        if (arr.Count <= 1) return;

        // Split the array into two halves
        int mid = arr.Count / 2;
        DynamicArray<Food> left = new DynamicArray<Food>();
        DynamicArray<Food> right = new DynamicArray<Food>();

        for (int i = 0; i < mid; i++)
            left.AddLast(arr.At(i));
        for (int i = mid; i < arr.Count; i++)
            right.AddLast(arr.At(i));

        // Recursively sort both halves
        MergeSort(left, criteria);
        MergeSort(right, criteria);

        // Merge the sorted halves
        Merge(arr, left, right, criteria);
    }

    private static void Merge(DynamicArray<Food> arr, DynamicArray<Food> left, DynamicArray<Food> right, string criteria)
    {
        int i = 0, j = 0, k = 0;

        while (i < left.Count && j < right.Count)
        {
            if (GetValue(left.At(i), criteria) <= GetValue(right.At(j), criteria))
                arr.Set(k++, left.At(i++));
            else
                arr.Set(k++, right.At(j++));
        }

        // Copy remaining elements from left (if any)
        while (i < left.Count)
            arr.Set(k++, left.At(i++));

        // Copy remaining elements from right (if any)
        while (j < right.Count)
            arr.Set(k++, right.At(j++));
    }

    // Member 3: Quick Sort (Carbohydrates)
    public static void QuickSort(DynamicArray<Food> arr, string criteria)
    {
        QuickSortHelper(arr, 0, arr.Count - 1, criteria);
    }

    private static void QuickSortHelper(DynamicArray<Food> arr, int low, int high, string criteria)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high, criteria); // Partition index
            QuickSortHelper(arr, low, pi - 1, criteria); // Sort left subarray
            QuickSortHelper(arr, pi + 1, high, criteria); // Sort right subarray
        }
    }

    private static int Partition(DynamicArray<Food> arr, int low, int high, string criteria)
    {
        Food pivot = arr.At(high); // Pivot element
        int i = low - 1; // Index of smaller element

        for (int j = low; j < high; j++)
        {
            if (GetValue(arr.At(j), criteria) <= GetValue(pivot, criteria))
            {
                i++;
                Swap(arr, i, j); // Swap elements
            }
        }

        Swap(arr, i + 1, high); // Swap pivot to its correct position
        return i + 1;
    }

    private static double GetValue(Food food, string criteria) =>
        criteria.ToLower() switch
        {
            "calories" => food.Calories,
            "protein" => food.Protein,
            "carbohydrates" => food.Carbohydrates,
            _ => 0
        };

    private static void Swap(DynamicArray<Food> arr, int i, int j)
    {
        Food temp = arr.At(i);
        arr.Set(i, arr.At(j));
        arr.Set(j, temp);
    }


}