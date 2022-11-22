using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly IProductData _productData;
        private readonly ISqlDataAccess _sql;

        public SaleData(IProductData productData, ISqlDataAccess sql)
        {
            _productData = productData;
            _sql = sql;
        }
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            // TODO: Make this SOLID/DRY/Better
            // Start filling in the models we will save to the database
            List<SaleDetailDBModel> Details = new List<SaleDetailDBModel>();
            var taxRate = ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                var Detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                // Get the information about this product
                var productInfo = _productData.GetProductById(Detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {Detail.ProductId} could not be dound the database.");
                }
                Detail.PurchasePrice = (productInfo.RentailPrice * Detail.Quantity);
                if (productInfo.IsTaxable)
                {
                    Detail.Tax = (Detail.PurchasePrice * taxRate);
                }
                Details.Add(Detail);
            }
            // Fill in the avaiable information
            // Create the Sale Model
            SaleDbModel sale = new SaleDbModel
            {
                SubTotal = Details.Sum(x => x.PurchasePrice),
                Tax = Details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;
            try
            {
                _sql.StartTransaction("TRMData");
                // Save the sale model
                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                // Get the ID from the sale model
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
                // Finish filling in the sale detail models

                foreach (var item in Details)
                {
                    item.SaleId = sale.Id;
                    // Save the sale detail models
                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }

                _sql.CommitTransaction();
            }
            catch
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
        public List<SaleReportModel> GetSaleReport()
        {
            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");
            return output;
        }
    }
}
