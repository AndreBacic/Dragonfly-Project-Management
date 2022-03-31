// Sprint plans for app Dragonfly Project Management, an app for users to track tasks and issues, and to collaborate as organizations.
// Each sprint is one week long and incrementally adds features
// UI should ultimately make it easy for users to work on each other's projects without removing needed privacy
// Note: Because MongoDB is used, some desired features may be mutually exclusive :(

// Each sprint object here adds new features to the app, and the finished app can be understood by combing all sprints together

// Sprint 0: Minimum functionality and initial setup (users can only see their own projects and information)
const DragonflyProjectManagement0 = {
    config: [
        ".NET MVC app template",
        "Cookie based authentication",
        "Tailwind CSS",
        "PostCSS for TailwindCSS",
        "MongoDB Database (user and project collections) and connection information",
        "Password hashing",
        "Choose color and font schemes",
        "Create Dragonfly Project Management logo icon"
    ],
    pages: [
        "Landing page",
        "Login",
        "Register",
        "User home (displays user information and project list)",
        "Create project"
    ],
    models: { // Guid ids are assumed for each model
        "User": {
            "username": "string",
            "password": "string",
            "dateJoined": "datetime",
            "projects": "array[Guid]",
        },
        "Project": {
            "name": "string",
            "description": "string",
            "dateCreated": "datetime",
        }
    },
    controllerActions: [
        "Register => Creates a new user in the database (after ensuring that the username is unique)",
        "Create project => Creates a new project in the database",
        "User home => Gets user data and user's projects from the database",
        "Login => Gets user data from the database and logs in the user with a session cookie",
        "Logout => Logs out the user and clears the session cookie"
    ]
}

// Sprint 1: Update and delete project or user (users can still only see their own tasks and information, but can now change it)
const DragonflyProjectManagement1 = {
    config: [ // yay no changes
    ],
    pages: [
        "Edit project",
        "Edit user account",
        "Delete account (says something like 'Are you sure you want to delete your account?')",
    ],
    models: {
        "Project": {
            "status": "boolean", // true if project is active, false if project is archived/completed
        },
    },
    controllerActions: [
        "Edit project => Updates the project in the database",
        "Edit user account => Updates the user in the database",
        "Delete account => Deletes the user from the database",
        "Toggle project status (on user home page) => Changes the status of the project in the database"
    ]
}

// Sprint 2: Basic user collaboration (users can now see other users' projects and information)
const DragonflyProjectManagement2 = {
    config: [
        "User role-based authorization",
        "Throw in serilog for logging because why not",
    ],
    pages: [
        "Search for user (displays all users that match inputted username)",
        "Other user home (displays user information and project list, but denies editing or deleting)",
    ],
    models: {}, // yay no changes
    controllerActions: [
        "Search for user => Searches for a user in the database",
        "Other user home => Gets user data and user's projects from the database"
    ]
}

// Sprint 3: Add tasks (users can now create sub-projects called tasks)
const DragonflyProjectManagement3 = {
    config: [ // yay no changes
    ],
    pages: [
        "User home (displays user information and project list, showing how many tasks are in each project)",
        "Project home (displays project information and task list, and allows for the CRUD of tasks)",
    ],
    models: {
        "Task": {
            "description": "string",
            "status": "boolean", // true if task is active, false if task is completed
        },
        "Project": {
            "tasks": "array[Guid]", // array of task ids
            "status": "enum[todo, active, completed, archived]" // this now needs to provide more information than just a boolean
        }
    },
    controllerActions: [
        "Create task => Creates a new task in the database",
        "Edit task => Updates the task in the database",
        "Delete task => Deletes the task from the database",
        "Toggle task status (on project home page) => Changes the status of the task in the database"
    ]
}

