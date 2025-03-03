namespace NutritionManagement
{
    public class User
    {
        public string Name { get; set; }
        public LinkedList<Food> FoodLog { get; } = new();

        public void AddToLog(Food food)
        {
            FoodLog.AddLast(food);
        }
    }
}