CREATE TABLE [dbo].[Compumat] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
    [Name]      NCHAR (25) NULL,
    [Longitude] FLOAT (53) NOT NULL,
    [Latitude]  FLOAT (53) NOT NULL,
    [Type]      INT        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
