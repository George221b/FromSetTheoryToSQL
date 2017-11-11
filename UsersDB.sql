/*============================================================================
	File:		UsersDB.sql

	Summary:	Scripts's purpose is to create two tables used for showing some basic
				SQL join queries.

	Date:		November 7th 2017

	SQL Server Version: 2008 / 2012 / 2014 / 2016
============================================================================*/
USE [master]
GO

CREATE DATABASE [UsersDB]
GO

USE [UsersDB]
GO

CREATE TABLE Countries (
Id INT PRIMARY KEY IDENTITY,
Name NVARCHAR(50) UNIQUE
)
GO

CREATE TABLE Users (
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(25),
LastName NVARCHAR(25),
CountryId INT
)
GO

-- Table: Countries
SET IDENTITY_INSERT Countries ON

INSERT INTO Countries (Id, Name) VALUES
(1,'Austria'),
(2,'Bulgaria'),
(3,'Germany'),
(4,'Indonesia'),
(5,'China'),
(6,'New Zeland')

SET IDENTITY_INSERT Countries OFF
GO

-- Table: Customers
SET IDENTITY_INSERT Users ON

INSERT INTO Users (Id, FirstName, LastName, CountryId) VALUES
(1,'Betty','Wallace',1),
(2,'Rachel','Bishop',2),
(3,'Joan','Peters',3),
(4,'Jean','Pierce',NULL),
(5,'Irene','Peters',NULL),
(6,'Quentin', 'Tarantino', NULL)

SET IDENTITY_INSERT Users OFF
GO

ALTER TABLE Users
ADD CONSTRAINT FK_Users_Country
FOREIGN KEY(CountryId)
REFERENCES Users(Id)
GO