// Sprint 4: Add comments (users can now add comments to tasks and projects)
// Note: the owner of a project will have permission to delete any comments on that project
const DragonflyProjectManagement4 = {
    config: [ 
        "MongoDB: new collection for comments",
        "MongoDB: aggregation pipelines to get all comments on a project or task or all of a user's comments",
    ],
    pages: [
        "Project home (now shows comments and lets users add comments or edit/delete their own comments)",
        "Task home (lets owner edit task and shows comments and lets users add comments or edit/delete their own comments)",
    ],
    models: {
        "Comment": {
            "content": "string",
            "datePosted": "datetime",
        },
        // So, tasks and projects need access to all nested comments, 
        // but users might also need access to all of their comments 
        // so comments have to be in a separate collection
        "Task": {
            "comments": "array[Guid]", // array of comment ids
        },
        "Project": {
            "comments": "array[Guid]", // array of comment ids
        },
        "User": {
            "comments": "array[Guid]", // array of comment ids
        }
        
    },
    controllerActions: [
        "Create comment => Creates a new comment in the database",
        "Edit comment => Updates the comment in the database",
        "Delete comment => Deletes the comment from the database",
        "Toggle project status (on project home page) => Now offers several options for the status of the project in the database",
    ]
}

// Sprint 5: Owner-grated editing (project or task owners can now allow other users to edit or delete their project or task)
// Note: the project or task owner always can strip other users of these permissions
// Note: I use the words roles and permissions interchangeably everywhere in this file
// Note: project owner are automatically made tasks owners for tasks in their projects
const DragonflyProjectManagement5 = {
    config: [
        "Add new user roles",
        "MongoDB: new collection for permissions",
        "MongoDB: aggregation pipelines to get all permissions on a project or task or all of a user's permissions",
    ],
    pages: [
        "Invite user (displays all users that match inputted username and allows owner to set initial permissions)",
        "Manage permissions (allows owner to set permissions)",
        "Project home (now shows all users working on it and their permissions)",
    ],
    models: {  // I feel bad for using MongoDB like SQL, but I want to have a project that demos my MongoDB skills.
        "UserPermission": {
            "user": "Guid",
            "project": "Guid", // can be project or task
            "permission": "enum[owner, deletor, editor, banned]", // Anyone may view or comment on projects and tasks unless they are banned
        },
        // since UserPermission references a user and a project/task, no other models need to be updated :)
    },
    controllerActions: [
        "Invite user => Creates a new permission in the database",
        "Manage permissions => Updates the permission in the database or deletes it if the owner kicks the user off the project without banning them",
        "User home => Now shows not only the user's projects and tasks, but also those the user has permissions to edit/delete",
        "Project/task home => Now also gets all users working on the project/task and their permissions"
    ]
}

// Sprint 6: Add project/task tags and priorities
// Note: users granted edit permissions can edits all fields, including tags and priorities
const DragonflyProjectManagement6 = {
    config: [
        "Have a list of standard tags available to all users",
    ],
    pages: [
        "Project/task home (now shows tags and priority)",
    ],
    models: {
        "Project": {
            "tags": "array[string]",
            "priority": "enum[bottom, low, medium, high, top]", // not using integers because I'm never sure if 1 means top or bottom
        },
        "Task": {
            "tags": "array[string]",
            "priority": "enum[bottom, low, medium, high, top]", // same deal as with projects
        }
    },
    controllerActions: [
        "Create/edit project/task => Now tags and priorities are in the database",
    ]
}

