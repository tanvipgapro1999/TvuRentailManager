CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
Begin
set nocount on;
SELECT Id,ProductName,[Description],RentailPrice,QuantityInStock, IsTaxible
	from [dbo].[Product]
	where Id = @Id;
end