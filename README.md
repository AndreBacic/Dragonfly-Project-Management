# Software Requirements Specification (SRS) for this app, *Dragonfly Project Management*

[//]: # (//TODO: Add license to repo)

## 1. Introduction

### 1.1 Purpose
The Bug Tracker helps teams conveniently manage their projects, deadlines, and general workflow. Users are grouped into 'Organizations' (AKA teams) where each team member has a role, assignments, and admins and managers may give permissions as needed. The app provides security measures to keep data private and keeps users' work safely backed up.

### 1.2 Intended Audience
This document is for any who wish to use or contribute to this application. Feel free to use the Bug Tracker to help your team!

### 1.3 Intended Use and Scope
This document is intended to be a reasonably comprehensive overview of the Bug Tracker. It does not elaborate on the myriad of smaller details that this app contains, but summarizes them.

### 1.4 Definitions and Acronyms
- User: A person who uses this app.
- Organization: A group of users in this app. Likely represents a company or team of employees.
- Project: A task, job, or some kind of work that needs to be completed by at least one user by a specified deadline.
- Comment: A chat message posted in a discussion board on a project.
- Role: A label given to a user in an organization that defines what they are allowed to do within that organization. Section 3.2 defines the different roles a User can have, and the following sections define what those permissions those roles give.
- AKA: Also Known As
- MVC: Model-View-Controller (a software design pattern)

[//]: # (//TODO: define assignments?)


## 2. Overall Description

### 2.1 User Needs
Users are people who need help keeping track of the many tasks and deadlines they have. Specifically, these users are assumed to be grouped together, like all of the employees in a company or one department of a company, or just an informal team of people who want to clearly divide up work and define goals for their work.

These people also want to comment on each other's work, and in a company environment certain users (like a manager or CEO) have roles marked with higher authority and granted higher permissions while lower-level employees need to be marked as such and granted only the app permissions they need.

### 2.2 App Features
The Bug Tracker lets its users:
- Create secure accounts
- Edit their account information
- Create organizations for grouping fellow users
- Be assigned to projects they are supposed to work on and finish
- Have roles that define what they may do to and in their organizations and assigned projects
- Leave comments and chat with each other within projects
- Edit or delete their comments

If they have permission, to:
- View organization and project information
- Invite other users to join their organization
- Edit an organization's name or description
- Change other users' roles
- Create projects within organizations
- Log work to these projects
- Assign users to these projects
- Have discussions about these projects by leaving comments
- Define or change a project's deadline, status, priority, name, or description
- Delete projects
- Delete other user's comments (ex. a manager removing an obscene comment)

Section 3 defines which roles receive these permissions and under which scope they get them.

The Bug Tracker also:
- Notifies users via email when they are invited (or added or assigned) to organizations or projects, when a deadline is soon, when one of their projects' status, deadline, or priority changes, or for other significant events.

### 2.3 Deployment Platform
This app will be deployed as web application with a responsive UI friendly to mobile and desktop screen sizes.


## 3. Description of Application Logic

### 3.1 Inviting Users to Join Organizations
As mentioned in 3.3, this privilege is restricted to the Admins of the organization.

Inviting a user into an organization is a 3-step process:
1) An admin of the organization searches by email or name for a user to invite.
2) The admin sends an invitation to the user, that includes what role the user will have in the organization (at least for now).
3) The user sees the invitation and can then accept it and join, or can decline and the admin will be notified that the user declined (or the user could just ignore it and then nothing is accomplished).

### 3.2 How Users Are Logically Linked to Organizations and Projects Within Those Organizations
Once a user has created an organization or accepted an invitation, they are linked to that organization, given access to it and a role in it.
Users can have one of these roles in an organization:
- Worker
- Manager
- Admin

Within that organization, users are assigned to work on projects and their roles determine what they may do in those projects.

### 3.3 Users' Permissions Given by Their Roles in an Organization
Workers may view the organization's about, statistics, team, and home pages. On the home page, users are shown only the projects they are assigned to. They may also remove themselves from the organization.

Managers have all permissions Workers have, but can view all projects in an organization on its home page.

Admins have all permissions Managers have, but may change the roles of other users in the organization, change the organization's name, description, and about, invite new users, remove users from the organization, create new projects, and delete these projects.

### 3.4 Users' Permissions Given by Their Roles in a Project
Workers may only view, comment on, and log work to projects they have been assigned to. They may remove themselves from projects they're assigned to, but of course can't assign themselves back to it once they've left.

Managers have all permissions Workers have, but can view and comment on all projects in the organization. In projects they have been assigned to, they may define or change a project's deadline, status, priority, name, or description, remove workers from the project, and delete comments left in the project by Workers (not by other managers or by admins).

Admins have all permissions Managers have, but these permissions apply across all projects in the organization. Admins can assign themselves or each other to projects, but doing so doesn't give them any additional permissions as they have read and write access to all properties of all projects. They have permission to delete projects, and can assign users to and remove users from any project in the organization. Admins *cannot* change each other's roles, remove each other from projects or the organization, or delete each other's comments, but they may demote themselves to lower roles.

### 3.5 Project Status and Priority Options
A project's status may be selected from:
- Todo
- In Progress
- Finished
- Abandoned

Once a user logs work to a project with a status of Todo, it's status is automatically updated to In Progress.
Users may log work to finished and abandoned projects.

And a project's priority may be selected from:
- Bottom
- Low
- Medium
- High
- Top

A project's priority is merely a label to tell users working on multiple projects which ones they should focus on.


## 4. Tech Stack and Implementation

### 4.1 Programming Languages and Frameworks
This full-stack app is built using C# .NET 5.0 MVC for handling both the front and back end, with MongoDB as a database and vanilla JavaScript and TailwindCSS for the front-end. DevOps tools (*as of 11/9/2021*) include git for version control, GitHub as a remote repository with a continuous integration pipeline, and the ability to deploy or share this app as a Docker container. Dependencies include the TailwindCSS framework, the C# MongoDB Driver, XUnit for C# unit testing, the built-in C# .NET Identity API for authentication and authorization, and the built-in C# .NET Dependency Injection library to implement the dependency inversion principle.

[//]: # (//TODO: mention emailing framework)

### 4.2 Codebase Structure
The codebase is divided into four projects (code projects, not the projects users use in the app):
- The C# MVC App that also contains the routing logic, a lot of business logic, database calls and all client-side code
- A data library containing classes for database access, data model definitions, hashing, and as much business logic as can be separated from the MVC project.
- A collection of unit tests
- A temporary console application for seeding the database and other tests that can't be put in the unit tests.

### 4.3 Front-end Routing
There are three logical sections to the front-end:
- The logged-out pages, where users can login or register (which logs them in)
- User account pages, where users are logged in but not into an organization, and can edit personal information, look at an overview of their assigned projects, app notifications, and organizations they are in charge of.
- Organization pages, where each user has selected an organization and now is logged in to it (as well as their personal accounts) with a specific role and list of assigned projects in the organization.

If a user has a session cookie where they are logged into an organization (authentication and authorization is handled with cookies), they are automatically directed to the organization home page.
If the user has a session cookie where they are logged into their personal account but not into an organization, they are directed to a page displaying a list of their app notifications and some of their account information.
If users without session cookies are directed to the login page.

### 4.4 Database Structure
(Has yet to be fully planned)

### 4.5 App Hosting Services
Has yet to be decided.


[//]: # (//TODO: Finish this SRS and make it more professional)