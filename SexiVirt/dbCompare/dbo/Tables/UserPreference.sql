CREATE TABLE [dbo].[UserPreference] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserID]       INT NOT NULL,
    [PreferenceID] INT NOT NULL,
    CONSTRAINT [PK_UserPreference] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserPreference_Preference] FOREIGN KEY ([PreferenceID]) REFERENCES [dbo].[Preference] ([ID]),
    CONSTRAINT [FK_UserPreference_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

