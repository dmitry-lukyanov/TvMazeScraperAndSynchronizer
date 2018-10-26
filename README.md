# TvMazeScraper

Db scripts:

Cast:
USE [TvMazeScraper]
GO

==============================

CREATE TABLE [dbo].[Casts] (

    [Id]       INT            NOT NULL,
    [Name]     NVARCHAR (250) NULL,
    [Birthday] DATE           NULL,
    [ShowId]   INT            NOT NULL
    
);

Primary key: Id + ShowId
==============================

CREATE TABLE [dbo].[Shows] (

    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (250) NULL
    
);

Primary key: Id
==============================

CREATE TABLE [dbo].[PagesProcessingStatuses] (

    [Id]               INT            NOT NULL,
    [AttemptToProcess] INT            NOT NULL,
    [LastError]        NVARCHAR (MAX) NULL,
    [Status]           INT            NOT NULL
);

Primary key: Id
==============================

CREATE TABLE [dbo].[SynchronizerState] (

    [LastUpdatedDate] DATETIME NOT NULL,
    [StateId]         INT      NOT NULL
);

Primary key: StateId
