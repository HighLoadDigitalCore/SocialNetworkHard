CREATE TABLE [dbo].[Gift] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Image]    NVARCHAR (150) NOT NULL,
    [Type]     INT            NOT NULL,
    [Price]    FLOAT (53)     NOT NULL,
    [IsActive] BIT            NOT NULL,
    CONSTRAINT [PK_Gift] PRIMARY KEY CLUSTERED ([ID] ASC)
);

