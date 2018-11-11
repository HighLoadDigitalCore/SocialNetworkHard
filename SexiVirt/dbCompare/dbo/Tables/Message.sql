CREATE TABLE [dbo].[Message] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [SenderID]   INT            NOT NULL,
    [ReceiverID] INT            NOT NULL,
    [ConnectID]  INT            NOT NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    [IsSend]     BIT            NOT NULL,
    [ReadedDate] DATETIME       NULL,
    [IsDeleted]  BIT            NOT NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Message_Connect] FOREIGN KEY ([ConnectID]) REFERENCES [dbo].[Connect] ([ID]),
    CONSTRAINT [FK_Message_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Message_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID])
);

