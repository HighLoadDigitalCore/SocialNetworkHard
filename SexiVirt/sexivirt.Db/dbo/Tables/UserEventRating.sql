﻿CREATE TABLE [dbo].[UserEventRating]
(
	[ID] INT IDENTITY (1, 1)  NOT NULL,
	[UserID]       INT NOT NULL,
    [EventID]     INT NOT NULL,
    [Mark]           INT NOT NULL,
	CONSTRAINT [PK_UserEventRating] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserEventRating_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])  ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserEventRating_Event] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Event] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,

)
