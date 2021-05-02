using Bug_Tracker_Library.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Bug_Tracker_Library.DataAccess
{
    public class MongoDBDataAccessor : IDataAccessor
    {
        private IMongoDatabase db;

        private const string OrganizationCollection = "Organizations";
        private const string UserCollection = "Users";

        /// <summary>
        /// Initialize database using the configuration supplied by DependencyInjection.
        /// </summary>
        /// <param name="configuration"></param>
        public MongoDBDataAccessor(IConfiguration configuration)
        {
            var client = new MongoClient();
            string database = configuration.GetConnectionString("MongoDB");
            db = client.GetDatabase(database);
        }

        /// <summary>
        /// Initialize database using a known connection string.
        /// </summary>
        /// <param name="database"></param>
        public MongoDBDataAccessor(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id); // Eq id for equals, ctrl+J to see other comparisons

            return collection.Find(filter).First();
        }

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);

            var result = collection.ReplaceOne(
                new BsonDocument("_id", new BsonBinaryData(id, GuidRepresentation.Standard)), // the BsonBinaryData with GuidRep is the not obsolete way
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id); // Eq id for equals, ctrl+J to see other comparisons
            collection.DeleteOne(filter);
        }

        public void CreasteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(CommentModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateOrganization(OrganizationModel model)
        {
            InsertRecord(OrganizationCollection, model);
        }

        public void CreateProject(ProjectModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserModel model)
        {
            InsertRecord(UserCollection, model);
        }

        public OrganizationModel GetOrganization(string organizationName, string password)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
