using MongoDB.Bson;
using MongoDB.Driver;
using FoodTracker.Scripts.Utils;

namespace FoodTracker.Scripts.DataBase
{
    public class DBManager
    {
        private string? _URIConnectionString = null;
        private MongoClient? _client;
        private bool _connected;
        public bool Connected => _connected && _client != null;

        //Collections
        private IMongoCollection<MongoFoodItem>? _foodItemCollection;

        public DBManager(string? URIConnectionString)
        {
            _URIConnectionString = URIConnectionString;
            if (_URIConnectionString == null)
            {
                Console.WriteLine("Failed to get URIConnection String. This needs to be handled"); //TODO handle this case                
            }

            EstablishConnection();
            LoadCollections();
        }

        #region Setup Stuff
        private void LoadCollections()
        {
            if (!Connected) return;

            IMongoDatabase db = _client.GetDatabase("FoodTracker");

            _foodItemCollection = db.GetCollection<MongoFoodItem>("FoodItems");
        }
        #endregion

        #region Connection Stuff
        private bool EstablishConnection()
        {
            DropConnection();
            if (_URIConnectionString == null) return false;

            // Set the serverAPi field of the settings object to set hte version of the stable API on the client
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_URIConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            _client = new MongoClient(settings);

            //Send a ping ton confirm successful connection
            return TestConnection();
        }

        private void DropConnection()
        {
            _connected = false;
            _client = null;
        }

        private bool TestConnection()
        {
            if (_client == null) return false;

            //Attempt a ping
            try
            {
                BsonDocument result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));

                _connected = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _connected = false;
                return false;
            }
        }
        #endregion

        #region FoodItemStuff
        public List<MongoFoodItem>? GetAllFoodItems()
        {
            if (!Connected) return null;

            FilterDefinition<MongoFoodItem> filter = Builders<MongoFoodItem>.Filter.Empty;

            return _foodItemCollection.Find(filter).ToList();
        }

        public (bool success, List<string> messages) TryInsertFoodItem(MongoFoodItem foodItem)
        {
            List<string> returnMessages = new List<string>();

            if (!Connected) //Check if connected to DB
            {
                returnMessages.Add("not connected to database");
                return (false, returnMessages);
            }

            //Input Validation
            returnMessages = ValidateFoodItem(foodItem);

            //If there's some return messages, validation has failed. Return all the reasons why
            if (returnMessages.Count != 0) return (false, returnMessages);

            //Input validation passed, try insert to db and see if it accepts the food item
            try
            {
                _foodItemCollection?.InsertOne(foodItem);
            }
            catch (Exception ex) //Womp womp
            {
                returnMessages.Add(ex.ToString());
                return (false, returnMessages);
            }

            //Success!
            returnMessages.Add($"Successfully created {foodItem.Name}");
            return (true, returnMessages);
        }

        public (bool success, List<string> messages) TryUpdateFoodItem(MongoFoodItem newData)
        {
            //Make sure the new data is valid
            List<string> returnMessages = ValidateFoodItem(newData);
            if (returnMessages.Count != 0) return (false, returnMessages);

            //New data is valid
            ObjectId id = newData.Id;

            FilterDefinition<MongoFoodItem> filter = Builders<MongoFoodItem>.Filter.Eq(foodItem => foodItem.Id, id);
            UpdateDefinition<MongoFoodItem> update = Builders<MongoFoodItem>.Update
                .Set(foodItem => foodItem.Name, newData.Name)
                .Set(fooditem => fooditem.Calories, newData.Calories)
                .Set(foodItem => foodItem.Protein, newData.Protein)
                .Set(foodItem => foodItem.Carbs, newData.Carbs)
                .Set(foodItem => foodItem.Fat, newData.Fat);                

            try
            {
                _foodItemCollection?.UpdateOne(filter, update);
                returnMessages.Add($"Successfully updated {newData.Name}");
                return (true, returnMessages);
            }
            catch(Exception ex)
            {
                returnMessages.Add(ex.ToString());
                return (false, returnMessages);
            }
            
        }

        /// <summary>
        /// Ensures a food item's values are all ok
        /// </summary>
        /// <param name="foodItem">The food item to check</param>
        /// <returns>A list of messages describing any issues found with <paramref name="foodItem"/></returns>
        private List<string> ValidateFoodItem(MongoFoodItem foodItem)
        {
            List<string> returnMessages = new List<string>();

            if (foodItem == null) //Make sure there's an acutal food item being passed in. No point in continuing if there isn't
            {
                returnMessages.Add(ErrorUtils.Messages.IsNull("food item"));
                return returnMessages;
            }

            if (foodItem.Name == null) returnMessages.Add(ErrorUtils.Messages.IsNull("Name"));
            if (foodItem.Name == "") returnMessages.Add(ErrorUtils.Messages.IsEmpty("Name"));
            if (foodItem.Calories < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Calories"));
            if (foodItem.Protein < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Protein"));
            if (foodItem.Carbs < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Carbs"));
            if (foodItem.Fat < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Fat"));
            if (_foodItemCollection.AsQueryable().Where( //Check to make sure there are no other food items with the same name
                    i => i.Id != foodItem.Id && //No need to check against itself (relevant when editing a food item)
                    i.Name == foodItem.Name
                ).FirstOrDefault() != null)
            {
                returnMessages.Add(ErrorUtils.Messages.AlreadyExists("Name", foodItem.Name));
            }

            return returnMessages;
        }
        #endregion
    }
}
