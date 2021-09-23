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

        public bool CreateAssignment(IAssignmentModel model)
        {
            IUserModel u = GetUser(model.AssigneeId);
            if (u.Assignments.Any(a => a.OrganizationId == model.OrganizationId &&
                                  a.ProjectIdTreePath.Last() == model.ProjectIdTreePath.Last()))
            { return false; }

            u.Assignments.Add(model);
            UpdateUser(u);
            return true;
        }

        public bool CreateOrganization(IOrganizationModel model)
        {
            List<MongoOrganizationModel> organizations = LoadRecords<MongoOrganizationModel>(_organizationCollection);

            // Ensure that no organizations with that name already exist
            if (organizations.Where(x => x.Name == model.Name).Any())
            { return false; }

            MongoOrganizationModel dbModel = ToOrgDbData(model);

            // No conflicts, so the record can be inserted.
            InsertRecord(_organizationCollection, dbModel);
            model.Id = dbModel.Id; // todo: see if casting model to dbModel actually didn't create a new object.
            return true;
        }

        private MongoOrganizationModel ToOrgDbData(IOrganizationModel model)
        {
            MongoOrganizationModel dbModel = (MongoOrganizationModel)model;

            dbModel.WorkerIds = dbModel.Workers.Select(u => u.Id).ToList();

            dbModel.Projects = dbModel.Projects.Select(p => ToProjectDbData(p)).ToList();
            return dbModel;
        }

        private MongoProjectModel ToProjectDbData(IProjectModel model)
        {
            return new MongoProjectModel()
            {
                Id = model.Id,
                Comments = model.Comments.Select(c => ToCommentDbData(c)).ToList(),
                WorkerIds = model.Workers.Select(u => u.Id).ToList(),
                SubProjects = model.SubProjects.Select(p => ToProjectDbData(p)).ToList(),
                Deadline = model.Deadline,
                Description = model.Description,
                Name = model.Name,
                ParentIdTreePath = model.ParentIdTreePath,
                Priority = model.Priority,
                Status = model.Status
            };
        }

        private MongoCommentModel ToCommentDbData(ICommentModel c)
        {
            return new MongoCommentModel
            {
                ParentProjectIdTreePath = c.ParentProjectIdTreePath,
                DatePosted = c.DatePosted,
                PosterId = c.Poster.Id,
                Text = c.Text
            };
        }

        public bool CreateUser(IUserModel model)
        {
            List<IUserModel> users = GetAllUsers();

            // do not insert record if email already used.
            if (users.Any(x => x.EmailAddress == model.EmailAddress))
            { return false; }

            // valid password; insert
            InsertRecord(_userCollection, model);
            return true;
        }

        public void CreateProject(IProjectModel model, Guid organizationId)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, organizationId);
            // this is ok because model.ParentIdTreePath is already filled out.
            org.GetDbProjectByIdTree(model.ParentIdTreePath).DbSubProjects.Add(ToProjectDbData(model));
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void CreateComment(ICommentModel model, Guid organizationId)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, organizationId);
            // this is ok because model.ParentProjectIdTreePath is already filled out.
            org.GetDbProjectByIdTree(model.ParentProjectIdTreePath).Comments.Add(ToCommentDbData(model));
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public IOrganizationModel GetOrganization(string organizationName, string password)
        {
            IMongoCollection<MongoOrganizationModel> collection = _db.
                GetCollection<MongoOrganizationModel>(_organizationCollection);
            FilterDefinition<MongoOrganizationModel> filter = Builders<MongoOrganizationModel>.Filter.Eq("Name", organizationName);

            MongoOrganizationModel org = collection.Find(filter).First();

            PasswordHashModel passwordHash = new PasswordHashModel();
            passwordHash.FromDbString(org.PasswordHash);
            (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(password, passwordHash);

            if (org == null || IsPasswordCorrect == false)
            {
                return null;
            }
            return GetOrgDbData(org);
        }

        public IOrganizationModel GetOrganization(Guid id)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, id);

            return GetOrgDbData(org);
        }

        /// <summary>
        /// All of the workers in org and all of its subproject's workers are found by id and added.
        /// </summary>
        /// <param name="org"></param>
        private IOrganizationModel GetOrgDbData(MongoOrganizationModel org)
        {
            Dictionary<Guid, IUserModel> users = GetAllUsers().ToDictionary(u => u.Id);

            foreach (Guid userId in org.WorkerIds)
            {
                if (users.ContainsKey(userId) == false)
                {
                    continue;
                }
                org.Workers.Add(users[userId]);
            }

            foreach (MongoProjectModel p in org.Projects)
            {
                org.Projects.Add(GetProjectDbData(p, users));
            }

            return org;
        }

        private IProjectModel GetProjectDbData(MongoProjectModel p, Dictionary<Guid, IUserModel> users)
        {
            foreach (Guid id in p.WorkerIds)
            {
                if (users.ContainsKey(id) == false)
                {
                    continue;
                }
                p.Workers.Add(users[id]);
            }
            foreach (MongoCommentModel c in p.Comments)
            {
                if (users.ContainsKey(c.PosterId) == true)
                {
                    c.Poster = users[c.PosterId];
                }
                p.AddComment(c);
            }
            foreach (MongoProjectModel sp in p.SubProjects)
            {
                p.AddSubProject(GetProjectDbData(sp, users));
            }

            return p;
        }

        public List<IUserModel> GetAllUsers()
        {
            return LoadRecords<MongoUserModel>(_userCollection).Select(u => (IUserModel)u).ToList();
        }

        public IUserModel GetUser(Guid id)
        {
            return LoadRecordById<MongoUserModel>(_userCollection, id);
        }

        public IUserModel GetUser(string emailAddress, string password)
        {
            IMongoCollection<MongoUserModel> collection = _db.GetCollection<MongoUserModel>(_userCollection);
            FilterDefinition<MongoUserModel> filter = Builders<MongoUserModel>.Filter.Eq("EmailAddress", emailAddress);

            MongoUserModel user = collection.Find(filter).First();

            PasswordHashModel passwordHash = new PasswordHashModel();
            passwordHash.FromDbString(user.PasswordHash);
            (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(password, passwordHash);

            if (user == null || IsPasswordCorrect == false)
            {
                return null;
            }
            return user;
        }

        public void UpdateOrganization(IOrganizationModel model)
        {
            UpsertRecord(_organizationCollection, model.Id, ToOrgDbData(model));
        }

        public void UpdateUser(IUserModel model)
        {
            UpsertRecord(_userCollection, model.Id, (MongoUserModel)model);
        }

        public void UpdateProject(IProjectModel model, Guid organizationId)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, organizationId);
            MongoProjectModel parent = org.GetProjectByIdTree(model.ParentIdTreePath);

            int i = parent.SubProjects.FindIndex(p => p.Id == model.Id);
            if (i < 0)
            { return; }

            parent.SubProjects[i] = ToProjectDbData(model);
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void UpdateComment(ICommentModel model, Guid organizationId)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, organizationId);
            MongoProjectModel parent = org.GetProjectByIdTree(model.ParentProjectIdTreePath);

            int i = parent.Comments.FindIndex(c => DateTime.Equals(c.DatePosted, model.DatePosted)); // todo: decide if dateposted is a valid unique id for comments.
            if (i < 0)
            { return; }

            parent.Comments[i] = ToCommentDbData(model);
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void UpdateAssignment(IAssignmentModel model)
        {
            MongoUserModel u = LoadRecordById<MongoUserModel>(_userCollection, model.AssigneeId);
            int i = u.Assignments.FindIndex(a => a.OrganizationId == model.OrganizationId &&
                                  a.ProjectIdTreePath.Last() == model.ProjectIdTreePath.Last());
            if (i < 0)
            { return; }

            u.Assignments[i] = model;
            UpsertRecord(_userCollection, u.Id, u);
        }

        public void DeleteOrganization(IOrganizationModel model)
        {
            DeleteRecord<MongoOrganizationModel>(_organizationCollection, model.Id);
        }

        public void DeleteUser(IUserModel model)
        {
            DeleteRecord<MongoUserModel>(_userCollection, model.Id);
        }

        public void DeleteProject(IProjectModel model, Guid organizationId)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, organizationId);
            org.GetProjectByIdTree(model.ParentIdTreePath).SubProjects.Remove(ToProjectDbData(model));
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void DeleteComment(ICommentModel model, Guid organizationId)
        {
            MongoOrganizationModel org = LoadRecordById<MongoOrganizationModel>(_organizationCollection, organizationId);
            org.GetProjectByIdTree(model.ParentProjectIdTreePath).Comments.Remove(ToCommentDbData(model));
            UpsertRecord(_organizationCollection, organizationId, org);
        }

        public void DeleteAssignment(IAssignmentModel model)
        {
            IUserModel u = GetUser(model.AssigneeId);
            u.Assignments.Remove(model); // todo: return whether or not model was successfully removed?
            UpdateUser(u);
        }
    }
}
