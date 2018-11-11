CREATE TABLE [dbo].[Group] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [Info]      NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [Rating]    INT            NOT NULL,
    [IsBanned]  BIT            NOT NULL,
    [IsVip]     BIT            NOT NULL,
    [AvatarUrl] NVARCHAR (150) NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Group_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

