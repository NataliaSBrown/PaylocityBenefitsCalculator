CREATE TABLE Dependent (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    DateOfBirth DATETIME NOT NULL,
    RelationshipTypeId INT FOREIGN KEY REFERENCES RelationshipType(Id),
    EmployeeId INT FOREIGN KEY REFERENCES Employee(Id)
);

GO;

CREATE UNIQUE INDEX UX_OneSpouseOrDomesticPartnerPerEmployee 
ON Dependent(EmployeeId) 
WHERE RelationshipTypeId IN (1, 2);
