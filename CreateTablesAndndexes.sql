--Use this to set up your Dapper Database

CREATE TABLE Users(
	Id int PRIMARY KEY identity(1,1),
	FirstName nvarchar(50),
	LastName nvarchar(50),
	Email nvarchar(50),
	Password nvarchar(50)
)
GO

CREATE TABLE Posts(
	Id int PRIMARY KEY identity(1,1),
	Text nvarchar(max),
	UserId int REFERENCES Users(Id)
)
GO

CREATE TABLE Likes(
	Id int PRIMARY KEY identity(1,1),
	PostId int REFERENCES Posts(Id),
	UserId int REFERENCES Users(Id),
)
GO

CREATE NONCLUSTERED INDEX IX_Posts_UserId
ON Posts(UserId)
GO

CREATE NONCLUSTERED INDEX IX_Likes_PostId
ON Likes(PostId)
GO

CREATE NONCLUSTERED INDEX IX_Likes_UserId
ON Likes(UserId)
GO