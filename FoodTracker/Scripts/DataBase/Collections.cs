using MongoDB.Bson;

namespace FoodTracker.Scripts.DataBase
{
    public class MongoFoodItem
    {
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
    }
}
