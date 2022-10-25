CREATE TABLE [dbo].[Site] (
    [Id]           INT        IDENTITY (1, 1) NOT NULL,
    [CampSiteName] NCHAR (25) NULL,
    [Longitude]    FLOAT (53) NULL,
    [Latitude]     FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
