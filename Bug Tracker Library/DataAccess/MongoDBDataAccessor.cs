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

        public bool CreateAssignment(AssignmentModel model)
        {
            UserModel u = GetUser(model.AssigneeId);
            if (u.Assignments.Any(a => a.OrganizationId == model.OrganizationId &&
                                  a.ProjectIdTreePath.Last() == model.ProjectIdTreePath.Last()))
            { return false; }

            u.Assignments.Add(model);
            UpdateUser(u);
            return true;
        }

        public bool CreateOrganization(OrganizationModel model)
        {
            List<OrganizationModel> organizations = LoadRecords<OrganizationModel>(_organizationCollection);

            // Ensure that no organizations with that name already exist
            if (organizations.Where(x => x.Name == model.Name).Any())
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
            OrganizationModel org = LoadRecordById<OrganizationModel>(_organizationCollection, organizationId);
            // this is ok because model.ParentIdTreePath is already filled out.
            if (model.ParentIdTreePath.Count > 0)
            {
                org.GetProjectByIdTree(model.ParentIdTreePath).SubProjects.Add(model);
            } else
            {
                org.Projects.Add(model);
            }
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void CreateComment(CommentModel model, Guid organizationId)
        {
            OrganizationModel org = LoadRecordById<OrganizationModel>(_organizationCollection, organizationId);
            // this is ok because model.ParentProjectIdTreePath is already filled out.
            org.GetProjectByIdTree(model.ParentProjectIdTreePath).Comments.Add(model);
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public OrganizationModel GetOrganization(Guid id)
        {
            return LoadRecordById<OrganizationModel>(_organizationCollection, id);
        }

        public List<UserModel> GetAllUsers()
        {
            return LoadRecords<UserModel>(_userCollection);
        }

        public Dictionary<Guid, UserModel> GetAllOrganizationUsers(Guid organizationId)
        {
            IMongoCollection<OrganizationModel> collection = _db.GetCollection<OrganizationModel>(_organizationCollection);
            FilterDefinition<OrganizationModel> filter = Builders<OrganizationModel>.Filter.Eq("Id", organizationId);

            BsonDocument let = new BsonDocument("ids", "$WorkerIds");

            BsonArray subPipeline = new BsonArray();

            subPipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                    "$in", new BsonArray { "$_id", "$$ids"} ))
                )
            );

            BsonDocument lookup = new BsonDocument("$lookup",
                             new BsonDocument("from", _userCollection)
                                         .Add("let", let)
                                         .Add("pipeline", subPipeline)
                                         .Add("as", "users")
            );

            BsonDocument group = new BsonDocument("$group", new BsonDocument("_id", "$users"));

            List<MongoAggregateOrgUserContainerModel> d = collection.Aggregate()
                .Match(filter)
                .AppendStage<BsonDocument>(lookup)
                .AppendStage<BsonDocument>(group)
                .Unwind("_id")
                .As<MongoAggregateOrgUserContainerModel>().ToList();
            Dictionary<Guid, UserModel> D = d.ToDictionary(u => u._id.Id, u => u._id);
            return D;
        }

        public UserModel GetUser(Guid id)
        {
            return LoadRecordById<UserModel>(_userCollection, id);
        }

        public UserModel GetUser(string emailAddress)
        {
            IMongoCollection<UserModel> collection = _db.GetCollection<UserModel>(_userCollection);
            FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Eq("EmailAddress", emailAddress);

           return collection.Find(filter).First();
        }

        public UserModel GetUser(string emailAddress, string password)
        {
            UserModel user = GetUser(emailAddress);

            PasswordHashModel passwordHash = new();
            passwordHash.FromDbString(user.PasswordHash);
            (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(password, passwordHash);

            if (user == null || IsPasswordCorrect == false)
            {
                return null;
            }
            return user;
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
            OrganizationModel org = LoadRecordById<OrganizationModel>(_organizationCollection, organizationId);
            if (model.ParentIdTreePath.Count > 0)
            {
                ProjectModel parent = org.GetProjectByIdTree(model.ParentIdTreePath);

                int i = parent.SubProjects.FindIndex(p => p.Id == model.Id);
                if (i < 0)
                { return; }

                parent.SubProjects[i] = model;
            } 
            else
            {
                org.Projects.Add(model);
            }
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void UpdateComment(CommentModel model, Guid organizationId)
        {
            OrganizationModel org = LoadRecordById<OrganizationModel>(_organizationCollection, organizationId);
            ProjectModel parent = org.GetProjectByIdTree(model.ParentProjectIdTreePath);

            int i = parent.Comments.FindIndex(c => DateTime.Equals(c.DatePosted, model.DatePosted)); // todo: decide if dateposted is a valid unique id for comments.
            if (i < 0)
            { return; }

            parent.Comments[i] = model;
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void UpdateAssignment(AssignmentModel model)
        {
            UserModel u = LoadRecordById<UserModel>(_userCollection, model.AssigneeId);
            int i = u.Assignments.FindIndex(a => a.OrganizationId == model.OrganizationId &&
                                  a.ProjectIdTreePath.Last() == model.ProjectIdTreePath.Last());
            if (i < 0)
            { return; }

            u.Assignments[i] = model;
            UpsertRecord(_userCollection, u.Id, u);
        }

        public void DeleteOrganization(OrganizationModel model)
        {
            DeleteRecord<OrganizationModel>(_organizationCollection, model.Id);
        }

        public void DeleteUser(UserModel model)
        {
            DeleteRecord<UserModel>(_userCollection, model.Id);
        }

        public void DeleteProject(ProjectModel model, Guid organizationId)
        {
            OrganizationModel org = LoadRecordById<OrganizationModel>(_organizationCollection, organizationId);
            if (model.ParentIdTreePath.Count > 0)
            {
                org.GetProjectByIdTree(model.ParentIdTreePath).SubProjects.Remove(model);
            }
            else
            {
                org.Projects.Remove(model);
            }
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void DeleteComment(CommentModel model, Guid organizationId)
        {
            OrganizationModel org = LoadRecordById<OrganizationModel>(_organizationCollection, organizationId);
            org.GetProjectByIdTree(model.ParentProjectIdTreePath).Comments.Remove(model);
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void DeleteAssignment(AssignmentModel model)
        {
            UserModel u = GetUser(model.AssigneeId);
            u.Assignments.Remove(model); // todo: return whether or not model was successfully removed?
            UpdateUser(u);
        }
    }
}
