CREATE TABLE [dbo].[DepositCandidate] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [UserID]      INT            NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Sum]         FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_DepositCandidate] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepositCandidate_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

