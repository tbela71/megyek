CREATE TABLE [dbo].[Membership]
(
	[PersonId] int NOT NULL 
	,[TeamId] INT NOT NULL
	,[DisplayName] nvarchar(256) NOT NULL
	,[Manager] BIT NOT NULL DEFAULT(0)
	,[Mail] BIT NOT NULL DEFAULT (0), 
    CONSTRAINT PK_Membership PRIMARY KEY ([PersonId],[TeamId])
	,CONSTRAINT FK_Membership_Person FOREIGN KEY ([PersonId]) REFERENCES [Person]([Id])
	,CONSTRAINT FK_Membership_Team FOREIGN KEY (TeamId) REFERENCES [Team]([Id])
)
