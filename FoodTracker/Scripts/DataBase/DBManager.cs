using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodTracker.Scripts.DataBase
{
    public class DBManager
    {
        private string? _URIConnectionString = null;
        private MongoClient? _client;
        public bool Connected { get; private set; }

        public DBManager(string? URIConnectionString)
        {
            _URIConnectionString = URIConnectionString;
            if (_URIConnectionString == null)
            {
                Console.WriteLine("Failed to get URIConnection String. This needs to be handled"); //TODO handle this case                
            }

            EstablishConnection();
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
            Connected = false;
            _client = null;
        }

        private bool TestConnection()
        {
            if (_client == null) return false;

            //Attempt a ping
            try
            {
                BsonDocument result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));

                Connected = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Connected = false;
                return false;
            }
        }
    }
}
