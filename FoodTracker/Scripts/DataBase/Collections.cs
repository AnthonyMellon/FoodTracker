using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodTracker.Scripts.DataBase
{    
    public abstract class CollectionBase
    {
        public ObjectId Id { get; set; }
    }
    
    public class MongoFoodItem : CollectionBase
    {
        public string? Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
    }
    
    public class MongoMeal : CollectionBase
    {
        public string? Name { get; set; }
        public ObjectId[]? FoodItems { get; set; }        
    }
    
    public class MongoDay : CollectionBase
    {
        public DateTime? date { get; set; }
        public ObjectId[]? foodItems { get; set; }
        public ObjectId[]? meals { get; set; }
    }
}
