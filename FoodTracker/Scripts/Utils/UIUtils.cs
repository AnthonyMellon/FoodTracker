using MongoDB.Bson;

namespace FoodTracker.Scripts.Utils
{
    public class SelectableMongoItem
    {
        public SelectableMongoItem(ObjectId Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public ObjectId Id;
        public string Name = "";
    }
}
