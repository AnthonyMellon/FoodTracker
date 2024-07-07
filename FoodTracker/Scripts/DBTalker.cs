namespace FoodTracker.Scripts
{
    using MongoDB.Driver;
    using MongoDB.Bson;

    public static class DBTalker
    {
        public static bool Connected { get; private set; } = false;
        private static MongoClient? _client;

        public static bool EstablishNewConnection(string connectionUri)
        {
            DropCurrentConnection();

            // Set the ServerApi field of the settings object to set the version of the stable API on the client
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            _client = new MongoClient(settings);

            //Send a ping to confirm a successful connection
            return TestConnection().success;
        }

        public static void DropCurrentConnection()
        {
            Connected = false;
            _client = null;
        }

        public static (bool success, Exception? exception) TestConnection()
        {
            if (_client == null) return (false, null);

            //Attempt a ping
            try
            {
                BsonDocument result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Connection successful!");

                Connected = true;                
                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                Connected = false;                
                return (false, ex);
            }            
        }
    }
}
