using DSA;
using NutritionManagement;

public static class SortingAlgorithms
{

    public static void BubbleSort(DynamicArray<Food> arr, string criteria)
    {
        for (int i = 0; i < arr.Count - 1; i++)
            for (int j = 0; j < arr.Count - i - 1; j++)
                if (GetValue(arr.At(j), criteria) > GetValue(arr.At(j + 1), criteria))
                    Swap(arr, j, j + 1);
    }


    public static void MergeSort(DynamicArray<Food> arr, string criteria)
    {
        if (arr.Count <= 1) return;


        int mid = arr.Count / 2;
        DynamicArray<Food> left = new DynamicArray<Food>();
        DynamicArray<Food> right = new DynamicArray<Food>();

        for (int i = 0; i < mid; i++)
            left.AddLast(arr.At(i));
        for (int i = mid; i < arr.Count; i++)
            right.AddLast(arr.At(i));


        MergeSort(left, criteria);
        MergeSort(right, criteria);


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


        while (i < left.Count)
            arr.Set(k++, left.At(i++));


        while (j < right.Count)
            arr.Set(k++, right.At(j++));
    }


    public static void QuickSort(DynamicArray<Food> arr, string criteria)
    {
        QuickSortHelper(arr, 0, arr.Count - 1, criteria);
    }

    private static void QuickSortHelper(DynamicArray<Food> arr, int low, int high, string criteria)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high, criteria);
            QuickSortHelper(arr, low, pi - 1, criteria);
            QuickSortHelper(arr, pi + 1, high, criteria);
        }
    }

    private static int Partition(DynamicArray<Food> arr, int low, int high, string criteria)
    {
        Food pivot = arr.At(high);
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (GetValue(arr.At(j), criteria) <= GetValue(pivot, criteria))
            {
                i++;
                Swap(arr, i, j);
            }
        }

        Swap(arr, i + 1, high);
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