CREATE TABLE [dbo].[AlbumAccess]
(
	[ID] INT            IDENTITY (1, 1) NOT NULL,
	[UserID]     INT            NOT NULL,
	[AlbumID]        INT            NOT NULL,
	[AddedDate] DATETIME NOT NULL,
	CONSTRAINT [PK_AlbumAccess] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AlbumAccess_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]),
	CONSTRAINT [FK_AlbumAccess_Album] FOREIGN KEY ([AlbumID]) REFERENCES [dbo].[Album] ([ID])  ON DELETE CASCADE ON UPDATE CASCADE,
)
