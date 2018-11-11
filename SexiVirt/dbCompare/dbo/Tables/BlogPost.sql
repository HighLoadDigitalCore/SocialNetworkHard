﻿CREATE TABLE [dbo].[BlogPost] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [Header]    NVARCHAR (500) NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    [Attach]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BlogPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BlogPost_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);
