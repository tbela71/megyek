CREATE TABLE [dbo].[Participation]
(
	[PersonId] int NOT NULL 
	,[EventId] INT NOT NULL 
	,[Participate] BIT NOT NULL DEFAULT(1)
	,[Date] DATETIME NOT NULL
	,[ByPersonId] INT NOT NULL, 
    CONSTRAINT PK_Participation PRIMARY KEY ([PersonId],EventId)
	,CONSTRAINT FK_Participation_Person FOREIGN KEY ([PersonId]) REFERENCES [Person]([Id])
	,CONSTRAINT FK_Participation_Event FOREIGN KEY (EventId) REFERENCES [Event]([Id])
	,CONSTRAINT FK_Participation_ByPerson FOREIGN KEY ([ByPersonId]) REFERENCES [Person]([Id])
)
