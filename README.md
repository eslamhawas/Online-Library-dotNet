# 📚 Library Management System
A robust and secure Library Management System developed with .NET Core, SQL Server, JWT authentication, and React. The system allows for efficient management of books, patrons, and borrowing records, with a strong focus on security and user experience.

## 🖼️ Project Overview
The Library Management System provides a comprehensive solution for managing library operations. It includes features such as book catalog management, patron profiles, borrowing and returning of books, and detailed reporting, all wrapped in a secure and user-friendly web interface.


## ⚙️ Key Features
 **🔒** Secure Authentication & Authorization: JWT-based authentication and role-based access control to ensure secure access.
 
 **🗄️** Book & Patron Management: CRUD operations for books and patrons, with efficient searching and filtering capabilities.
 
 **🔑** Password Hashing & Encryption: Utilizes hashing algorithms like BCrypt for secure password storage.
 
 **📊** Reporting & Analytics: Detailed reports for books, patrons, and borrowing history.
 
 **🛠️** Modern Tech Stack: Built with .NET Core for the backend, SQL Server for the database, and React for the frontend.

## 🛠️ Tech Stack
Backend

.NET Core

Entity Framework Core

SQL Server

Security

JWT Authentication

Password Hashing: BCrypt

## 🚀 Getting Started
Prerequisites
.NET Core SDK
SQL Server
Node.js & npm
Docker
## Installation
### Clone the Repository:
```
git clone https://github.com/your-username/library-management-system.git
cd library-management-system
```
### Setup Backend (.NET Core API):
```
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

## Configuration
 Update the appsettings.json file in the backend project to configure the database connection string and JWT settings.
