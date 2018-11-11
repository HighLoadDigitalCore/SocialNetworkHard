CREATE TABLE [dbo].[Meeting] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [UserID]      INT            NOT NULL,
    [CityID]      INT            NOT NULL,
    [MeetingDate] DATETIME       NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Meeting_City] FOREIGN KEY ([CityID]) REFERENCES [dbo].[City] ([ID]),
    CONSTRAINT [FK_Meeting_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

