CREATE TABLE [dbo].[UserGift]
(
	[ID]		INT IDENTITY (1, 1) NOT NULL,
	[GiftID]	 INT NOT NULL,
	[SenderID]   INT            NOT NULL,
    [ReceiverID] INT            NOT NULL,
	[AddedDate] DATETIME NOT NULL,
	[Text] NVARCHAR (MAX) NULL,
	[Visible] BIT NOT NULL,
	CONSTRAINT [PK_UserGift] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_UserGift_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_UserGift_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
	CONSTRAINT [FK_UserGift_Gift] FOREIGN KEY ([GiftID]) REFERENCES [dbo].[Gift] ([ID])
)
