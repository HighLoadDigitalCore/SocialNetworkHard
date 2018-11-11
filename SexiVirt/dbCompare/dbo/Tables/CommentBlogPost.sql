CREATE TABLE [dbo].[CommentBlogPost] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [CommentID]  INT NOT NULL,
    [BlogPostID] INT NOT NULL,
    CONSTRAINT [PK_CommentBlogPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CommentBlogPost_BlogPost] FOREIGN KEY ([BlogPostID]) REFERENCES [dbo].[BlogPost] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_CommentBlogPost_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

