using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    public class MongoDBDataAccessor : IDataAccessor
    {
        private IMongoDatabase _db;
        /// <summary>
        /// Stores organizations, with their projects and comments
        /// </summary>
        private const string _organizationCollection = "Organizations";
        /// <summary>
        /// Stores users and their assignments
        /// </summary>
        private const string _userCollection = "Users";

        private const string _modelIdName = "Id";

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


        //####################################### BASIC MONGOBD METHODS ##########################################

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
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(_modelIdName, id); // Eq id for equals, ctrl+J to see other comparisons

            return collection.Find(filter).First();
        }

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);

            collection.ReplaceOne(
                new BsonDocument("_id", new BsonBinaryData(id, GuidRepresentation.Standard)), // the BsonBinaryData with GuidRep is the not obsolete way
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            IMongoCollection<T> collection = _db.GetCollection<T>(table);
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(_modelIdName, id); // Eq id for equals, ctrl+J to see other comparisons
            collection.DeleteOne(filter);
        }


        //####################################### INTERFACE IMPLEMENTATION ##########################################

        public void CreateAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }

        public bool CreateOrganization(OrganizationModel model)
        {
            List<OrganizationModel> organizations = LoadRecords<OrganizationModel>(_organizationCollection);

            // Ensure that no organizations with that name already exist
            if (organizations.Where(x => x.Id == model.Id).Any())
            { return false; }

            // No conflicts, so the record can be inserted.
            InsertRecord(_organizationCollection, model);
            return true;
        }

        public bool CreateUser(UserModel model)
        {
            List<UserModel> users = GetAllUsers();

            // do not insert record if email already used.
            if (users.Any(x => x.EmailAddress == model.EmailAddress))
            { return false; }

            // valid password; insert
            InsertRecord(_userCollection, model);
            return true;
        }

        public void CreateProject(ProjectModel model, Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(CommentModel model, Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel GetOrganization(string organizationName, PasswordHashModel passwordHash)
        {
            IMongoCollection<OrganizationModel> collection = _db.GetCollection<OrganizationModel>(_organizationCollection);
            FilterDefinition<OrganizationModel> filter = Builders<OrganizationModel>.Filter.Eq("Name", organizationName);

            OrganizationModel model = collection.Find(filter).First();
            if (model.PasswordHash == passwordHash.ToDbString())
            {
                return model;
            }
            else
            {
                return null;
            }
        }

        public OrganizationModel GetOrganization(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> GetAllUsers()
        {
            return this.LoadRecords<UserModel>(_userCollection);
        }

        public UserModel GetUser(Guid id)
        {
            return LoadRecordById<UserModel>(_userCollection, id);
        }

        public UserModel GetUser(string emailAddress, PasswordHashModel passwordHash)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrganization(OrganizationModel model)
        {
            UpsertRecord(_organizationCollection, model.Id, model);
        }

        public void UpdateUser(UserModel model)
        {
            UpsertRecord(_userCollection, model.Id, model);
        }

        public void UpdateProject(ProjectModel model, Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public void UpdateComment(CommentModel model, Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrganization(OrganizationModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteProject(ProjectModel model, Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public void DeleteComment(CommentModel model, Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public void DeleteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
