CREATE TABLE [dbo].[Event]
(
	[ID] INT            IDENTITY (1, 1) NOT NULL,
	[UserID]     INT            NOT NULL,
	[Name] NVARCHAR(500) NOT NULL, 
	[Description] NVARCHAR(MAX) NOT NULL, 
	[ImagePath]  NVARCHAR(150) NOT NULL, 
    [EventDate] DATETIME NOT NULL, 
    [CityID] INT NULL, 
    [Place] NVARCHAR(500) NOT NULL, 
    [Coordinate] NVARCHAR(50) NOT NULL, 
	Rating INT NOT NULL, 
	CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Event_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]),
	CONSTRAINT [FK_Event_City] FOREIGN KEY ([CityID]) REFERENCES [dbo].[City] ([ID]),

)
