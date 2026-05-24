# WebApiSocialMedia

A social media backend API built with **ASP.NET Core 8** and **Entity Framework Core** using **PostgreSQL**.

## Tech Stack

- **.NET 8** — ASP.NET Core Web API
- **Entity Framework Core 8** — ORM with PostgreSQL (Npgsql)
- **JWT Bearer** — Token-based authentication
- **BCrypt** — Password hashing
- **Swagger** — API documentation and testing

## Features

| Module               | Endpoints                                           | Auth   |
| -------------------- | --------------------------------------------------- | ------ |
| **Auth**             | Register, Login                                     | Public |
| **Users**            | List, Get, Create, Update, Delete                   | Mixed  |
| **Posts**            | List, Get, Get by Community, Create, Update, Delete | Mixed  |
| **Comments**         | List by Post, Create (with replies), Delete         | Mixed  |
| **Communities**      | List, Get, Create, Update, Delete                   | Mixed  |
| **CommunityMembers** | Join, Leave, List by Community, List by User        | Mixed  |
| **Votes**            | Upvote/Downvote/Remove, Get Vote Count              | Mixed  |
