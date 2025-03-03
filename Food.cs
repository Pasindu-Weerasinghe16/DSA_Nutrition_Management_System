namespace NutritionManagement
{
    public class Food
    {
        public double Index { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
        public double VitaminC { get; set; }

        public Food(double index, string name, double calories, double protein, double carbohydrates, double fat, double vitaminC)
        {
            Index = index;
            Name = name;
            Calories = calories;
            Protein = protein;
            Carbohydrates = carbohydrates;
            Fat = fat;
            VitaminC = vitaminC;
        }

        public override string ToString()
        {
            return $"{Name} - Calories: {Calories}, Protein: {Protein}g, Carbs: {Carbohydrates}g, Fat: {Fat}g, Vitamin C: {VitaminC}mg";
        }
    }
}