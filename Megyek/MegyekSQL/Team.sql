CREATE TABLE [dbo].[Team]
(
	[Id] INT IDENTITY(1,1) NOT NULL
	,[Name] nvarchar(256)
	,[DefaultPlace] NVARCHAR(256) NULL, 
    CONSTRAINT PK_Team PRIMARY KEY (Id)
)
