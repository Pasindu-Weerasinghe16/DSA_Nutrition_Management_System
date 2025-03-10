using DSA;

namespace NutritionManagement
{
    public class SearchResults
    {
        public DynamicArray<Food> Res { get; set; }

        public
        SearchResults()
        {
            Res = new DynamicArray<Food>();
        }

        public void ShowResults()
        {
            if (Res == null || Res.Count == 0)
            {
                Console.WriteLine("No results found.");
                return;
            }


            int terminalWidth = Console.WindowWidth;


            int idWidth = 10;
            int caloriesWidth = 20;
            int proteinWidth = 20;
            int carbsWidth = 20;
            int fatWidth = 20;
            int vitaminCWidth = 20;


            int remainingSpace = terminalWidth - (idWidth + caloriesWidth + proteinWidth + carbsWidth + fatWidth + vitaminCWidth + 19);
            int nameWidth = Math.Max(20, remainingSpace);


            string idCol = "ID";
            string nameCol = "Food Name";
            string calCol = "Calories";
            string proteinCol = "Protein";
            string carbsCol = "Carbs";
            string fatCol = "Fat";
            string vitCCol = "Vit C";


            string topLine = "┌" + new string('─', idWidth + 2) +
                      "┬" + new string('─', nameWidth + 2) +
                      "┬" + new string('─', caloriesWidth + 2) +
                      "┬" + new string('─', proteinWidth + 2) +
                      "┬" + new string('─', carbsWidth + 2) +
                      "┬" + new string('─', fatWidth + 2) +
                      "┬" + new string('─', vitaminCWidth + 2) + "┐";

            string midLine = "├" + new string('─', idWidth + 2) +
                             "┼" + new string('─', nameWidth + 2) +
                             "┼" + new string('─', caloriesWidth + 2) +
                             "┼" + new string('─', proteinWidth + 2) +
                             "┼" + new string('─', carbsWidth + 2) +
                             "┼" + new string('─', fatWidth + 2) +
                             "┼" + new string('─', vitaminCWidth + 2) + "┤";

            string bottomLine = "└" + new string('─', idWidth + 2) +
                                "┴" + new string('─', nameWidth + 2) +
                                "┴" + new string('─', caloriesWidth + 2) +
                                "┴" + new string('─', proteinWidth + 2) +
                                "┴" + new string('─', carbsWidth + 2) +
                                "┴" + new string('─', fatWidth + 2) +
                                "┴" + new string('─', vitaminCWidth + 2) + "┘";

            Console.WriteLine("\nSearch Results:");
            Console.WriteLine(topLine);


            Console.WriteLine(
                $"│ {idCol.PadRight(idWidth)} │ {nameCol.PadRight(nameWidth)} │ {calCol.PadRight(caloriesWidth)} │ " +
                $"{proteinCol.PadRight(proteinWidth)} │ {carbsCol.PadRight(carbsWidth)} │ {fatCol.PadRight(fatWidth)} │ {vitCCol.PadRight(vitaminCWidth)} │"
            );

            Console.WriteLine(midLine);

            int id = 1;
            for (int i = 0; i < Math.Min(200, Res.Count); i++)
            {
                Food food = Res.At(i);
                Console.WriteLine(
                    $"│ {id.ToString().PadRight(idWidth)} │ {food.Name.PadRight(nameWidth)} │ " +
                    $"{food.Calories.ToString("F3").PadRight(caloriesWidth)} │ {food.Protein.ToString("F3").PadRight(proteinWidth)} │ " +
                    $"{food.Carbohydrates.ToString("F3").PadRight(carbsWidth)} │ {food.Fat.ToString("F3").PadRight(fatWidth)} │ {food.VitaminC.ToString("F3").PadRight(vitaminCWidth)} │"
                );
                id++;
            }

            Console.WriteLine(bottomLine);
        }
    }
}