CREATE TABLE [dbo].[CommentEvent]
(
	[ID] INT            IDENTITY (1, 1) NOT NULL,
	[CommentID]     INT            NOT NULL,
	[EventID] INT            NOT NULL,
	CONSTRAINT [PK_CommentEvent] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_CommentEvent_Event] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Event] ([ID])  ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_CommentEvent_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID])  ON DELETE CASCADE ON UPDATE CASCADE,
)
