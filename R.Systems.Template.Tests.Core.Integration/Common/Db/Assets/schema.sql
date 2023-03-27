IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [company] (
    [id] int NOT NULL IDENTITY(3, 1),
    [name] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_company] PRIMARY KEY ([id])
);
GO

CREATE TABLE [employee] (
    [id] int NOT NULL IDENTITY(4, 1),
    [first_name] nvarchar(100) NOT NULL,
    [last_name] nvarchar(100) NOT NULL,
    [company_id] int NULL,
    CONSTRAINT [PK_employee] PRIMARY KEY ([id]),
    CONSTRAINT [FK_employee_company_company_id] FOREIGN KEY ([company_id]) REFERENCES [company] ([id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'name') AND [object_id] = OBJECT_ID(N'[company]'))
    SET IDENTITY_INSERT [company] ON;
INSERT INTO [company] ([id], [name])
VALUES (1, N'Meta'),
(2, N'Google');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'name') AND [object_id] = OBJECT_ID(N'[company]'))
    SET IDENTITY_INSERT [company] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'company_id', N'first_name', N'last_name') AND [object_id] = OBJECT_ID(N'[employee]'))
    SET IDENTITY_INSERT [employee] ON;
INSERT INTO [employee] ([id], [company_id], [first_name], [last_name])
VALUES (1, 1, N'John', N'Doe'),
(2, 2, N'Will', N'Smith'),
(3, 2, N'Jack', N'Parker');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'id', N'company_id', N'first_name', N'last_name') AND [object_id] = OBJECT_ID(N'[employee]'))
    SET IDENTITY_INSERT [employee] OFF;
GO

CREATE UNIQUE INDEX [IX_company_name] ON [company] ([name]);
GO

CREATE INDEX [IX_employee_company_id] ON [employee] ([company_id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230307201118_InitDb', N'7.0.3');
GO

COMMIT;
GO

