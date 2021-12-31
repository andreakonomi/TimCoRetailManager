CREATE TABLE [dbo].[SaleDetail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SaleId] INT NOT NULL,
	[ProductId] int not null,
	[Quantity] int not null default 1,
	[PurchasePrice] money not null,
	[Tax] money not null DEFAULT 0
)
