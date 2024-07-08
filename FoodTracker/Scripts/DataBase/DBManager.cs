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

        private void LoadCollections()
        {
            if (!Connected) return;

            IMongoDatabase db = _client.GetDatabase("FoodTracker");

            _foodItemCollection = db.GetCollection<MongoFoodItem>("FoodItems");
        }

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
            if (foodItem == null) //Make sure we were actually given a food item to begin with
            {
                returnMessages.Add(ErrorUtils.Messages.IsNull("foodItem"));
                return (false, returnMessages);
            }

            //Input Validation
            if (foodItem.Name == null) returnMessages.Add(ErrorUtils.Messages.IsNull("Name"));
            else if (_foodItemCollection.AsQueryable().Where(i => i.Name == foodItem.Name).FirstOrDefault() != null) returnMessages.Add(ErrorUtils.Messages.AlreadyExists("Name", foodItem.Name)); //Make sure there are no duplicate named items
            if (foodItem.Calories < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Calories"));
            if (foodItem.Protein < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Protein"));
            if (foodItem.Carbs < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Carbs"));
            if (foodItem.Fat < 0) returnMessages.Add(ErrorUtils.Messages.IsNegative("Fat"));

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
    }
}
