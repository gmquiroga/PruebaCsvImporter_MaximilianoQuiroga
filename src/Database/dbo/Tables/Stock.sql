CREATE TABLE [dbo].[Stock](
	[PointOfSale] [varchar](50) NOT NULL,
	[Product] [varchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[Stock] [int] NOT NULL
) ON [PRIMARY]
GO