﻿CREATE TABLE [dbo].[UserStatus] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserStatus_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

