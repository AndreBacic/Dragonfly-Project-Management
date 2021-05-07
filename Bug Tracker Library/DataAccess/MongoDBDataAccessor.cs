using Bug_Tracker_Library.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Bug_Tracker_Library.DataAccess
{
    public class MongoDBDataAccessor : IDataAccessor
    {
        private IMongoDatabase db;

        private UserModel _user;
        private OrganizationModel _organization;
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
        public MongoDBDataAccessor(IConfiguration configuration, UserModel user, OrganizationModel organization)
        {
            var client = new MongoClient();
            string database = configuration.GetConnectionString("MongoDB");
            db = client.GetDatabase(database);

            // _user and _organization are passed by reference so this can track their changes and save them easily
            _user = user;
            _organization = organization;
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


        //####################################### BASIC MONGOBD METHODS ##########################################

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
            var filter = Builders<T>.Filter.Eq(ModelIdName, id); // Eq id for equals, ctrl+J to see other comparisons

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
            var filter = Builders<T>.Filter.Eq(ModelIdName, id); // Eq id for equals, ctrl+J to see other comparisons
            collection.DeleteOne(filter);
        }


        //####################################### INTERFACE IMPLEMENTATION ##########################################

        public void SetGlobalUser(UserModel user)
        {
            this._user = user;
        }

        public void SetGlobalOrganization(OrganizationModel organization)
        {
            this._organization = organization;
        }

        public void CreasteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException("This doesn't work in mongo. Use UpdateUser");
        }
        public void CreateComment(CommentModel model)
        {
            throw new NotImplementedException("This doesn't work in mongo. Use UpdateOrganization");
        }
        public void CreateProject(ProjectModel model)
        {
            throw new NotImplementedException("This doesn't work in mongo. Use UpdateOrganization");
        }

        public bool CreateOrganization(OrganizationModel model)
        {
            List<OrganizationModel> organizations = LoadRecords<OrganizationModel>(OrganizationCollection);
            
            // Ensure that no organizations with that name or password already exist
            if (organizations.Where(x => x.GuidId == model.GuidId).Any()) { return false; }
            if (organizations.Where(x => x.PasswordHash == model.PasswordHash).Any()) { return false; }

            // No conflicts, so the record can be inserted.
            InsertRecord(OrganizationCollection, model);
            return true;
        }

        public bool CreateUser(UserModel model)
        {
            List<UserModel> users = LoadRecords<UserModel>(UserCollection);

            // do not insert record if password already used.
            if (users.Any(x => x.PasswordHash == model.PasswordHash)) { return false; }

            // valid password; insert
            InsertRecord(UserCollection, model);
            return true;
        }

        public OrganizationModel GetOrganization(string organizationName, string passwordHash)
        {
            var collection = db.GetCollection<OrganizationModel>(OrganizationCollection);
            var filter = Builders<OrganizationModel>.Filter.Eq("Name", organizationName);

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
        
        public UserModel GetUser(int id)
        {
            throw new NotImplementedException("This overload does not work for MongoDB data access. Use GetUser(Guid id).");
        }
        public UserModel GetUser(Guid id)
        {
            return LoadRecordById<UserModel>(UserCollection, id);
        }

        public void UpdateOrganization(OrganizationModel model)
        {
            UpsertRecord(OrganizationCollection, model.GuidId, model);
        }

        public void UpdateUser(UserModel model)
        {
            UpsertRecord(UserCollection, model.GuidId, model);
        }
    }
}
