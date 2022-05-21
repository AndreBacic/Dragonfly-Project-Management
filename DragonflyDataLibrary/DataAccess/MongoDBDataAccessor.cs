using DragonflyDataLibrary.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonflyDataLibrary.DataAccess
{
    public class MongoDBDataAccessor : IDataAccessor
    {
        private readonly IMongoDatabase _db;
        /// <summary>
        /// Stores users and their assignments
        /// </summary>
        private const string _userCollection = "Users";

        private const string _modelIdName = "Id";
        private const string _userUniqueEmailName = "EmailAddress";

        /// <summary>
        /// Initialize database using the configuration supplied by DependencyInjection.
        /// </summary>
        /// <param name="configuration"></param>
        public MongoDBDataAccessor(IConfiguration configuration)
        {
            MongoClient client = new MongoClient();
            string database = configuration.GetConnectionString("MongoDB");
            _db = client.GetDatabase(database);
        }

        /// <summary>
        /// Initialize database using a known connection string.
        /// </summary>
        /// <param name="database"></param>
        public MongoDBDataAccessor(string database)
        {
            MongoClient client = new MongoClient();
            _db = client.GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(_modelIdName, id);

            return collection.Find(filter).First();
        }

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);

            collection.ReplaceOne(
                new BsonDocument("_id", new BsonBinaryData(id, GuidRepresentation.Standard)),
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(_modelIdName, id); // Eq = equals, ctrl+J to see other comparisons
            collection.DeleteOne(filter);
        }

        /// <summary>
        /// Performs a $lookup left join for a one to one relationship.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="localTable"></param>
        /// <param name="foreignTable"></param>
        /// <param name="id"></param>
        /// <param name="localField"></param>
        /// <param name="foreignField"></param>
        /// <param name="asField"></param>
        /// <returns></returns>
        public T LookupRecord<T>(string localTable, string foreignTable, Guid id,
            string localField, string foreignField, string asField)
        {
            var collection = _db.GetCollection<T>(localTable);
            var filter = Builders<T>.Filter.Eq(_modelIdName, id);

            var a = collection.Aggregate()
                .Match(filter)
                .Lookup(foreignTable, localField, foreignField, asField)
                .Unwind(asField)
                .As<T>().ToList();
            return a.First();
        }


        ///////////////////// INTERFACE IMPLEMENTATION /////////////////////

        public void CreateUser(UserModel user)
        {
            InsertRecord(_userCollection, user);
        }

        public UserModel GetUser(string emailAddress)
        {
            IMongoCollection<UserModel> collection = _db.GetCollection<UserModel>(_userCollection);
            FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq(_userUniqueEmailName, emailAddress);

            return collection.Find(filter).FirstOrDefault();
        }

        public void UpdateUser(UserModel user)
        {
            UpsertRecord(_userCollection, user.Id, user);
        }
    }
}
