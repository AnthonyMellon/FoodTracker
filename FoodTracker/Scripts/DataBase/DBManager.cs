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

        public (bool success, string message) TryInsertFoodItem(MongoFoodItem foodItem)
        {
            if (!Connected) return (false, "not connected to database");
            if (foodItem == null) return (false, "foodItem is null");

            //Do input validation here

            try
            {
                _foodItemCollection.InsertOne(foodItem);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
            return (true, $"Successfully inserted {foodItem.Name}");
        }
    }
}
