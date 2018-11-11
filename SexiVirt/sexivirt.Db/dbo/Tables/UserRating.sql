CREATE TABLE [dbo].[UserRating]
(
	[ID] INT IDENTITY (1, 1)  NOT NULL,
	[SenderID]       INT NOT NULL,
    [ReceiverID]     INT NOT NULL,
    [Mark]           INT NOT NULL,
	CONSTRAINT [PK_UserRating] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserRating_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_UserRating_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID]) 

)
