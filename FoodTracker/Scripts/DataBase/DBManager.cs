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
        public FoodItemManager FoodItemManager { get; private set; }
        public MealsManager MealsManager { get; private set; }
        public DaysManager DaysManager { get; private set; }

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

        /// <summary>
        /// Loads collections to be used in the app
        /// </summary>
        private void LoadCollections()
        {
            if (!Connected) return;

            IMongoDatabase? db = _client?.GetDatabase("FoodTracker");
            if (db == null) return;

            FoodItemManager = new FoodItemManager("FoodItems", db);
            MealsManager = new MealsManager("Meals", db);
            DaysManager = new DaysManager("Days", db);
        }
        #endregion

        #region Connection Stuff
        /// <summary>
        /// Sets up connection to DB using _URIConnectionString
        /// </summary>
        /// <returns>returns true if connection tests successful, false if not</returns>
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

        /// <summary>
        /// Remove the current connection to the database
        /// </summary>
        private void DropConnection()
        {
            _connected = false;
            _client = null;
        }

        /// <summary>
        /// Ping the database to ensure it's connected
        /// </summary>
        /// <returns>true if connected, false if not</returns>
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
    }
}
