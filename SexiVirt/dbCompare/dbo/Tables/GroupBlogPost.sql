CREATE TABLE [dbo].[GroupBlogPost] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [GroupID]   INT            NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [Header]    NVARCHAR (500) NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [Attach]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_GroupBlogPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GroupBlogPost_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_GroupBlogPost_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

