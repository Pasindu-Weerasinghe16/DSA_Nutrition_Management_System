using DSA;
using System;
using System.Collections.Generic;
using System.IO;
using NutritionManagement;
using System.Drawing;

class Program
{
    static User currentUser = new User();
    static NutritionStore? store;

    static void Main()
    {
        InitializeSystem();
        ShowMainMenu();
        Meals = new Dictionary<string, MealPlan>();
    }

    public static void AddMeal(string mealType, MealPlan meal)
    {
        Meals[mealType] = meal;
    }

    static void InitializeSystem()
    {

        Loader loader = new Loader("food_data.txt");
        store = loader.Read();
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine(" \n");
            Console.WriteLine("========================================================");
            Console.WriteLine("                   Nutrition Management                 ");
            Console.WriteLine("========================================================");
            Console.WriteLine("      1.  Search Food");
            Console.WriteLine("      2.  Show Top 100 Foods by Nutrient");
            Console.WriteLine("      3.  My Food List");
            Console.WriteLine("      4.  View MY Food List");
            Console.WriteLine("      5.  Genarate Balance Meal Plan");
            Console.WriteLine("        (Please add at least 5 food items to My Food List)");
            Console.WriteLine("      6.  Show Daily Nutrient Needs");
            Console.WriteLine("      7.  Nutrition Calculator");
            Console.WriteLine("      8.  Sort Foods (Performance Test)");

            Console.WriteLine("      9. Exit");
            Console.WriteLine("=======================================================");
            Console.WriteLine(" \n");

            Console.Write("Enter your Choice:  ");


            string choice = Console.ReadLine()!.Trim();
            switch (choice)
            {
                case "1":
                    SearchFood();
                    break;
                case "2":
                    ShowTop100NutrientMenu();
                    break;
                case "3":
                    LogFood();
                    break;
                case "4":
                    ShowFoodLog();
                    break;
                case "5":
                    GenerateBalancedMealPlan();
                    break;
                case "6":
                    ShowDailyNutrientNeeds();
                    break;
                case "7":
                    CreateMealPlan();
                    break;
                case "8":
                    RunPerformanceAnalysis();
                    break;
                case "9":
                    Console.WriteLine("Exiting... Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1-11.");
                    break;
            }

            static void ShowDailyNutrientNeeds()
            {

                double dailyCalories = 2000;
                double dailyProtein = 50;
                double dailyCarbohydrates = 300;
                double dailyFat = 70;
                double dailyVitaminC = 90;

                Console.WriteLine("================================================================");
                Console.WriteLine("                   DAILY NUTRIENT NEEDS");
                Console.WriteLine("================================================================");
                Console.WriteLine("┌──────────────────────────────┬────────────────┐");
                Console.WriteLine("│ Nutrient                     │ Daily Need     │");
                Console.WriteLine("├──────────────────────────────┼────────────────┤");
                Console.WriteLine($"│ {"Calories ",-24} │{dailyCalories.ToString().PadLeft(12)} kcal│");
                Console.WriteLine($"│ {"Protein",-28} │ {dailyProtein.ToString().PadLeft(14)}g│");
                Console.WriteLine($"│ {"Carbohydrates",-28} │ {dailyCarbohydrates.ToString().PadLeft(14)}g│");
                Console.WriteLine($"│ {"Fat",-28} │ {dailyFat.ToString().PadLeft(14)}g│");
                Console.WriteLine($"│ {"Vitamin C",-28} │ {dailyVitaminC.ToString().PadLeft(14)}mg│");
                Console.WriteLine("└──────────────────────────────┴────────────────┘");
                Console.WriteLine("================================================================");
            }
        }
    }


    static void ShowTop100NutrientMenu()
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
                ShowTop100Foods("protein");
                break;
            case "2":
                ShowTop100Foods("calories");
                break;
            case "3":
                ShowTop100Foods("carbohydrates");
                break;
            case "4":
                ShowTop100Foods("fat");
                break;
            case "5":
                ShowTop100Foods("vitamin c");
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }


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


    static void LogFood()
    {
        Console.Write("Enter the name of the food you consumed: ");
        string foodName = Console.ReadLine()!.Trim();


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
            currentUser.AddToLog(food);

            Console.WriteLine("\n");

            Console.WriteLine("==============================================");
            Console.WriteLine("       FOOD LOGGED SUCCESSFULLY!              ");
            Console.WriteLine("==============================================");



            Console.WriteLine($"{"You've logged:",-20} {food.Name}");
            Console.WriteLine($"{"Calories:",-20} {food.Calories,10:F2} kcal");
            Console.WriteLine($"{"Protein:",-20} {food.Protein,10:F2} g");
            Console.WriteLine($"{"Carbs:",-20} {food.Carbohydrates,10:F2} g");
            Console.WriteLine($"{"Fat:",-20} {food.Fat,10:F2} g");
            Console.WriteLine($"{"Vitamin C:",-20} {food.VitaminC,10:F2} mg");


            Console.WriteLine("==============================================");
            Console.WriteLine("                                              ");
            Console.WriteLine("==============================================");
            Console.ResetColor();
            Console.WriteLine("\n");

        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("==============================================");
            Console.WriteLine("              FOOD NOT FOUND!                 ");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("The food you entered is not in the database. ");
            Console.WriteLine("Please check the spelling or add it manually.");
            Console.ResetColor();

            Console.WriteLine("==============================================");
            Console.WriteLine("                 Try again!                  ");
            Console.WriteLine("==============================================");
            Console.WriteLine("\n");

        }
    }


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

    static void RunPerformanceAnalysis()
    {
        Console.WriteLine("\n");

        Console.WriteLine("\nSorting Algorithm Performance:");
        Console.WriteLine("| Algorithm | Criteria       | Time (ms) |");
        Console.WriteLine("|-----------|----------------|-----------|");

        TimeSort("Merge", "protein");
        TimeSort("Bubble", "calories");
        TimeSort("Quick", "carbohydrates");
        Console.WriteLine("\n");

    }


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

    static void ShowTop100Foods(string nutrient)
    {
        if (store == null || store.Foods.Count == 0)
        {
            Console.WriteLine("No food items available.");
            return;
        }


        var sortedFoods = SortByNutrient(nutrient);



        Console.WriteLine("\n");

        Console.WriteLine($"\nTop 100 Foods by {nutrient}:");
        Console.WriteLine("┌──────────┬──────────────────────────────────┬──────────────────┬──────────────────┬──────────────────┬──────────────────┬──────────────────┐");
        Console.WriteLine("│ ID       │ Food Name                        │ Calories         │ Protein          │ Carbs            │ Fat              │ Vit C            │");
        Console.WriteLine("├──────────┼──────────────────────────────────┼──────────────────┼──────────────────┼──────────────────┼──────────────────┼──────────────────┤");

        int count = Math.Min(100, sortedFoods.Count);
        for (int i = 0; i < count; i++)
        {
            Food food = sortedFoods.At(i);
            Console.WriteLine(
                $"│ {i + 1,-8} │ {food.Name,-32} │ {food.Calories,16:F2} │ {food.Protein,16:F2} │ " +
                $"{food.Carbohydrates,16:F2} │ {food.Fat,16:F2} │ {food.VitaminC,16:F2} │"
            );
        }

        Console.WriteLine("└──────────┴──────────────────────────────────┴──────────────────┴──────────────────┴──────────────────┴──────────────────┴──────────────────┘");

    }

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

        Console.WriteLine("\n");
        Console.Write("Enter meal plan name: ");
        string mealPlanName = Console.ReadLine()!.Trim();


        List<MealPlan> meals = new List<MealPlan>();

        string[] mealTypes = { "Breakfast", "Lunch", "Dinner" };

        foreach (var mealType in mealTypes)
        {
            Console.WriteLine($"\nPlanning {mealType}:");
            MealPlan meal = new MealPlan(mealType);

            while (true)

            {
                Console.WriteLine("\n");

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

            meals.Add(meal);
        }


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
        Console.WriteLine("\n");

        Console.WriteLine($"\nMeal Plan '{mealPlanName}' created successfully!");
        Console.WriteLine("===================================");
        Console.WriteLine("| Nutrient          | Total       |");
        Console.WriteLine("===================================");
        Console.WriteLine($"| Total Calories    | {totalCalories,10:F2} |");
        Console.WriteLine($"| Total Protein     | {totalProtein,10:F2}g |");
        Console.WriteLine($"| Total Carbohydrates | {totalCarbohydrates,10:F2}g |");
        Console.WriteLine($"| Total Fat         | {totalFat,10:F2}g |");
        Console.WriteLine($"| Total Vitamin C   | {totalVitaminC,10:F2}mg |");
        Console.WriteLine("===================================");
        Console.WriteLine("\n");


    }
    public static Dictionary<string, MealPlan> Meals { get; private set; } = new Dictionary<string, MealPlan>();


    static void AddMealToMealPlan()
    {
        Console.Write("Enter meal plan name: ");
        string mealPlanName = Console.ReadLine()!.Trim();

        if (!Meals.ContainsKey(mealPlanName))
        {
            Console.WriteLine($"Meal plan '{mealPlanName}' does not exist.");
            return;
        }

        MealPlan mealPlan = Meals[mealPlanName];

        Console.Write("Enter meal type (e.g., Breakfast, Lunch, Dinner): ");
        string mealType = Console.ReadLine()!.Trim();

        if (!mealPlan.Meals.ContainsKey(mealType))
        {
            mealPlan.Meals[mealType] = new List<Food>();
        }

        while (true)
        {
            Console.WriteLine("\n");

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
                mealPlan.Meals[mealType].Add(food);
                Console.WriteLine($"{food.Name} added to {mealType}.");
            }
            else
            {
                Console.WriteLine("\n");

                Console.WriteLine("Food not found in the database.");
            }
        }
    }



    public class MealPlan
    {
        public string Name { get; set; }
        public List<Food> Foods { get; private set; }

        public Dictionary<string, List<Food>> Meals { get; private set; }

        public MealPlan(string name)
        {
            Name = name;
            Foods = new List<Food>();
            Meals = new Dictionary<string, List<Food>>();
        }

        public void AddFood(Food food)
        {
            Foods.Add(food);
        }

        public void RemoveMeal(string mealType)
        {
            if (Meals.ContainsKey(mealType))
            {
                Meals.Remove(mealType);
            }
            else
            {
                Console.WriteLine("\n");

                throw new ArgumentException($"Meal type '{mealType}' does not exist.");
            }
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


    static void DeleteMealPlan()

    {
        Console.WriteLine("\n");

        Console.Write("Enter meal plan name: ");
        string mealPlanName = Console.ReadLine()!.Trim();

        if (!Meals.ContainsKey(mealPlanName))
        {
            Console.WriteLine($"Meal plan '{mealPlanName}' does not exist.");
            return;
        }

        MealPlan mealPlan = Meals[mealPlanName];

        Console.Write("Enter meal type to delete (e.g., Breakfast, Lunch, Dinner): ");
        string mealType = Console.ReadLine()!.Trim();

        if (mealPlan.Meals.ContainsKey(mealType))
        {
            mealPlan.RemoveMeal(mealType);
            Console.WriteLine($"Meal '{mealType}' removed from meal plan '{mealPlanName}'.");
        }
        else
        {
            Console.WriteLine($"Meal '{mealType}' does not exist in meal plan '{mealPlanName}'.");
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
            Console.WriteLine("\n");
            Console.WriteLine("\n");


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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Foods.Count} food items loaded.");
            Console.ResetColor();
            return new NutritionStore(Foods);

        }
    }
    public class NewFood
    {
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
        public double VitaminC { get; set; }

        public NewFood(string name, double calories, double protein, double carbohydrates, double fat, double vitaminC)
        {
            Name = name;
            Calories = calories;
            Protein = protein;
            Carbohydrates = carbohydrates;
            Fat = fat;
            VitaminC = vitaminC;
        }

        public override string ToString()
        {
            return $"{Name}: {Calories} Calories, {Protein}g Protein, {Carbohydrates}g Carbs, {Fat}g Fat, {VitaminC}mg Vitamin C";
        }
    }

    public class AVLTreeNode
    {
        public NewFood Data { get; set; }
        public AVLTreeNode? Left { get; set; }
        public AVLTreeNode? Right { get; set; }
        public int Height { get; set; }

        public AVLTreeNode(NewFood data)
        {
            Data = data;
            Left = null;
            Right = null;
            Height = 1;
        }
    }

    public class AVLTree
    {
        private AVLTreeNode? root;

        public AVLTree()
        {
            root = null;
        }

        public void Insert(NewFood food)
        {
            root = InsertRec(root, food);
        }

        private AVLTreeNode InsertRec(AVLTreeNode? node, NewFood food)
        {
            if (node == null)
            {
                return new AVLTreeNode(food);
            }

            if (string.Compare(food.Name, node.Data.Name, StringComparison.OrdinalIgnoreCase) < 0)
            {
                node.Left = InsertRec(node.Left, food);
            }
            else if (string.Compare(food.Name, node.Data.Name, StringComparison.OrdinalIgnoreCase) > 0)
            {
                node.Right = InsertRec(node.Right, food);
            }
            else
            {
                return node;
            }

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            if (balance > 1 && string.Compare(food.Name, node.Left!.Data.Name, StringComparison.OrdinalIgnoreCase) < 0)
            {
                return RightRotate(node);
            }

            if (balance < -1 && string.Compare(food.Name, node.Right!.Data.Name, StringComparison.OrdinalIgnoreCase) > 0)
            {
                return LeftRotate(node);
            }

            if (balance > 1 && string.Compare(food.Name, node.Left!.Data.Name, StringComparison.OrdinalIgnoreCase) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            if (balance < -1 && string.Compare(food.Name, node.Right!.Data.Name, StringComparison.OrdinalIgnoreCase) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        private int GetHeight(AVLTreeNode? node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(AVLTreeNode? node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private AVLTreeNode RightRotate(AVLTreeNode y)
        {
            AVLTreeNode x = y.Left!;
            AVLTreeNode T2 = x.Right!;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private AVLTreeNode LeftRotate(AVLTreeNode x)
        {
            AVLTreeNode y = x.Right!;
            AVLTreeNode T2 = y.Left!;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public List<NewFood> InOrderTraversal()
        {
            List<NewFood> foods = new List<NewFood>();
            InOrderRec(root, foods);
            return foods;
        }

        private void InOrderRec(AVLTreeNode? node, List<NewFood> foods)
        {
            if (node != null)
            {
                InOrderRec(node.Left, foods);
                foods.Add(node.Data);
                InOrderRec(node.Right, foods);
            }
        }
    }

    static AVLTree newFoodTree = new AVLTree();

    static void GenerateBalancedMealPlan()
    {
        if (currentUser.FoodLog.Count < 5)
        {
            Console.WriteLine("Not enough food items to generate a balanced meal plan. Please add at least 5 food items to MY FOOD LIST.");
            return;
        }

        foreach (var food in currentUser.FoodLog)
        {
            NewFood newFood = new NewFood(food.Name, food.Calories, food.Protein, food.Carbohydrates, food.Fat, food.VitaminC);
            newFoodTree.Insert(newFood);
        }

        List<NewFood> foods = newFoodTree.InOrderTraversal();
        Console.WriteLine("\n");
        Console.WriteLine("\nBalanced Meal Plan:");
        Console.WriteLine("===================================");


        Console.WriteLine("\nBreakfast:");
        for (int i = 0; i < foods.Count / 3; i++)
        {
            Console.WriteLine(foods[i]);
        }

        Console.WriteLine("\nLunch:");
        for (int i = foods.Count / 3; i < 2 * foods.Count / 3; i++)
        {
            Console.WriteLine(foods[i]);
        }

        Console.WriteLine("\nDinner:");
        for (int i = 2 * foods.Count / 3; i < foods.Count; i++)
        {
            Console.WriteLine(foods[i]);
        }

        Console.WriteLine("===================================");
    }

}