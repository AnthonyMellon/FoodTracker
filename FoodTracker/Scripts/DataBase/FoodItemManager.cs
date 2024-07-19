using FoodTracker.Scripts.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography.Xml;

namespace FoodTracker.Scripts.DataBase
{
    public class FoodItemManager(string collectionName) : CollectionManager<MongoFoodItem>(collectionName)
    {
        protected override List<string> validateData(MongoFoodItem data)
        {
            List<string> returnMessages = new List<string>();

            if(data == null) //Make sure there's an actual food item to validate in the first place
            {
                returnMessages.Add(ErrorUtils.Messages.IsNull("data"));
                return returnMessages;
            }

            //Validation
            if (data.Name == null) returnMessages.Add(ErrorUtils.Messages.IsNull("Name"));
            if (data.Name == "") returnMessages.Add(ErrorUtils.Messages.IsEmpty("Name"));
            if (data.Calories < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Calories"));
            if (data.Protein < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Protein"));
            if (data.Carbs < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Carbs"));
            if (data.Fat < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Fat"));
            if (_collection?.AsQueryable().Where( //Check to make sure there are no other food items with the same name
                    i => i.Id != data.Id && //No need to check against itself (relevant when editing a food item)
                    i.Name == data.Name
                ).FirstOrDefault() != null)
            {
                returnMessages.Add(ErrorUtils.Messages.AlreadyExists("Name", data.Name));
            }

            return returnMessages;
        }

        protected override UpdateDefinition<MongoFoodItem> CreateUpdateDefinition(MongoFoodItem newData)
        {
            return Builders<MongoFoodItem>.Update
                .Set(oldData => oldData.Name, newData.Name)
                .Set(oldData => oldData.Calories, newData.Calories)
                .Set(oldData => oldData.Protein, newData.Protein)
                .Set(oldData => oldData.Carbs, newData.Carbs)
                .Set(oldData => oldData.Fat, newData.Fat);
        }
    }
}
