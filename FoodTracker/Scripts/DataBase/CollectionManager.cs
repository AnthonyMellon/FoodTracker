using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodTracker.Scripts.DataBase
{
    public abstract class CollectionManager<T> where T: CollectionBase
    {
        protected IMongoCollection<T>? _collection;
        protected string _collectionName;

        public CollectionManager(string collectionName)
        {
            _collectionName = collectionName;
        }

        public virtual void LoadCollection(IMongoDatabase db)
        {
            _collection = db.GetCollection<T>(_collectionName);
        }

        /// <summary>
        /// Get a list of all items in the collection
        /// </summary>
        /// <returns>List of all items in the collection</returns>
        public virtual List<T> GetAllItems()
        {
            Console.WriteLine(typeof(T));

            FilterDefinition<T> filter = Builders<T>.Filter.Empty;

            return _collection.Find(filter).ToList();
        }

        /// <summary>
        /// Attempt to delete an item with <paramref name="id"/> from the collection
        /// </summary>
        /// <param name="id">The id of the item to be deleted</param>
        /// <returns>true / false depending on success along with a list of messages detailing what happened</returns>
        public virtual (bool success, List<string> messages) TryDeleteItem(ObjectId id)
        {
            List<string> returnMessages = new List<string>();

            FilterDefinition<T> filter = Builders<T>.Filter.Eq(item => item.Id, id);
            try //Try delete the food item
            {
                _collection?.DeleteOne(filter);
                returnMessages.Add($"Deleted {id}");
                return (true, returnMessages);
            }
            catch (Exception ex) //Failed to delete the item
            {
                returnMessages.Add(ex.ToString());
                return (false, returnMessages);
            }
        }

        /// <summary>
        /// Attempts to insert <paramref name="item"/> into the collection
        /// </summary>
        /// <param name="item">The item to be inserted</param>
        /// <returns>true / false depending on success along with a list of messages detailing what happened</returns>
        public virtual (bool success, List<string> messages) TryInsertItem(T item)
        {
            List<string> returnMessages = new List<string>();

            //Validate data
            returnMessages = validateData(item);

            //If there's some return messages, validation has failed. Return all the reasons why
            if (returnMessages.Count != 0) return (false, returnMessages);

            //Data is valid, try insert to db and see if it accepts
            try
            {
                _collection?.InsertOne(item);
            }
            catch (Exception ex) //Womp womp
            {
                returnMessages.Add(ex.ToString());
                return (false, returnMessages);
            }

            //Success!
            returnMessages.Add($"Successfully created {item.Id}");
            return (true, returnMessages);
        }

        /// <summary>
        /// Attempts to update the item in collection with matching id to <paramref name="newData"/> to match <paramref name="newData"/>        
        /// </summary>
        /// <param name="newData">The new data</param>
        /// <returns>true / false depending on success along with a list of messages detailing what happened</returns>
        public virtual (bool success, List<string> messages) TryUpdateItem(T newData)
        {
            List<string> returnMessages = new List<string>();

            //Make sure the new data is valid
            returnMessages = validateData(newData);
            if (returnMessages.Count != 0) return (false, returnMessages);

            //New data is valid
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(item => item.Id, newData.Id);
            UpdateDefinition<T> update = CreateUpdateDefinition(newData);

            try //Attempt to update item
            {
                _collection?.UpdateOne(filter, update);
                returnMessages.Add($"Successfully updated {newData.Id}");
                return (true, returnMessages);
            }
            catch (Exception ex) //Failed :(
            {
                returnMessages.Add(ex.ToString());
                return (false, returnMessages);
            }
        }

        /// <summary>
        /// Ensures an item's data is valid
        /// </summary>
        /// <param name="data">The data to check</param>
        /// <returns>A list of messages describing any issues found with <paramref name="data"/></returns>
        protected abstract List<string> validateData(T data);

        protected abstract UpdateDefinition<T> CreateUpdateDefinition(T newData);
    }
}
