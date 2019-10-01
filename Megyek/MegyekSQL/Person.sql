CREATE TABLE [dbo].[Person]
(
	[Id] INT IDENTITY(1,1) NOT NULL
	,[UserName] nvarchar(256) NOT NULL
	,CONSTRAINT PK_Person PRIMARY KEY (Id)
)
