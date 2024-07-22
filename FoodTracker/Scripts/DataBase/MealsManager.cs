using MongoDB.Driver;
using FoodTracker.Scripts.Utils;

namespace FoodTracker.Scripts.DataBase
{
    public class MealsManager(string collectionName, IMongoDatabase db) : CollectionManager<MongoMeal>(collectionName, db)
    {
        protected override List<string> validateData(MongoMeal data)
        {
            List<String> returnMessages = new List<string>();

            if(data == null)
            {
                returnMessages.Add(ErrorUtils.Messages.IsNull("data"));
                return returnMessages;
            }

            //Validation
            if (data.Name == null) returnMessages.Add(ErrorUtils.Messages.IsNull("Name"));
            if (data.Name == "") returnMessages.Add(ErrorUtils.Messages.IsEmpty("Name"));
            if (_collection.AsQueryable().Where( //Check to make sure there are no other meals with the same name
                i => i.Id != data.Id && //No ned to check against itself (relevant when editing a meal)
                i.Name == data.Name
                ).FirstOrDefault() != null)
            {
                returnMessages.Add(ErrorUtils.Messages.AlreadyExists("Name", data.Name));
            }
            if (data.FoodItems == null) returnMessages.Add(ErrorUtils.Messages.IsNull("Food Items"));

            return returnMessages;
        }

        protected override UpdateDefinition<MongoMeal> CreateUpdateDefinition(MongoMeal newData)
        {
            throw new NotImplementedException();
        }
    }
}
