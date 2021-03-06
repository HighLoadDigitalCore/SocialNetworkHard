﻿CREATE TABLE [dbo].[Feed] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          INT            NOT NULL,
    [ActionType]      INT            NOT NULL,
    [Text]            NVARCHAR (MAX) NOT NULL,
    [ActorID]         INT            NULL,
    [BlogPostID]      INT            NULL,
    [EventID]         INT            NULL,
    [GroupBlogPostID] INT            NULL,
    [AlbumAccessID]   INT            NULL,
    [AddedDate]       DATETIME       NOT NULL,
    [IsNew]           BIT            NOT NULL,
    [CommentID]       INT            NULL,
    CONSTRAINT [PK_Feed] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Feed_Actor] FOREIGN KEY ([ActorID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Feed_AlbumAccess] FOREIGN KEY ([AlbumAccessID]) REFERENCES [dbo].[AlbumAccess] ([ID]),
    CONSTRAINT [FK_Feed_BlogPost] FOREIGN KEY ([BlogPostID]) REFERENCES [dbo].[BlogPost] ([ID]),
    CONSTRAINT [FK_Feed_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]),
    CONSTRAINT [FK_Feed_Event] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Event] ([ID]),
    CONSTRAINT [FK_Feed_GroupBlogPost] FOREIGN KEY ([GroupBlogPostID]) REFERENCES [dbo].[GroupBlogPost] ([ID]),
    CONSTRAINT [FK_Feed_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

