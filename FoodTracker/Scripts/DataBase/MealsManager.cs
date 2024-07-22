using MongoDB.Driver;

namespace FoodTracker.Scripts.DataBase
{
    public class MealsManager(string collectionName, IMongoDatabase db) : CollectionManager<MongoMeal>(collectionName, db)
    {
        protected override List<string> validateData(MongoMeal data)
        {
            return ["Not yet implemented"];
        }

        protected override UpdateDefinition<MongoMeal> CreateUpdateDefinition(MongoMeal newData)
        {
            throw new NotImplementedException();
        }
    }
}
