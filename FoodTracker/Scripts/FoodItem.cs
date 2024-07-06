namespace FoodTracker.Scripts
{
    public class FoodItem
    {
        public string name { get; private set; }
        public float calories { get; private set; }
        public Macro protein { get; private set; }
        public Macro carbs { get; private set; }
        public Macro fat { get; private set; }

        public FoodItem(string name, float calories, float protein, float carbs, float fat)
        {
            this.name = name;
            this.calories = calories;
            this.protein = new Macro(Macros.Protein, protein);
            this.carbs = new Macro(Macros.Carb, carbs);
            this.fat = new Macro(Macros.Fat, fat);
        }

        public override string ToString()
        {
            return name.Replace('_', ' ');
        }
    }
}
