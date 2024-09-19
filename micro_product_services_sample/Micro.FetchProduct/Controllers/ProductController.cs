using Dapper;
using Micro.FetchProduct.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace Micro.FetchProduct.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IConfiguration config, ILogger<ProductController> logger)
        {
            _config = config;
            _logger = logger;
        }
        [HttpGet("[action]/{p_code}")]
        public async Task<ActionResult<ProductModel>> GetProduct(string p_code)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            try
            {
                var prods = await connection.QueryAsync<ProductModel>("Select * From Products where ProductCode = @code", new { code = p_code });

                return Ok(prods);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message + ex.InnerException + ex.StackTrace}");
                return Ok(ex);
            }           
        }

        [HttpGet("[action]/{p_code}")]
        public async Task<ActionResult<List<InventoryDataModelByShops>>> GetInventory(string p_code)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            string ppSql = string.Format(@"Select PSZ.ProductSizeName,PSZ.ProductSizeCode,PIY.ProductInventoryID, PIY.ShopID, P.ProductName,P.ProductCode,P.ProductID,
                                            PIS.ProductItemID,PIS.ProductID,PIS.ProductItemName,PIS.ProductItemCode,PIS.ProductItemCodeOld,PIS.ProductSizeID,
                                            PIS.ProductCombinationID,PIS.ProductItemPurchasedCost,PIS.ProductItemSalesCost,PIY.Quantity AS NumberOfItems, S.ShopName
                                            From ProductItems PIS
                                            Inner Join Products P On P.ProductID = PIS.ProductID 
                                            Inner Join ProductInventory PIY On PIY.ProductItemID = PIS.ProductItemID 
                                            Inner Join ProductCombinations PCO On PCO.ProductCombinationID = PIS.ProductCombinationID 
                                            Inner Join ProductSizes PSZ On PSZ.ProductSizeID = PIS.ProductSizeID 
                                            Inner Join Shops S On PIY.ShopID = S.ShopID
                                            Where P.ProductCode = @code");

            var productItems = await connection.QueryAsync<ProductItemModel>(ppSql, new { code = p_code });

            List<InventoryDataModelByShops> inventoryByShop = new List<InventoryDataModelByShops>();
            if (productItems.Count() > 0)
            {
                var combos = await connection.QueryAsync<ProductCombination>("Select * From ProductCombinations");
                var combinations = (List<ProductCombination>)combos;
                short[] shopIDs = productItems.Select(x => x.ShopID).Distinct().ToArray();
                List<ProductBasedDiscount> pbd = new List<ProductBasedDiscount>();
                if (shopIDs.Count() > 0)
                {
                    long product_id = productItems.Select(x => x.ProductID).FirstOrDefault();
                    var result = await connection.QueryAsync<ProductBasedDiscount>(@"Select * From ProductBasedDiscount 
                                    where shopID in @str_shopids and ProductID = @prod_id", new { str_shopids = shopIDs, prod_id = product_id });
                    pbd = (List<ProductBasedDiscount>)result;
                }
                foreach (var shopid in shopIDs)
                {
                    var availableCombinations = productItems.Where(c => c.ShopID == shopid).Select(x => x.ProductCombinationID).Distinct().ToList();
                    var intersectedCombinations = combinations.Where(c => availableCombinations.Contains(c.ProductCombinationID));//.FirstOrDefault());
                    InventoryDataModelByShops inventory = new InventoryDataModelByShops();
                    inventory.InventoryDataModel = new List<InventoryDataModel>();
                    inventory.Inv_shopID = shopid;
                    inventory.DiscountByShop = pbd.Where(x => x.ShopID == shopid).FirstOrDefault();
                    List<InventoryDataModel> combo_list = new List<InventoryDataModel>();

                    foreach (var combination in intersectedCombinations)
                    {
                        InventoryDataModel combo = new InventoryDataModel();
                        combo.ProductCombinationID = combination.ProductCombinationID;
                        combo.ProductCombinationName = combination.ProductCombinationName;
                        combo.ProductCombinationCode = combination.ProductCombinationCode;
                        var pitems = productItems.Where(x => x.ProductCombinationID == combination.ProductCombinationID && x.ShopID == shopid).ToList();
                        combo.ProductItemModel = pitems;
                        //combo.ProductCombinationID2 = pitems.Select(x => x.ProductCombinationID2).FirstOrDefault();
                        inventory.Inv_shopName = pitems.Select(y => y.ShopName).FirstOrDefault();
                        //model.Inv_shopID = model.ProductItemModel.Select(x => x.ShopID).FirstOrDefault();
                        combo.Product_ID = combo.ProductItemModel.Select(x => x.ProductID).FirstOrDefault();
                        combo_list.Add(combo);
                    }
                    inventory.InventoryDataModel.AddRange(combo_list);
                    inventoryByShop.Add(inventory);
                }
            }
            //return inventoryByShop;

            return Ok(inventoryByShop);
        }
    }
}
