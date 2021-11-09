# Software Requirements Specification (SRS) for this app, the **Bug Tracker**

[//]: # (//TODO: Add license to repo)
[//]: # (//TODO: Rename The Bug Tracker/Bug Tracker to 'Dragonfly (Project Management))
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
- Sub-project: A project that is nested inside another one.
- Comment: A chat message posted in a discussion board on a project.
- Role: A label given to a user in an organization that defines what they are allowed to do within that organization. Section 3.2 defines the different roles a User can have, and the following sections define what those permissions those roles give.

[//]: # (//TODO: define assignments?)


## 2. Overall Description

### 2.1 User Needs
Users are people who need help keeping track of the many tasks and deadlines they have. Specifically, these users are assumed to be grouped together, like all of the employees in a company or one department of a company, or just an informal team of people who want to clearly divide up work and define goals for their work.

These people also want to comment on each other's work, and in a company environment certain users (like a manager or CEO) need to be marked with higher authority and granted higher permissions while lower-level employees need to be marked as such and granted only the app permissions they need.

### 2.2 App Features
The Bug Tracker lets its users:
- Create secure accounts
- Edit their account information
- Create organizations for grouping fellow users
- Be assigned to projects they are supposed to work on and finish
- Have permissions that define what they may do to and in their organizations and assigned projects
- Leave comments and chat with each other within projects
- Edit or delete their comments

If they have permission, to:
- View organization and project information
- Invite other users to join their organization
- Edit an organization's name or description
- Change other users' permissions
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


## 3 Description of Application Logic

### 3.1 Inviting Users to Join Organizations
As mentioned in 3.4, this privilege is restricted to the Admins of the organization.

Inviting a user into an organization is a 3-step process:
1) An admin of the organization searches by email or name for a user to invite.
2) The admin sends an invitation to the user, that includes what role the user will have in the organization (at least for now).
3) The user sees the invitation and can then accept it and join, or can decline and the admin will be notified that the user declined (or the user could just ignore it and then nothing is accomplished).


### 3.2 How Users Are Logically Linked to Organizations and Projects Within Those Organizations


### 3.3 Users' Permissions Given by Their Roles in an Organization


### 3.4 Users' Permissions Given by Their Roles in a Project

### 3.5 Relationships Between Projects and Sub-projects


## 4 Tech Stack


[//]: # (//TODO: Finish this SRS and make it more professional)