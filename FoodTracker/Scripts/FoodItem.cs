using FoodTracker.Scripts.Utils;

namespace FoodTracker.Scripts
{
    public class FoodItem
    {
        public string name { get; private set; }
        public float calories { get; private set; }
        public Macro protein { get; private set; }
        public Macro carbs { get; private set; }
        public Macro fat { get; private set; }

        public FoodItem(string name, float numCalories, float numProtein, float numCarbs, float numFat)
        {
            this.name = name;
            this.calories = numCalories;
            this.protein = new Macro(Macros.Protein, numProtein);
            this.carbs = new Macro(Macros.Carb, numCarbs);
            this.fat = new Macro(Macros.Fat, numFat);
        }

        public override string ToString()
        {
            return name.Replace('_', ' ');
        }
    }
}
