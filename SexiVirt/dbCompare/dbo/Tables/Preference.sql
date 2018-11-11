CREATE TABLE [dbo].[Preference] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Preference] PRIMARY KEY CLUSTERED ([ID] ASC)
);

