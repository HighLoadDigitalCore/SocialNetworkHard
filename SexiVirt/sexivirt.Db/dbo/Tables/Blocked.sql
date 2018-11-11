CREATE TABLE [dbo].[Blocked]
(
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [SenderID]   INT NOT NULL,
    [ReceiverID] INT NOT NULL,
    CONSTRAINT [PK_Blocked] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Blocked_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Blocked_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID])
);
