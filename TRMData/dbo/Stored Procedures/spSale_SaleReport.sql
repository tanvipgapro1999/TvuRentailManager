﻿CREATE PROCEDURE [dbo].[spSale_SaleReport]
As
begin 
	set nocount on;

	select [s].[Id], [s].[CashierId], [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], u.FirstName, u.LastName, u.EmailAddress
	from dbo.Sale s
	inner join dbo.[User] u on s.CashierId = u.Id
end