CREATE TABLE [dbo].[MoneyDetail] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [UserID]      INT           NULL,
    [Type]        INT           NOT NULL,
    [Description] NVARCHAR (500) NOT NULL,
    [Sum]         FLOAT (53)    NOT NULL,
    [AddedDate]   DATETIME      NOT NULL,
	[IsFee]		  BIT           NOT NULL,
	[Global]      UNIQUEIDENTIFIER NOT NULL,
	[Submited]    BIT           NOT NULL,
    CONSTRAINT [PK_MoneyDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MoneyDetail_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

