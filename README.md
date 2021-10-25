# Software Requirements Specification (SRS) for app **Bug Tracker**

## 1. Introduction

#### 1.1 Purpose
The Bug Tracker helps teams conveniently manage their projects, deadlines, and general workflow. Users are grouped into 'Organizations' (AKA teams) where each team member has a role, assignments, and admins and managers may give permissions as needed. The app provides security measures to keep data private and keeps users' work safely backed up.

#### 1.2 Intended Audience
This document is for any who wish to use or contribute to this application. Feel free to use the Bug Tracker to help your team!

#### 1.3 Intended Use and Scope
This document is intended to be a reasonably comprehensive overview of the Bug Tracker. It does not elaborate on the myriad of smaller details that this app contains, but summarizes them.

#### 1.4 Definitions and Acronyms
- User: A person who uses this app.
- Organization: A group of users in this app. Likely represents a company or team of employees.
- Project: A task, job, or some kind of work that needs to be completed by at least one user by a specified deadline.
- Comment: A chat message posted in a discussion board on a project.

## 2. Overall Description

#### 2.1 User Needs
Users are people who need help keeping track of the many tasks and deadlines they have. Specifically, these users are assumed to be grouped together, like all of the employees in a company or one department of a company, or just an informal team of people who want to clearly divide up work and define goals for their work.

These people also want to comment on each other's work, and in a company environment certain users (like a manager or CEO) need to be marked with higher authority and granted higher permissions while lower-level employees need to be marked as such and granted only the app permissions they need.

#### 2.2 App Features
The Bug Tracker lets its users:
- Create secure accounts
- Edit their account information
- Create organizations for grouping fellow users
- Be assigned to projects they are supposed to work on and finish
- Have permissions that define what they may do to and in their organizations and assigned projects
- Be prevented from seeing, commenting on and editing projects that they do have permission to
- If they have permission, to change or delete organizations and projects
- To request to be added to an organization or project
- If they have permission, to invite other users to join their organization or project or accept those who have requested to join
If they have permission, to:
- Create projects within organizations
- Log work to these projects
- Have discussions about these projects by leaving comments
- To define/change a project's deadline
- To define/change a project's status and priority
- To define/change a project's name or description

The Bug Tracker also:
- Notifies users via email when they are invited/added/assigned to organizations or projects, when a deadline is soon, when one of their projects' status, deadline, or priority changes, or for other significant events.


#### 2.3 Assumptions and Dependencies

#### 2.4 Deployment Platform
This app will be deployed as web application with a responsive UI friendly to mobile and desktop screen sizes.

## 3. System Features and Requirements

#### 3.1 Functional Requirements

#### 3.2 External Interface Requirements

#### 3.3 System Features

#### 3.4 Nonfunctional Requirements