using DSA;
using NutritionManagement;

class Program
{
    static User currentUser = new User(); // Track the current user
    static NutritionStore? store; // Store the nutrition data

    static void Main()
    {
        InitializeSystem(); // Load data
        ShowMainMenu(); // Display the main menu
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

            Console.WriteLine("6. Exit");
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


        // var res = TimeSort("Merge", "protein");
        // var tst = new SearchResults();
        //tst.Res = res;
        TimeSort("Merge", "protein");
        TimeSort("Bubble", "Calories");
        TimeSort("Quick", "carbohydrates");
        //tst.ShowResults();

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

        Console.WriteLine("└────┴──────────────────────┴────────────┴────────────┴────────────┴────────────┴────────────┘");
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