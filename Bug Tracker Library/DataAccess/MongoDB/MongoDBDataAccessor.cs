using Bug_Tracker_Library.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bug_Tracker_Library.DataAccess
{
    public class MongoDBDataAccessor : IDataAccessor
    {
        private IMongoDatabase db;
        /// <summary>
        /// Stores organizations, with their projects and comments
        /// </summary>
        private const string OrganizationCollection = "Organizations";
        /// <summary>
        /// Stores users and their assignments
        /// </summary>
        private const string UserCollection = "Users";

        private const string ModelIdName = "GuidId";

        /// <summary>
        /// Initialize database using the configuration supplied by DependencyInjection.
        /// </summary>
        /// <param name="configuration"></param>
        public MongoDBDataAccessor(IConfiguration configuration)
        {
            MongoClient client = new MongoClient();
            string database = configuration.GetConnectionString("MongoDB");
            db = client.GetDatabase(database);
        }

        /// <summary>
        /// Initialize database using a known connection string.
        /// </summary>
        /// <param name="database"></param>
        public MongoDBDataAccessor(string database)
        {
            MongoClient client = new MongoClient();
            db = client.GetDatabase(database);
        }


        //####################################### BASIC MONGOBD METHODS ##########################################

        public void InsertRecord<T>(string table, T record)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(table);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ModelIdName, id); // Eq id for equals, ctrl+J to see other comparisons

            return collection.Find(filter).First();
        }

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(table);

            ReplaceOneResult result = collection.ReplaceOne(
                new BsonDocument("_id", new BsonBinaryData(id, GuidRepresentation.Standard)), // the BsonBinaryData with GuidRep is the not obsolete way
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(table);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ModelIdName, id); // Eq id for equals, ctrl+J to see other comparisons
            collection.DeleteOne(filter);
        }


        //####################################### INTERFACE IMPLEMENTATION ##########################################

        public void CreasteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException("This doesn't work in mongo. Use UpdateUser");
        }
        public void CreateComment(CommentModel model, Guid organizationId, List<int> indexTree)
        {
            throw new NotImplementedException("This doesn't work in mongo. Use UpdateOrganization");
        }
        public void CreateProject(ProjectModel model, Guid organizationId, List<int> indexTree)
        {
            throw new NotImplementedException("This doesn't work in mongo. Use UpdateOrganization");
        }

        public bool CreateOrganization(OrganizationModel model)
        {
            List<OrganizationModel> organizations = LoadRecords<OrganizationModel>(OrganizationCollection);

            // Ensure that no organizations with that name or password already exist
            if (organizations.Where(x => x.Id == model.Id).Any())
            { return false; }
            if (organizations.Where(x => x.PasswordHash == model.PasswordHash).Any())
            { return false; }

            // No conflicts, so the record can be inserted.
            InsertRecord(OrganizationCollection, model);
            return true;
        }

        public bool CreateUser(UserModel model)
        {
            List<UserModel> users = LoadRecords<UserModel>(UserCollection);

            // do not insert record if password already used.
            if (users.Any(x => x.PasswordHash == model.PasswordHash))
            { return false; }

            // valid password; insert
            InsertRecord(UserCollection, model);
            return true;
        }

        public OrganizationModel GetOrganization(string organizationName, string passwordHash)
        {
            IMongoCollection<OrganizationModel> collection = db.GetCollection<OrganizationModel>(OrganizationCollection);
            FilterDefinition<OrganizationModel> filter = Builders<OrganizationModel>.Filter.Eq("Name", organizationName);

            OrganizationModel model = collection.Find(filter).First();
            if (model.PasswordHash == passwordHash)
            {
                return model;
            }
            else
            {
                return null;
            }
        }

        public UserModel GetUser(Guid id)
        {
            return LoadRecordById<UserModel>(UserCollection, id);
        }

        public void UpdateOrganization(OrganizationModel model)
        {
            UpsertRecord(OrganizationCollection, model.Id, model);
        }

        public void UpdateUser(UserModel model)
        {
            UpsertRecord(UserCollection, model.Id, model);
        }

        public List<UserModel> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
