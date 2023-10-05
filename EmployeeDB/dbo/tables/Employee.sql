﻿CREATE TABLE [dbo].[Employee]
(
	Id INT PRIMARY KEY IDENTITY(1,1), 
    FirstName NVARCHAR(100) NULL, 
    LastName NVARCHAR(100) NULL,
    Salary DECIMAL(18, 2) NOT NULL, 
    DateOfBirth DATETIME NOT NULL
);
