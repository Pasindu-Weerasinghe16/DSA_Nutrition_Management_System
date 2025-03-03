using DSA;

namespace NutritionManagement
{
    public class NutritionStore
    {
        public DynamicArray<Food> Foods { get; set; }

        public NutritionStore(DynamicArray<Food> foods) => Foods = foods;

        // Existing ShowFoods() method

        // New method for sorting
        public void SortFoods(string criteria, string algorithm)
        {
            switch (algorithm.ToLower())
            {
                case "bubble":
                    SortingAlgorithms.BubbleSort(Foods, criteria);
                    break;
                case "merge":
                    SortingAlgorithms.MergeSort(Foods, criteria);
                    break;
                case "quick":
                    SortingAlgorithms.QuickSort(Foods, criteria);
                    break;
            }
        }
    }
}