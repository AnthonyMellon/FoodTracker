using FoodTracker.Scripts.Utils;
using MongoDB.Bson;

namespace FoodTracker.Scripts
{
    public class FoodItem
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public float Calories { get; private set; }
        public Macro Protein { get; private set; }
        public Macro Carbs { get; private set; }
        public Macro Fat { get; private set; }

        public FoodItem(ObjectId id, string name, float numCalories, float numProtein, float numCarbs, float numFat)
        {
            this.Id = id;
            this.Name = name;
            this.Calories = numCalories;
            this.Protein = new Macro(Macros.Protein, numProtein);
            this.Carbs = new Macro(Macros.Carb, numCarbs);
            this.Fat = new Macro(Macros.Fat, numFat);
        }

        public override string ToString()
        {
            return Name.Replace('_', ' ');
        }
    }
}
