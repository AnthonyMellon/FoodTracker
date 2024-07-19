using MongoDB.Driver;

namespace FoodTracker.Scripts.DataBase
{
    public class DaysManager(string collectionName) : CollectionManager<MongoDay>(collectionName)
    {
        protected override List<string> validateData(MongoDay data)
        {
            return ["Not yet implemented"];
        }

        protected override UpdateDefinition<MongoDay> CreateUpdateDefinition(MongoDay newData)
        {
            throw new NotImplementedException();
        }
    }
}
