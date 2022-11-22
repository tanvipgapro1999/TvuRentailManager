CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
begin
	set nocount on;

	SELECT Id,ProductName,[Description],RentailPrice,QuantityInStock, IsTaxible
	from [dbo].[Product]
	order by ProductName;
end
