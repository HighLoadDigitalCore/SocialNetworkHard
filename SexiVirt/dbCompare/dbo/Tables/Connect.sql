CREATE TABLE [dbo].[Connect] (
    [ID]          INT IDENTITY (1, 1) NOT NULL,
    [UserID]      INT NOT NULL,
    [OtherUserID] INT NOT NULL,
    CONSTRAINT [PK_Connect] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Connect_OtherUser] FOREIGN KEY ([OtherUserID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Connect_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

