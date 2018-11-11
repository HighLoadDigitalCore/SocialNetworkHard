CREATE TABLE [dbo].[BlockGroupUser]
(
	[ID]             INT            IDENTITY (1, 1) NOT NULL,
    [UserID]         INT            NOT NULL,
	[GroupID]        INT           NOT NULL,
	CONSTRAINT [PK_BlockGroupUser] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BlockGroupUser_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_BlockGroupUser_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE

)
