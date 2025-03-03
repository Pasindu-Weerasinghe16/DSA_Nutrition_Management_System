using System;
using System.Collections.Generic;
// Add this line if Meal class is in the same namespace

namespace NutritionManagementSystem.Models
{
    public class MealPlan
    {
        public string Name { get; set; }
        public Dictionary<string, MealPlan> Meals { get; private set; }

        public MealPlan(string name)
        {
            Name = name;
            Meals = new Dictionary<string, MealPlan>();
        }

        public void AddMeal(string mealType, MealPlan meal)
        {
            Meals[mealType] = meal;
        }

        public void RemoveMeal(string mealType)
        {
            Meals.Remove(mealType);
        }

        public double TotalCalories()
        {
            double total = 0;
            foreach (var meal in Meals.Values)
            {
                total += meal.TotalCalories();
            }
            return total;
        }

        public double TotalProtein()
        {
            double total = 0;
            foreach (var meal in Meals.Values)
            {
                total += meal.TotalProtein();
            }
            return total;
        }

        public double TotalCarbohydrates()
        {
            double total = 0;
            foreach (var meal in Meals.Values)
            {
                total += meal.TotalCarbohydrates();
            }
            return total;
        }

        public double TotalFat()
        {
            double total = 0;
            foreach (var meal in Meals.Values)
            {
                total += meal.TotalFat();
            }
            return total;
        }

        public double TotalVitaminC()
        {
            double total = 0;
            foreach (var meal in Meals.Values)
            {
                total += meal.TotalVitaminC();
            }
            return total;
        }
    }
}