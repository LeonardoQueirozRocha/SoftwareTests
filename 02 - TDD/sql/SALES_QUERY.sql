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

CREATE SEQUENCE [MySequence] AS int START WITH 1000 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
GO

CREATE TABLE [Vouchers] (
    [Id] uniqueidentifier NOT NULL,
    [Code] varchar(100) NOT NULL,
    [VoucherDiscountType] int NOT NULL,
    [DiscountValue] decimal(18,2) NULL,
    [DiscountPercentage] decimal(18,2) NULL,
    [Quantity] int NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    [Active] bit NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_Vouchers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Orders] (
    [Id] uniqueidentifier NOT NULL,
    [Code] int NOT NULL DEFAULT (NEXT VALUE FOR MySequence),
    [CustomerId] uniqueidentifier NOT NULL,
    [VoucherId] uniqueidentifier NULL,
    [TotalValue] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [OrderStatus] int NOT NULL,
    [IsVoucherUsed] bit NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Vouchers_VoucherId] FOREIGN KEY ([VoucherId]) REFERENCES [Vouchers] ([Id])
);
GO

CREATE TABLE [OrderItems] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [ProductName] varchar(250) NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id])
);
GO

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
GO

CREATE INDEX [IX_Orders_VoucherId] ON [Orders] ([VoucherId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240128002607_Final', N'7.0.15');
GO

COMMIT;
GO

