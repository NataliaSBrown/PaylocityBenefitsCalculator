-- Inserting relationship type data
INSERT INTO RelationshipTypes (Id, Type)
VALUES
(0, 'None'),
(1, 'Spouse'),
(2, 'DomesticPartner'),
(3, 'Child');

-- Inserting Employee data
INSERT INTO Employees (Id, FirstName, LastName, Salary, DateOfBirth)
VALUES
(1, 'LeBron', 'James', 75420.99, '1984-12-30'),
(2, 'Ja', 'Morant', 92365.22, '1999-08-10'),
(3, 'Michael', 'Jordan', 143211.12, '1963-02-17');

-- Inserting Dependent data
INSERT INTO Dependents (Id, FirstName, LastName, DateOfBirth, RelationshipTypeId, EmployeeId)
VALUES
(1, 'Spouse', 'Morant', '1998-03-03', 1, 2),   
(2, 'Child1', 'Morant', '2020-06-23', 3, 2),    
(3, 'Child2', 'Morant', '2021-05-18', 3, 2),  
(4, 'DP', 'Jordan', '1974-01-02', 2, 3);  