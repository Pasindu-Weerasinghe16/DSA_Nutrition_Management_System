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
            Console.WriteLine("2. Log Food Consumption");
            Console.WriteLine("3. View Food Log");
            Console.WriteLine("4. Sort Foods (Performance Test)");
            Console.WriteLine("5. Exit");
            Console.Write("Choice: ");

            string choice = Console.ReadLine()!.Trim();
            switch (choice)
            {
                case "1":
                    SearchFood();
                    break;
                case "2":
                    LogFood();
                    break;
                case "3":
                    ShowFoodLog();
                    break;
                case "4":
                    RunPerformanceAnalysis();
                    break;
                case "5":
                    Console.WriteLine("Exiting... Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1-5.");
                    break;
            }
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


        var res = TimeSort("Bubble", "calories");
        TimeSort("Merge", "protein");
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