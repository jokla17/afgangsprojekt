CREATE TABLE [dbo].[Compumat] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
	[StationNo] NCHAR (25) NULL,
    [Name]      NCHAR (25) NULL,
    [Latitude]  FLOAT (53) NOT NULL,
    [Longitude] FLOAT (53) NOT NULL,
    [Type]      INT        NOT NULL,
    [Status]    NCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);