namespace FoodTracker.Scripts.Utils
{
    public static class DebugUtils
    {
        public static List<FoodItem> DummyFoods = new List<FoodItem>()
        {
            new FoodItem("Protein_Thing", 100, 10, 0, 0),
            new FoodItem("Carb_Thing", 100, 0, 10, 0),
            new FoodItem("Fat_Thing", 100, 0, 0, 10),
            new FoodItem("Empty_Calories", 100, 0, 0, 0)
        };
    }
}
