CREATE OR ALTER PROCEDURE GetAllUsers
AS
SELECT Id, FirstName, LastName, Email FROM Users
GO

CREATE OR ALTER PROCEDURE GetUserById @Id INT
AS
SELECT Id, FirstName, LastName, Email FROM Users
WHERE Id = @Id
GO

CREATE OR ALTER PROCEDURE GetAllPosts
AS
SELECT Posts.Id, Text, Users.Id, FirstName, LastName, Email FROM Posts
JOIN Users ON Posts.UserId = Users.Id
GO

CREATE OR ALTER PROCEDURE GetMostLikedPosts @Quantity INT
AS
SELECT TOP (@Quantity) COUNT(l.Id) AS Likes, 
	MAX(p.Id) AS PostId, 
	MAX(p.Text) AS Text, 
	MAX(u.Id) AS UserId, 
	MAX(u.FirstName) AS FirstName, 
	MAX(u.LastName) AS LastName, 
	MAX(u.Email) AS Email 
FROM Likes l
JOIN Posts p ON l.PostId = p.Id
JOIN Users u ON p.UserId = u.Id
GROUP BY l.PostId
ORDER BY COUNT(l.Id) DESC
GO

CREATE OR ALTER PROCEDURE PutUser @Id INT, @NewFirstName NVARCHAR(50), @NewLastName NVARCHAR(50), @NewEmail NVARCHAR(50), @NewPassword NVARCHAR(50)
AS
UPDATE Users
SET FirstName = @NewFirstName, LastName = @NewLastName, Email = @NewEmail, Password = @NewPassword
WHERE Id = @Id
GO

CREATE OR ALTER PROCEDURE ClearAllTables
AS
DELETE FROM Likes
DELETE FROM Posts
DELETE FROM Users
GO