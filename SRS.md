# Software Requirements Specification (SRS) for this app, *Dragonfly Project Management*

[//]: # (//TODO: Add license to repo)

## 1. Introduction

### 1.1 Purpose and Scope of The Product
This software is intended to assist individual users in tracking their work, time, and expenses. It is intended to be available and accessible to anyone who wants to use it.

### 1.2 Intended Audience and Use
This document is intended to be used by the developers of this software as a guide to implement the product defined by this document. It is also intended to be read by the users of this software to understand the functionality of this software.

### 1.3 Definitions and Acronyms
- User: A person who uses this software.
- Front End: The user interface of this software.
- Back End: The server-side code of this software, including the database and business logic.
- Task: A unit of work that a user desires to complete.
- Project: A collection of tasks and tasks that are related to each other.
- UI: **U**ser **I**nterface


## 2. Overall Description

### 2.1 User Needs
The users of this software need:
- To access the app through a web browser on a computer or mobile device.
- Their data saved securely for later use.
- Their accounts protected by a password and unique email address.
- To be able to edit or delete the data they add as they choose.
- A running list of projects and tasks they have marked as due within several days.
- To be able to organize their tasks into hierarchies. These hierarchies must not allow loops, but will be tree-like data structures.
- To mark each task as a specific priority, status, monetary expense, and due date.
- Diagrams displaying the analysis of individual projects and of all projects owned by a user. These analysis diagrams must generated by the software and show information regarding the total expense of a project, the percent of a project's tasks completed, and other statistics.
- A demo login mode for users to use the software without having to create an account.
- All links and buttons to be consistently and descriptively named.

### 2.2 Assumptions, Risks, and Dependencies
This document assumes that the underlying technologies used to implement the software will function as expected, and that the developer(s) of the software will use reliable technologies that do not limit the software to a specific platform.

The software will be dependent of the frameworks, libraries, and programming languages used to implement the software.


## 3. System Features and Requirements

### 3.1 Functional Requirements and Features
The front end will provide the following features:
- A landing page with options to create an account, login, or use the demo login.
- A login page with a form to enter a username and password.
- A page with a form to create a new account.
- Once a user is logged in, a page with inputs to update their account information.
- A user home page with a list of all the projects they have created.
- A page listing a user's pressing deadlines, including both projects and tasks.
- A page displaying analytics for all projects owned by a user.
- A page with a form to create a new project.
- When a user attempts to delete a project or task, confirmation modals or pages will be displayed to prevent accidental deletions.
- Forms to create tasks and update their details.
- When a user enters a project, a project home displaying project information and four tabs: backlog, board, notes, and analytics. The notes tab lets the user write text notes about the project, and the analytics tab shows diagrams of the project's statistics.

While the project backlog and board tabs differ in that the backlog tab displays tasks in a list, while the board tab displays tasks on a kanban board where users can drag and drop tasks to different columns, they both:
- Display a list of tasks for the user to view, edit, or open to view task details.
- Allow the user to add new tasks to the project.
- Let the user change the status of a task.
- Display the tasks with their subtasks in the logical tree nested within or under them.

The back end will provide the following features:
- A database that stores all the data for the app.
- Business logic that enforces security, prevents task hierarchy loops, and manages the application data in general.
- Cryptography that encrypts passwords and other sensitive data.

It is assumed that the hosting provider will provide the software administrator(s) interfaces to maintain the software.


### 3.2 External Interface Requirements
- The front end will be accessible through web browsers, and will interact with standard browser APIs.
- Administration of the back end (server and database) will be accessible through means provided by the hosting provider.
- The software will interface with server and client hardware through the underlying technologies used to implement the software.

### 3.3 Nonfunctional Requirements
- As a web app, this software must be compatible with all modern browsers with >1.25% market share. 
- The chosen color scheme must use contrasting colors such that the interface passes the devtools lighthouse audit.
- All buttons and links must be larger than or equal to 400px^2 (ex. 20px x 20px), regardless of screen size.
- Although application performance depends primarily on the user's device and hosting service, all algorithms used in the software must be implemented with best scalability.