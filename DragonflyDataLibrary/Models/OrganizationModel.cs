﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonflyDataLibrary.Models
{
    /// <summary>
    /// An organization that serves as an umbrella for a specific group of users working on their projects.
    /// <br />
    /// NOTE: OrganizationModel.Name must be unique.
    /// <br />
    /// Also, users must be invited to join the organination (and then accept) to be allowed access, 
    /// or request to be allowed access and then approved. 
    /// this.WorkerIds are the ids of all of the users who are allowed access to this.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class OrganizationModel
    {
        [BsonId]
        public Guid Id { get; set; }
        /// <summary>
        /// Unique name of the organization
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// When this organization was created.
        /// </summary>
        public DateTime DateFounded { get; set; }
        /// <summary>
        /// Projects under the umbrella of this, not including these projects' sub-projects.
        /// </summary>
        public List<ProjectModel> Projects { get; set; } = new();
        /// <summary>
        /// The ids of the users invited and assigned to work on this including the admin.
        /// </summary>
        public List<Guid> WorkerIds { get; set; } = new();

        /// <summary>
        /// Short(small paragraph) description of what this organization is, in summary.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Large (several paragraphs) description of what this organization is, in detail.<br/>
        /// May be in markdown syntax that is rendered as html elements on the front end.
        /// </summary>
        public string About { get; set; }

        public ProjectModel GetProjectByIdTree(List<Guid> idTree)
        {
            if (idTree == null || idTree.Count == 0)
            {
                return null;
            }
            ProjectModel project = Projects.FirstOrDefault(p => p.Id == idTree[0]);
            for (int i = 1; i < idTree.Count; i++)
            {
                if (project == null)
                {
                    return null;
                }
                project = project.SubProjects.FirstOrDefault(p => p.Id == idTree[i]);
            }
            return project;
        }
    }
}