// Sprint 7-8: Group users into organizations (now turning the ad-hoc groups into formal, company-like organizations)
// Note: organizations are basically glorified wrappers for projects, except that users can search for organizations to try to join them.
// Note: only users accepted into an organization can see the projects/tasks in that organization and leave comments on them
// Note: organization owners are automatically made project owners for projects in their organizations
const DragonflyProjectManagement7and8 = {
    config: [
        "MongoDB: new collection for organizations",
        "MongoDB: aggregation pipelines to get all organizations a user is a member of and all users in an organization",
        "Restrict users not see projects nested in organizations when they are not a member of the organization",
    ],
    pages: [
        "User home (now shows organizations)",
        "Organization home (now shows projects and accepted users)",
        "Create organization",
        "Join organization (displays all organizations that match inputted name and allows user to apply to join)",
        "Organization appliers (displays all users that have applied to join an organization and is visible only to the owner of the organization)",
        "Manage organization (allows owner to set permissions and edit/delete the organization)",
    ],
    models: {
        "Organization": {
            "projects": "array[Guid]",
            "members": "array[Guid]",
            "dateCreated": "datetime",
            "title": "string",
            "description": "string",
            "tags": "array[string]",
            "owner": "Guid", // owner has a UserPermission that references the organization but the organization needs a reference to the owner
            "applyingMembers": "array[Guid]", // array of ids of user that are applying to join the organization
        },
    },
    controllerActions: [
        "Create organization => Creates a new organization in the database",
        "Join organization => Creates a new permission in the database",
        "Organization appliers => Gets all users that have applied to join an organization",
        "User home => Now gets all organizations the user is a member of",
        "Edit organization => Updates the organization in the database",
        "Manage organization => Updates UserPermission in the database or deletes it if the owner kicks the user off the organization",
        "Delete organization => Deletes the organization from the database and deletes all permissions, projects, tasks and comments associated with it (but first prompts the owner to confirm)",
    ]
}

// Sprint 9: Add attachments to projects and tasks because that seems like a good idea
// Note: users granted edit permissions can edits all fields, including attachments
const DragonflyProjectManagement9 = {
    config: [
        "MongoDB: new collection for attachments",
        "MongoDB: aggregation pipelines to get all attachments on a project or task",
    ],
    pages: [
        "Project/task home (now shows attachments)",
    ],
    models: {
        "Attachment": {
            "fileName": "string",
            "fileType": "string",
            "fileSize": "int",
            "fileData": "string",
        },
        "Project": {
            "attachments": "array[Guid]",
        },
        "Task": {
            "attachments": "array[Guid]",
        }
    },
    controllerActions: [
        "Create/edit project/task => Now attachments are in the database",
    ]
}


