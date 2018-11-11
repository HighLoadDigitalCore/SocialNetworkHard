CREATE TABLE [dbo].[Photo] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [AlbumID]  INT            NOT NULL,
    [FilePath] NVARCHAR (150) NOT NULL,
    [IsCover]  BIT            NOT NULL,
    CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Photo_Album] FOREIGN KEY ([AlbumID]) REFERENCES [dbo].[Album] ([ID])
);

