CREATE TABLE [dbo].[ChangePassword] (
    [ID]        INT              IDENTITY (1, 1) NOT NULL,
    [UserID]    INT              NOT NULL,
    [Code]      UNIQUEIDENTIFIER NOT NULL,
    [AddedDate] DATETIME         NOT NULL,
    [Email]     NVARCHAR (500)   NOT NULL,
    [IsUsed]    BIT              NOT NULL,
    CONSTRAINT [PK_ChangePassword] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChangePassword_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

