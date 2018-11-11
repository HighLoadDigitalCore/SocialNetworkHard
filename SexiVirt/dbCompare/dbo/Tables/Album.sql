CREATE TABLE [dbo].[Album] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [UserID]      INT            NOT NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [OrderBy]     INT            NOT NULL,
    [IsModerated] BIT            NOT NULL,
    [Price]       FLOAT (53)     NULL,
    CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Album_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

