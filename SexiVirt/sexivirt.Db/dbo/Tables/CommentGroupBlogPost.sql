CREATE TABLE [dbo].[CommentGroupBlogPost]
(
	[ID] INT            IDENTITY (1, 1) NOT NULL,
	[CommentID]     INT            NOT NULL,
	[GroupBlogPostID] INT            NOT NULL,
	CONSTRAINT [PK_CommentGroupBlogPost] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_CommentGroupBlogPost_GroupBlogPost] FOREIGN KEY ([GroupBlogPostID]) REFERENCES [dbo].[GroupBlogPost] ([ID])  ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_CommentGroupBlogPost_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID])  ON DELETE CASCADE ON UPDATE CASCADE,
)
