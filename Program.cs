using DSA;
using NutritionManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using NutritionManagement;

class Program
{
    static User currentUser = new User(); // Track the current user
    static NutritionStore? store; // Store the nutrition data

    static void Main()
    {
        InitializeSystem(); // Load data
        ShowMainMenu(); // Display the main menu
        Meals = new Dictionary<string, MealPlan>();
    }

    public static void AddMeal(string mealType, MealPlan meal)
    {
        Meals[mealType] = meal;
    }

    static void InitializeSystem()
    {
        // Load nutrition data from file
        Loader loader = new Loader("food_data.txt"); // Ensure the file exists
        store = loader.Read();
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Search Food");
            Console.WriteLine("2. Show Top 50 Foods by Nutrient");
            Console.WriteLine("3. Log Food Consumption");
            Console.WriteLine("4. View Food Log");
            Console.WriteLine("5. Sort Foods (Performance Test)");
            Console.WriteLine("6. Create Meal Plan");
            Console.WriteLine("7. Exit");
            Console.Write("Choice: ");

            string choice = Console.ReadLine()!.Trim();
            switch (choice)
            {
                case "1":
                    SearchFood();
                    break;
                case "2":
                    ShowTop50NutrientMenu();
                    break;
                case "3":
                    LogFood();
                    break;
                case "4":
                    ShowFoodLog();
                    break;
                case "5":
                    RunPerformanceAnalysis();
                    break;
                case "6":
                    CreateMealPlan();
                    break;
                case "7":
                    Console.WriteLine("Exiting... Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1-7.");
                    break;
            }
        }
    }

    // New method to show the top 50 nutrient menu
    static void ShowTop50NutrientMenu()
    {
        Console.WriteLine("\nSelect Nutrient:");
        Console.WriteLine("1. Protein");
        Console.WriteLine("2. Calories");
        Console.WriteLine("3. Carbohydrates");
        Console.WriteLine("4. Fat");
        Console.WriteLine("5. Vitamin C");
        Console.Write("Choice: ");

        string choice = Console.ReadLine()!.Trim();
        switch (choice)
        {
            case "1":
                ShowTop50Foods("protein");
                break;
            case "2":
                ShowTop50Foods("calories");
                break;
            case "3":
                ShowTop50Foods("carbohydrates");
                break;
            case "4":
                ShowTop50Foods("fat");
                break;
            case "5":
                ShowTop50Foods("vitamin c");
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    // Method to search for food items
    static void SearchFood()
    {
        Console.Write("Enter food name to search: ");
        string query = Console.ReadLine()!.Trim();

        SearchResults searchResults = new SearchResults();
        for (int i = 0; i < store!.Foods.Count; i++)
        {
            Food food = store.Foods.At(i);
            if (food.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                searchResults.Res.AddLast(food);
            }
        }

        Console.WriteLine($"\nSearch Results for '{query}':");
        searchResults.ShowResults();
        
    }

    // Method to log food consumption
    static void LogFood()
    {
        Console.Write("Enter the name of the food you consumed: ");
        string foodName = Console.ReadLine()!.Trim();

        // Search for the food in the store
        Food? food = null;
        for (int i = 0; i < store!.Foods.Count; i++)
        {
            if (store.Foods.At(i).Name.Equals(foodName, StringComparison.OrdinalIgnoreCase))
            {
                food = store.Foods.At(i);
                break;
            }
        }

        if (food != null)
        {
            currentUser.AddToLog(food); // Add to user's food log
            Console.WriteLine($"{food.Name} logged successfully!");
        }
        else
        {
            Console.WriteLine("Food not found in the database.");
        }
    }

    // Method to show the user's food log
    static void ShowFoodLog()
    {
        Console.WriteLine("\nYour Food Log:");
        if (currentUser.FoodLog.Count == 0)
        {
            Console.WriteLine("No food items logged yet.");
            return;
        }

        for (int i = 0; i < currentUser.FoodLog.Count; i++)
        {
            Food? food = currentUser.FoodLog.ElementAt(i);
            if (food != null)
            {
                Console.WriteLine(food);
            }
        }
    }

    // Method to compare sorting algorithm performance
    static void RunPerformanceAnalysis()
    {
        Console.WriteLine("\nSorting Algorithm Performance:");
        Console.WriteLine("| Algorithm | Criteria       | Time (ms) |");
        Console.WriteLine("|-----------|----------------|-----------|");

        TimeSort("Merge", "protein");
        TimeSort("Bubble", "calories");
        TimeSort("Quick", "carbohydrates");
    }

    // Helper method to time sorting algorithms
    static DynamicArray<Food> TimeSort(string algorithm, string criteria)
    {
        var tempArray = new DynamicArray<Food>();
        for (int i = 0; i < store!.Foods.Count; i++)
            tempArray.AddLast(store.Foods.At(i));

        var watch = System.Diagnostics.Stopwatch.StartNew();
        store.SortFoods(criteria, algorithm);
        watch.Stop();

        Console.WriteLine($"| {algorithm,-9} | {criteria,-14} | {watch.ElapsedMilliseconds,9} |");
        return tempArray;
    }

    static void ShowTop50Foods(string nutrient)
    {
        if (store == null || store.Foods.Count == 0)
        {
            Console.WriteLine("No food items available.");
            return;
        }

        // Sort foods by the selected nutrient
        var sortedFoods = SortByNutrient(nutrient);

        // Display the top 50 foods
        Console.WriteLine($"\nTop 50 Foods by {nutrient}:");
        Console.WriteLine("┌────┬──────────────────────┬────────────┬────────────┬────────────┬────────────┬────────────┐");
        Console.WriteLine("│ ID │ Food Name            │ Calories   │ Protein    │ Carbs      │ Fat        │ Vit C      │");
        Console.WriteLine("├────┼──────────────────────┼────────────┼────────────┼────────────┼────────────┼────────────┤");

        int count = Math.Min(50, sortedFoods.Count); // Show top 50 or all available foods
        for (int i = 0; i < count; i++)
        {
            Food food = sortedFoods.At(i);
            Console.WriteLine(
                $"│ {i + 1,-2} │ {food.Name,-20} │ {food.Calories,10:F2} │ {food.Protein,10:F2} │ " +
                $"{food.Carbohydrates,10:F2} │ {food.Fat,10:F2} │ {food.VitaminC,10:F2} │"
            );
        }

        Console.WriteLine("└────┴──────────────────────┴────────────┴────────────┴────────────┴────────────┘");
    }

    // Method to sort foods by a specific nutrient
    static DynamicArray<Food> SortByNutrient(string nutrient)
    {
        var sortedFoods = new DynamicArray<Food>();
        for (int i = 0; i < store!.Foods.Count; i++)
            sortedFoods.AddLast(store.Foods.At(i));

        switch (nutrient.ToLower())
        {
            case "protein":
                sortedFoods.Sort((a, b) => b.Protein.CompareTo(a.Protein));
                break;
            case "calories":
                sortedFoods.Sort((a, b) => b.Calories.CompareTo(a.Calories));
                break;
            case "carbohydrates":
                sortedFoods.Sort((a, b) => b.Carbohydrates.CompareTo(a.Carbohydrates));
                break;
            case "fat":
                sortedFoods.Sort((a, b) => b.Fat.CompareTo(a.Fat));
                break;
            case "vitamin c":
                sortedFoods.Sort((a, b) => b.VitaminC.CompareTo(a.VitaminC));
                break;
            default:
                throw new ArgumentException("Invalid nutrient specified.");
        }

        return sortedFoods;
    }

    static void CreateMealPlan()
    {
        Console.Write("Enter meal plan name: ");
        string mealPlanName = Console.ReadLine()!.Trim();

        // Create a list to store all meals in the meal plan
        List<MealPlan> meals = new List<MealPlan>();

        string[] mealTypes = { "Breakfast", "Lunch", "Dinner" };

        foreach (var mealType in mealTypes)
        {
            Console.WriteLine($"\nPlanning {mealType}:");
            MealPlan meal = new MealPlan(mealType);

            while (true)
            {
                Console.Write($"Enter food name to add to {mealType} (or 'done' to finish): ");
                string foodName = Console.ReadLine()!.Trim();
                if (foodName.ToLower() == "done")
                    break;

                Food? food = null;
                for (int i = 0; i < store!.Foods.Count; i++)
                {
                    if (store.Foods.At(i).Name.Equals(foodName, StringComparison.OrdinalIgnoreCase))
                    {
                        food = store.Foods.At(i);
                        break;
                    }
                }

                if (food != null)
                {
                    meal.AddFood(food);
                    Console.WriteLine($"{food.Name} added to {mealType}.");
                }
                else
                {
                    Console.WriteLine("Food not found in the database.");
                }
            }

            meals.Add(meal); // Add the meal to the meal plan
        }

        // Calculate totals for the entire meal plan
        double totalCalories = 0;
        double totalProtein = 0;
        double totalCarbohydrates = 0;
        double totalFat = 0;
        double totalVitaminC = 0;

        foreach (var meal in meals)
        {
            totalCalories += meal.TotalCalories();
            totalProtein += meal.TotalProtein();
            totalCarbohydrates += meal.TotalCarbohydrates();
            totalFat += meal.TotalFat();
            totalVitaminC += meal.TotalVitaminC();
        }

        Console.WriteLine($"\nMeal Plan '{mealPlanName}' created successfully!");
        Console.WriteLine($"Total Calories: {totalCalories:F2}");
        Console.WriteLine($"Total Protein: {totalProtein:F2}g");
        Console.WriteLine($"Total Carbohydrates: {totalCarbohydrates:F2}g");
        Console.WriteLine($"Total Fat: {totalFat:F2}g");
        Console.WriteLine($"Total Vitamin C: {totalVitaminC:F2}mg");
    }
    public static Dictionary<string, MealPlan> Meals { get; private set; } = new Dictionary<string, MealPlan>();

    public class MealPlan
    {
        public string Name { get; set; }
        public List<Food> Foods { get; private set; }

        public MealPlan(string name)
        {
            Name = name;
            Foods = new List<Food>();
        }

        public void AddFood(Food food)
        {
            Foods.Add(food);
        }

        public double TotalCalories()
        {
            double total = 0;
            foreach (var food in Foods)
            {
                total += food.Calories;
            }
            return total;
        }

        public double TotalProtein()
        {
            double total = 0;
            foreach (var food in Foods)
            {
                total += food.Protein;
            }
            return total;
        }

        public double TotalCarbohydrates()
        {
            double total = 0;
            foreach (var food in Foods)
            {
                total += food.Carbohydrates;
            }
            return total;
        }

        public double TotalFat()
        {
            double total = 0;
            foreach (var food in Foods)
            {
                total += food.Fat;
            }
            return total;
        }

        public double TotalVitaminC()
        {
            double total = 0;
            foreach (var food in Foods)
            {
                total += food.VitaminC;
            }
            return total;
        }
    }

    class Loader
    {
        public string FilePath { get; set; }
        public DynamicArray<Food> Foods { get; private set; }

        public Loader(string filePath)
        {
            FilePath = filePath;
            Foods = new DynamicArray<Food>();
        }

        public NutritionStore Read()
        {
            Console.WriteLine("Loading Nutrition Data...");

            using StreamReader sr = new(FilePath);
            string? foodName;
            double index, calories, protein, carbohydrates, fat, vitaminC;

            while ((foodName = sr.ReadLine()) != null)
            {
                try
                {
                    index = Convert.ToDouble(sr.ReadLine());
                    calories = Convert.ToDouble(sr.ReadLine());
                    protein = Convert.ToDouble(sr.ReadLine());
                    carbohydrates = Convert.ToDouble(sr.ReadLine());
                    fat = Convert.ToDouble(sr.ReadLine());
                    vitaminC = Convert.ToDouble(sr.ReadLine());

                    Foods.AddLast(new Food(index, foodName, calories, protein, carbohydrates, fat, vitaminC));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing food data: {ex.Message}");
                }
            }

            Console.WriteLine($"{Foods.Count} food items loaded.");
            return new NutritionStore(Foods);
        }
    }
}