// OK BIG FANALIE: AGGREGATE ALL SPRINTS INTO ONE OBJECT:
// Where a page or controller action is mentioned in multiple sprints, it is only mentioned once in the sprints object but lists all functionality gained from all sprints
// All config strings are listed, and sorted from front-end to back-end (so, CSS stuff comes first, followed by roles policies, and MongoDB collections and pipelines are last)
// Where a model is mentioned in multiple sprints, it is only mentioned once in the sprints object but has all fields from all sprints, except where a field in changed by a later sprint, where the most recent value is used
const DragonflyProjectManagement = {
    config: [
        "Tailwind CSS",
        "PostCSS",
        "Color and font schemes",
        "Dragonfly logo icon",
        "Cookie-based authentication",
        "User role-based authorization",
        "Restrict users not see projects or tasks nested in organizations when they are not a member of the organization",
        "Restrict banned users from seeing projects or tasks they are banned from",
        "Password hashing",
        "MongoDB database",
        "MongoDB collections: users, projects, tasks, comments, permissions, organizations, attachments",
        "MongoDB aggregation pipelines: get all comments in a project/task, get all users in an organization," +
            "get all projects in an organization, get all tasks in a project, get all projects a user is a member of," +
            "get all tasks a user is a member of, get all users applying to join an organization, get all organizations a user is a member of," +
            "get all projects a user is an owner of, get all tasks a user is an owner of, get all attachments on a project or task",
    ],
    pages: [
        "User home (shows all projects and tasks the user is working on, and all organizations the user is a member of)",
        "Edit user account (allows user to edit their username, email, and password)",
        "Delete account (says 'Are you sure?' and then deletes the user from the database)",
        "Login",
        "Register",
        "Project home (shows all users working on the project, and all tasks in the project)",
        "Task home (shows all users working on the task, and all comments on the task)",
        "Create project",
        "Create task",
        "Organization home (shows all projects and users in the organization)",
        "Invite user (displays all users that match inputted username and allows owner to set initial permissions)",
        "Manage permissions (allows owner to set permissions)",
        "Organization appliers (displays all users that have applied to join an organization and is visible only to the owner of the organization)",
        "Manage organization (allows owner to set permissions and edit/delete the organization)",
        "Join organization (displays all organizations that match inputted name and allows user to apply to join)",
        "Create organization",
        "Create/edit project/task (displays all tags and priorities)",
        "Project/task home (shows all users in the project/task, their comments, tags and priorities, and all attachments)",
        
    ],
    models: { // Guid ids are assumed for each model ("id": "Guid",)
        "User": {
            "firstName": "string",
            "lastName": "string",
            "email": "string",
            "password": "string",
            "dateCreated": "datetime",
            "projects": "array[Guid]",
            "tasks": "array[Guid]",
            "organizations": "array[Guid]",
            "permissions": "array[Guid]",
        },
        "Project": {
            "title": "string",
            "description": "string",
            "tags": "array[string]",
            "owner": "Guid",
            "members": "array[Guid]",
            "dateCreated": "datetime",
            "status": "enum[bottom, low, medium, high, top]",
            "tasks": "array[Guid]",
            "attachments": "array[Guid]",
            "comments": "array[Guid]",
        },
        "Task": {
            "title": "string",
            "description": "string",
            "tags": "array[string]",
            "owner": "Guid",
            "members": "array[Guid]",
            "dateCreated": "datetime",
            "status": "enum[bottom, low, medium, high, top]",
            "attachments": "array[Guid]",
            "comments": "array[Guid]",
        },
        "Comment": {
            "owner": "Guid",
            "dateCreated": "datetime",
            "text": "string",
        },
        "UserPermission": {
            "user": "Guid",
            "project": "Guid",
            "permission": "enum[owner, deletor, editor, banned]",
        },
        "Organization": {
            "projects": "array[Guid]",
            "members": "array[Guid]",
            "dateCreated": "datetime",
            "title": "string",
            "description": "string",
            "tags": "array[string]",
            "owner": "Guid", // owner has a UserPermission that references the organization but the organization needs a reference to the owner
            "applyingMembers": "array[Guid]", // array of ids of user that are applying to join the organization
        },
        "Attachment": {
            "fileName": "string",
            "fileType": "string",
            "fileSize": "int",
            "fileData": "string",
        },
    },
    controllerActions: [
        "Create user => Creates a new user in the database",
        "Create project => Creates a new project in the database",
        "Create task => Creates a new task in the database",
        "Create comment => Creates a new comment in the database",
        "Create organization => Creates a new organization in the database",
        "Create attachment => Creates a new attachment in the database",
        "Create user permission => Creates a new user permission in the database",
        "Edit user => Updates the user in the database",
        "Edit project => Updates the project in the database",
        "Edit task => Updates the task in the database",
        "Edit comment => Updates the comment in the database",
        "Edit organization => Updates the organization in the database",
        "Edit attachment => Updates the attachment in the database",
        "Edit user permission => Updates the user permission in the database",
        "Delete user => Deletes the user from the database",
        "Delete project => Deletes the project from the database",
        "Delete task => Deletes the task from the database",
        "Delete comment => Deletes the comment from the database",
        "Delete organization => Deletes the organization from the database",
        "Delete attachment => Deletes the attachment from the database",
        "Delete user permission => Deletes the user permission from the database",
        "Join organization => Creates a new permission in the database",
        "User home => Now gets all organizations the user is a member of",
        "Project home => Now gets all tasks in the project",
        "Task home => Now gets all comments on the task",
        "Edit project/task => Now attachments are in the database",
        "Manage organization => Updates UserPermission in the database or deletes it if the owner kicks the user off the organization",
        "Delete organization => Deletes the organization from the database and deletes all permissions, projects, tasks and comments associated with it (but first prompts the owner to confirm)",
    ]
}
