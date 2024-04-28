CREATE DATABASE OnlineLibrary
 CREATE TABLE dbo.Books (
 ISBN varchar(50) PRIMARY KEY NOT NULL,
 Title varchar(50) NOT NUll,
 Category varchar(50)  ,
 RackNumber  varchar(50)  ,
 Price int NOT NULL ,
 StockNumber int )

CREATE TABLE dbo.Users(
 ID int IDENTITY (1,1) PRIMARY KEY,
 UserName varchar(50) not null ,
 DateOfBirth DATE ,
 Email varchar(150) not null,
 IsAdmin BIT null ,
 IsAccepted BIT null,
 PasswordHash varbinary(1000),
 PassordSalt varbinary(1000),
 EncryptionKey nvarchar(50),
IVKey nvarchar(50))

 
 CREATE TABLE dbo.BorrowedBooks(
 DateOfReturn DATE ,
 OrderNumber int,
 IsAccepted BIT,
 BookISBN varchar(50),
 UserID INT,
 FOREIGN KEY (BookISBN) REFERENCES Books(ISBN),
  FOREIGN KEY (UserID) REFERENCES Users(ID),
 )


