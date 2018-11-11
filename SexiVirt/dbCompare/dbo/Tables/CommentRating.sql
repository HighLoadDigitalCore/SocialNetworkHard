CREATE TABLE [dbo].[CommentRating] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [UserID]    INT NOT NULL,
    [CommentID] INT NOT NULL,
    [Mark]      INT NOT NULL,
    CONSTRAINT [PK_CommentRating] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CommentRating_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_CommentRating_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

