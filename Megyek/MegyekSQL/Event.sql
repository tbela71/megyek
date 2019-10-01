CREATE TABLE [dbo].[Event]
(
	[Id] INT IDENTITY(1,1) NOT NULL 
	,[TeamId] INT NOT NULL
	,[Date] DateTime NOT NULL
	,[Description] nvarchar(256)
	,[EndDate] DATETIME NULL, 
    [Place] NVARCHAR(256) NULL, 
    [Special] BIT NULL DEFAULT 0, 
    [LastAlertSent] DATETIME NULL, 
    CONSTRAINT PK_Event PRIMARY KEY (Id)
	,CONSTRAINT FK_Event_Team FOREIGN KEY (TeamId) REFERENCES [Team]([Id])
)
