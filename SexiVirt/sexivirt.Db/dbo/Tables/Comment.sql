CREATE TABLE [dbo].[Comment]
(
	[ID] INT            IDENTITY (1, 1) NOT NULL,
	[ParentID]     INT          NULL,
	[UserID]     INT            NOT NULL,
	[Text] NVARCHAR(MAX) NOT NULL,
	[AddedDate] DATETIME NOT NULL,
	[Rating] INT NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Comment_Parent] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Comment] ([ID]),
	CONSTRAINT [FK_Comment_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
)
