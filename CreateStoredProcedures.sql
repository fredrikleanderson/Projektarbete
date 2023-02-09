CREATE OR ALTER PROCEDURE GetAllUsers
AS
SET NOCOUNT ON
SELECT Id, FirstName, LastName, Email FROM Users
GO

CREATE OR ALTER PROCEDURE GetUserPage @Id INT
AS
SET NOCOUNT ON
SELECT p.Id, p.UserId, p.Text, u.Id, u.FirstName, u.LastName, u.Email FROM Posts p
JOIN Users u ON p.UserId = u.Id
WHERE u.Id = @Id
GO

CREATE OR ALTER PROCEDURE GetAllPosts
AS
SET NOCOUNT ON
SELECT Posts.Id, Text, Users.Id, FirstName, LastName, Email FROM Posts
JOIN Users ON Posts.UserId = Users.Id
GO

CREATE OR ALTER PROCEDURE GetMostLikedPosts @Quantity INT
AS
SET NOCOUNT ON
SELECT TOP (@Quantity) 
	COUNT(l.Id) AS Likes,
	MAX(l.PostId) AS PostId,
	MAX(l.UserId) AS UserId,
	MAX(p.Id) AS Id, 
	MAX(p.Text) AS Text,
	MAX(p.UserId) AS UserId,
	MAX(u.Id) AS Id, 
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
SET NOCOUNT ON
UPDATE Users
SET FirstName = @NewFirstName, LastName = @NewLastName, Email = @NewEmail, Password = @NewPassword
WHERE Id = @Id
GO

CREATE OR ALTER PROCEDURE DeleteUserPosts @UserId INT
AS
SET NOCOUNT ON
DELETE FROM Posts WHERE UserId = @UserId
GO

CREATE OR ALTER PROCEDURE ClearAllTables
AS
SET NOCOUNT ON
DELETE FROM Likes
DELETE FROM Posts
DELETE FROM Users
GO