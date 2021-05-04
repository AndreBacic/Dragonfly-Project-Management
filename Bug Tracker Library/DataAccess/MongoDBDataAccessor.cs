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

        private const string OrganizationCollection = "Organizations";
        private const string UserCollection = "Users";

        private const string ModelIdName = "GuidId";

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

        public void CreasteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(CommentModel model)
        {
            throw new NotImplementedException();
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

        public void CreateProject(ProjectModel model, Guid organizationId)
        {
            OrganizationModel org = LoadRecordById<OrganizationModel>(OrganizationCollection, organizationId);

            // TODO: Figure out how to sensibly insert a project into a layred hirearchy.
            // Ok I figured it out:
            // - Use int Ids for projects
            // - Have a mongo collection that records what the highest id for those Ids is
            // - Each time a project is added, it's given an the highest id + 1 and then the highest id is incremented
            // - When adding an project/assignment/comment to a project, a loop goes through the project hirearchy back to the base project and saves the id chain in a list
            // - Then, that id chain is used to go through the organization's project hirearchy and add the project/assignment/comment in the right place
            org.Projects.Add(model);

            UpsertRecord(OrganizationCollection, organizationId, org);
        }
        public void CreateProject(ProjectModel model, int organizationId)
        {
            throw new NotImplementedException("This overload does not work for MongoDB data access. Use CreateProject(..., Guid organizationId).");
        }


        public void CreateUser(UserModel model)
        {
            InsertRecord(UserCollection, model);
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
    }
}
